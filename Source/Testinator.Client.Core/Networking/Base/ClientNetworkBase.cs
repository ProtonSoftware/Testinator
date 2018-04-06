using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Provides base functionallity for client-side networking
    /// </summary>
    public abstract class ClientNetworkBase : BaseViewModel, IClientNetwork
    {
        #region Public Settings

        /// <summary>
        /// Defeines the number of attempts befere we are timed out from connecting
        /// 0 - infinite
        /// </summary>
        public const uint AttemptsNumberTimeout = 5;

        #endregion

        #region Private Members

        /// <summary>
        /// Buffer size for incoming data
        /// </summary>
        private int mBufferSize;

        /// <summary>
        /// Buffer for received data
        /// </summary>
        private byte[] mReceiverBuffer;

        /// <summary>
        /// Target server ip address 
        /// </summary>
        private IPAddress mIPAddress;

        /// <summary>
        /// Target server port
        /// </summary>
        private int mPort;

        /// <summary>
        /// Indicates if the connection loop is in progress, used to prevent from multiple connection proccess running at this same time
        /// </summary>
        private bool mConnetionLoopInProgress;

        /// <summary>
        /// Socket that handles communication
        /// </summary>
        private Socket mClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Maximum number of attempts based on the <see cref="AttemptsNumberTimeout"/> constant
        /// </summary>
        private uint MaxAttempts => AttemptsNumberTimeout == 0 ? uint.MaxValue : AttemptsNumberTimeout;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the client is trying to connect to the server
        /// </summary>
        public bool IsTryingToConnect { get; private set; }

        /// <summary>
        /// Indicates if the client is connected to the server
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets and sets server buffer size
        /// NOTE: If client is connected or is trying to connect buffer size change will NOT be saved
        /// </summary>
        public int BufferSize
        {
            get => mBufferSize;
            set
            {
                if (!IsTryingToConnect && !IsConnected)
                {
                    mBufferSize = value;
                    mReceiverBuffer = new byte[BufferSize];
                }
            }
        }

        /// <summary>
        /// Gets and sets client ip
        /// NOTE: If client is connected or is trying to connect ip address change will NOT be saved
        /// </summary>
        public IPAddress IPAddress
        {
            get => mIPAddress;
            set
            {
                if (!IsTryingToConnect && !IsConnected)
                    mIPAddress = value;
            }
        }

        /// <summary>
        /// Gets client ip as a string
        /// </summary>
        public string IPString => IPAddress.ToString();

        /// <summary>
        /// Gets and sets client port
        /// NOTE: If client is connected or is trying to connect port change will NOT be saved
        /// </summary>
        public int Port
        {
            get => mPort;
            set
            {
                if (!IsTryingToConnect && !IsConnected)
                    mPort = value;
            }
        }

        /// <summary>
        /// Indicates the proccess of cancelling a conncetion attempt, in that time no new connections can be started
        /// </summary>
        public bool CancellingConncetion { get; private set; }

        /// <summary>
        /// Number of attempts taken to connect to the server
        /// </summary>
        public int Attempts { get; private set; } = 0;

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when client establishes connection with the server
        /// </summary>
        public event Action Connected = () => { };

        /// <summary>
        /// Fired when client is disconnected from the server
        /// </summary>
        public event Action Disconnected = () => { };

        /// <summary>
        /// Fired when maximum connections attempt is reachd
        /// </summary>
        public event Action AttemptsTimeout = () => { };

        /// <summary>
        /// Fired when attempting to connect to the server is completed
        /// </summary>
        public event Action ConnectionFinished = () => { };

        /// <summary>
        /// Fired when there is data received from the server
        /// </summary>
        public event Action<DataPackage> DataReceived = (data) => { };

        /// <summary>
        /// Fired when <see cref="Attempts"/> counter updates 
        /// </summary>
        public event Action AttemptCounterUpdated = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientNetworkBase()
        {
            // Create default values
            BufferSize = 32768;
            Port = 3333;
            IPAddress = NetworkHelpers.GetLocalIPAddress();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the connecting sequence
        /// </summary>
        public async void Connect()
        {
            if (IsConnected || IsTryingToConnect || mConnetionLoopInProgress)
                return;

            // Create new client socket in case the previous one has been disposed
            mClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IsTryingToConnect = true;
            Attempts = 0;

            OnConnectionStarted();

            // Spin up the connecting loop and wait for it to complete either by successful connection trial or by the Stop call which sets the IsTryingToConnect flag to false
            await Task.Run(() =>
            {
                try
                {
                    TryToConnectLoop();
                }
                catch (AttemptNumberTimeoutException)
                {
                    AttemptsTimeout.Invoke();
                }
            });

            // If we're connected...
            if (mClientSocket.Connected)
            {
                // If we connected to the server right after the Stop() call, disconnect immediately and close the socket
                if (CancellingConncetion)
                {
                    mClientSocket.Shutdown(SocketShutdown.Both);

                    IsConnected = false;
                }

                // Otherwise, everything went perfectly fine
                else
                {
                    IsConnected = true;

                    // Start receiving
                    mClientSocket.BeginReceive(mReceiverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, mClientSocket);

                    // Let them know we have have connected to the server
                    OnConnectionEstablished();

                    Connected.Invoke();
                }
            }

            // If connection failed...
            else
                IsConnected = false;


            // No matter what
            IsTryingToConnect = false;
            CancellingConncetion = false;

            ConnectionFinished.Invoke();
        }

        /// <summary>
        /// Either disconnects from the server or stops connecting to the server
        /// </summary>
        public void Disconnect()
        {
            OnDisconnecting();

            if (IsConnected)
            {
                // Tell the server that we want to disconnect
                SendData(new DataPackage(PackageType.DisconnectRequest, null));

                // Shutdown the socket
                mClientSocket.Shutdown(SocketShutdown.Both);
            }

            CancellingConncetion = true;
        }

        /// <summary>
        /// Sets data to the server, if connected
        /// </summary>
        /// <param name="data">Data to be sent</param>
        public void SendData(DataPackage data)
        {
            if (!IsConnected)
                return;

            if (DataPackageDescriptor.TryConvertToBin(out var sendBuffor, data))
            {
                // Send it if conversion was successful
                mClientSocket.Send(sendBuffor, 0, sendBuffor.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// Initializes the client with the given data
        /// </summary>
        /// <param name="ip">Ip of the sever the client will attempt to connect to</param>
        /// <param name="port">Server port</param>
        public void Initialize(string ip, int port)
        {
            IPAddress = IPAddress.Parse(ip);
            Port = port;
        }

        /// <summary>
        /// Initializes the client with the given data
        /// </summary>
        /// <param name="ip">Ip of the sever the client will attempt to connect to</param>
        /// <param name="port">Server port</param>
        public void Initialize(IPAddress ip, int port)
        {
            IPAddress = ip;
            Port = port;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fired when starts a new connection attempt to the remote server
        /// </summary>
        protected virtual void OnConnectionStarted() { }

        /// <summary>
        /// Fired when connection is established with the remote server
        /// </summary>
        protected virtual void OnConnectionEstablished() { }

        /// <summary>
        /// Fired when connection with the remote server is lost
        /// </summary>
        protected virtual void OnConnectionLost() { }

        /// <summary>
        /// Fired when the client gets disconnected from the server
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Fired when the user attempts to disconnect from the server
        /// </summary>
        protected virtual void OnDisconnecting() { }

        /// <summary>
        /// Fied when there has been data received from the remote server
        /// </summary>
        /// <param name="data">The received data</param>
        protected virtual void OnDataReceived(DataPackage data) { }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Tries to connected to the server in a blocking pooling mode
        /// </summary>
        /// <returns>True if attempting to connect was successful; otherwise, false</returns>
        private void TryToConnectLoop()
        {
            mConnetionLoopInProgress = true;

            // Attempt to connect until we get the connection working or we're told to stop
            while (!mClientSocket.Connected && !CancellingConncetion)
            {
                try
                {
                    // Try to connect, if failed try again
                    Attempts++;
                    AttemptCounterUpdated.Invoke();

                    // Attempt to connect
                    mClientSocket.Connect(IPAddress, Port);

                }

                // Connection failed, try again...
                catch (SocketException)
                { }

                // Check if the max attempts has been reached only if connection failed
                if (!mClientSocket.Connected && Attempts >= MaxAttempts)
                {
                    mConnetionLoopInProgress = false;
                    throw new AttemptNumberTimeoutException();
                }

            }

            mConnetionLoopInProgress = false;
        }

        /// <summary>
        /// Called when data is received
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            // Number of bytes received
            int received;

            try
            {
                received = mClientSocket.EndReceive(ar);
            }
            // Exeption is caused by some connection problem
            catch (SocketException)
            {
                // Disconnect from the server in this case 
                IsConnected = false;

                // Let them know we have been disconnected
                OnConnectionLost();

                Disconnected.Invoke();

                return;
            }

            // Copy the received buffer
            var recBuf = new byte[received];
            Array.Copy(mReceiverBuffer, recBuf, received);

            // Try to get the data
            if (DataPackageDescriptor.TryConvertToObj<DataPackage>(recBuf, out var PackageReceived))
            {
                // If we are told to disconnect
                if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    // Close the socket
                    mClientSocket.Shutdown(SocketShutdown.Both);
                    IsConnected = false;

                    // Let listeners know we have beed disconnected
                    OnDisconnected();
                    Disconnected.Invoke();
                }
                else
                {
                    // Call the subscribed methods

                    OnDataReceived(PackageReceived);
                    DataReceived.Invoke(PackageReceived);
                }
            }

            // If we are still connected start receiving again
            if (IsConnected)
                mClientSocket.BeginReceive(mReceiverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, mClientSocket);
        }




        #endregion
    }
}
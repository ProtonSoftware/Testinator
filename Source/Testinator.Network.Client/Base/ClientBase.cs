using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Testinator.Core;

namespace Testinator.Network.Client
{
    public class ClientBase : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Buffer size for incoming data
        /// </summary>
        private int mBufferSize;

        /// <summary>
        /// Client ip address
        /// </summary>
        private IPAddress mIPAddress;

        /// <summary>
        /// Port to connect to the server
        /// </summary>
        private int mPort;

        /// <summary>
        /// Socket that handles communication
        /// </summary>
        private Socket mClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Buffer for received data
        /// </summary>
        private byte[] mReceiverBuffer;

        #endregion

        #region Private Methods

        /// <summary>
        /// Tries to connected to the server
        /// </summary>
        private void TryConnecting()
        {
            while (!IsConnected && Connecting)
            {
                try
                {
                    // Try to connect, if failed try again
                    Attempts++;
                    OnAttemptUpdate.Invoke();
                    mClientSocket.Connect(IPAddress, Port);
                }
                catch (SocketException)
                { }
            }

            Connecting = false;

            if (IsConnected)
            {
                // Start receiving
                mClientSocket.BeginReceive(mReceiverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, mClientSocket);

                // Let them know we have connected to the server
                OnConnected.Invoke();
            }
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
            catch
            {
                Disconnect();

                // Let them know we have been disconnected
                OnDisconnected.Invoke();

                return;
            }

            // Copy the received buffer
            var recBuf = new byte[received];
            Array.Copy(mReceiverBuffer, recBuf, received);

            // Try to get the data
            if (DataPackageDescriptor.TryConvertToObj(recBuf, out var PackageReceived))
            {
                // If we are told to disconnect
                if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    // Close the socket
                    mClientSocket.Shutdown(SocketShutdown.Both);

                    // Let listeners know we have beed disconnected
                    OnDisconnected.Invoke();
                }
                else
                {
                    // Call the subscribed method
                    OnDataReceived.Invoke(PackageReceived);
                }

            }

            // If we are still connected start receiving again
            if (IsConnected)
                mClientSocket.BeginReceive(mReceiverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, mClientSocket);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the client is trying to connect to the server
        /// </summary>
        public bool Connecting { get; private set; }

        /// <summary>
        /// Indicates if the client is connected to the server
        /// </summary>
        public bool IsConnected => mClientSocket.Connected;

        /// <summary>
        /// Gets and sets server buffer size
        /// NOTE: If client is connected or is trying to connect buffer size change will NOT be saved
        /// </summary>
        public int BufferSize
        {
            get => mBufferSize;
            set
            {
                if (!Connecting && !IsConnected)
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
                if (!Connecting && !IsConnected)
                    mIPAddress = value;
            }
        }

        /// <summary>
        /// Gets client ip as a string
        /// </summary>
        public string Ip => IPAddress.ToString();

        /// <summary>
        /// Gets and sets client port
        /// NOTE: If client is connected or is trying to connect port change will NOT be saved
        /// </summary>
        public int Port
        {
            get => mPort;
            set
            {
                if (!Connecting && !IsConnected)
                    mPort = value;
            }
        }

        /// <summary>
        /// Number of attempts taken to connect to the server
        /// </summary>
        public int Attempts { get; private set; } = 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the connecting sequence
        /// </summary>
        public void StartConnecting()
        {
            if (IsConnected || Connecting)
                return;

            // Create new client socket
            mClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); ;

            // Start a new thread that handles the try-to-connect loop
            var connectingThread = new Thread(new ThreadStart(TryConnecting))
            {
                IsBackground = true,
                Name = "ConnectingThread"
            };

            Connecting = true;
            Attempts = 0;
            connectingThread.Start();
        }

        /// <summary>
        /// Either disconnects from the server or stops connecting to the server
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                // Tell the server that we want to disconnect
                SendData(new DataPackage(PackageType.DisconnectRequest, null));

                // Shutdown the socket
                mClientSocket.Shutdown(SocketShutdown.Both);
            }
            Connecting = false;
        }

        /// <summary>
        /// Sets data to the server, if connected
        /// </summary>
        /// <param name="data">Data to be sent</param>
        public void SendData(DataPackage data)
        {
            // Try convert data to the binary array and chceck if we are stil connected
            if (IsConnected && DataPackageDescriptor.TryConvertToBin(out var sendBuffor, data))
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

        #endregion

        #region Public Events

        /// <summary>
        /// Method to be called any data is received from a client
        /// </summary>
        public event Action<DataPackage> OnDataReceived = (data) => { };

        /// <summary>
        /// Method to be called when client connects to the server
        /// </summary>
        public event Action OnConnected = () => { };

        /// <summary>
        /// Method to be called when client disconnects from the server
        /// </summary>
        public event Action OnDisconnected = () => { };

        /// <summary>
        /// Method to be called when Attempts counter is updated
        /// /// </summary>
        public event Action OnAttemptUpdate = () => { };

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientBase()
        {
            // Create default values
            BufferSize = 32768;
            Port = 3333;
            IPAddress = NetworkHelpers.GetLocalIPAddress();

            Connecting = false;
            Attempts = 0;
        }

        #endregion
    }
}
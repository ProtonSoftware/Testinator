using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Testinator.Core;

namespace Testinator.Network.Client
{
    public abstract class ClientBase
    {
        #region Public Properties

        /// <summary>
        /// Indicates if the client is trying to connect to the server
        /// </summary>
        public bool Connecting => _Connecting;

        /// <summary>
        /// Indicates if the client is connected to the server
        /// </summary>
        public bool IsConnected => clientSocket.Connected;

        /// <summary>
        /// Gets and sets server buffer size
        /// NOTE: If client is connected or is trying to connect buffer size change will NOT be saved
        /// </summary>
        public int BufferSize
        {
            get => _BufferSize;
            set
            {
                if (!Connecting && !IsConnected)
                {
                    _BufferSize = value;
                    ReceiverBuffer = new byte[BufferSize];
                }
            }
        }

        /// <summary>
        /// Gets and sets client ip
        /// NOTE: If client is connected or is trying to connect ip address change will NOT be saved
        /// </summary>
        public IPAddress IPAddress
        {
            get => _IPAddress;
            set
            {
                if (!Connecting && !IsConnected)
                    _IPAddress = value;
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
            get => _Port;
            set
            {
                if (!Connecting && !IsConnected)
                    _Port = value;
            }
        }

        /// <summary>
        /// Number of attempts taken to connect to the server
        /// </summary>
        public int Attempts => _Attempts;

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Starts the connecting sequence
        /// </summary>
        public void StartConnecting()
        {
            if (IsConnected || Connecting)
                return;


            Thread connectingThread = new Thread(new ThreadStart(TryConnecting))
            {
                IsBackground = true,
                Name = "ConnectingThread"
            };

            _Connecting = true;
            connectingThread.Start();
        }

        /// <summary>
        /// Either disconnects from the server or stops connecting to the server
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                SendData(new DataPackage("me", PackageType.DisconnectRequest, null));
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            _Connecting = false;
        }

        public void SendData(DataPackage data)
        {
            if (DataPackageDescriptor.TryConvertToBin(out byte[] sendBuffor, data))
            {
                clientSocket.Send(sendBuffor, 0, sendBuffor.Length, SocketFlags.None);
            }
        }

        #endregion

        #region Public Delegates

        /// <summary>
        /// Delegate to the method to be called when data is received
        /// </summary>
        /// <param name="data">Data received</param>
        public delegate void DataReceivedDelegate(DataPackage data);

        /// <summary>
        /// Fired when the clinet connects to the server
        /// </summary>
        public delegate void ConnectedDelegate();

        /// <summary>
        /// Fired when a client disconnects
        /// </summary>
        public delegate void ClientDisconnectedDelegate();

        /// <summary>
        /// Method to be called any data is received from a client
        /// </summary>
        public DataReceivedDelegate DataRecivedCallback { get; set; }

        /// <summary>
        /// Method to be called when client connects to the server
        /// </summary>
        public ConnectedDelegate ConnectedCallback { get; set; }

        /// <summary>
        /// Method to be called when client dissconnects from the server
        /// </summary>
        public ClientDisconnectedDelegate DisconnectedCallback { get; set; }

        #endregion

        #region Protected Members

        /// <summary>
        /// Used to connect to the sever
        /// </summary>
        protected Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Buffer for received data
        /// </summary>
        protected byte[] ReceiverBuffer;

        #endregion

        #region Protected Abstract Methods

        protected abstract void ReciveCallback(IAsyncResult ar);

        #endregion

        #region Private Members

        /// <summary>
        /// Indicates if the client is trying to connect to the server
        /// </summary>
        private bool _Connecting;

        /// <summary>
        /// Buffer size for incoming data
        /// </summary>
        private int _BufferSize;

        /// <summary>
        /// Client ip address
        /// </summary>
        private IPAddress _IPAddress;

        /// <summary>
        /// Port to connect to the server
        /// </summary>
        private int _Port;

        /// <summary>
        /// Number of attempts taken to connect to the server
        /// </summary>
        private int _Attempts;

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
                    _Attempts++;
                    clientSocket.Connect(IPAddress, Port);
                }
                catch (SocketException)
                { }
            }
            if (IsConnected)
            {
                clientSocket.BeginReceive(ReceiverBuffer, 0, BufferSize, SocketFlags.None, ReciveCallback, clientSocket);

                // Let them know we have connected to the server
                ConnectedCallback();
            }
            _Connecting = false;
        }

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
            IPAddress = IPAddress.Parse("127.0.0.1");

            _Connecting = false;
            _Attempts = 0;
        }

        #endregion

    }
}

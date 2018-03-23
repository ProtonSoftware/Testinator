using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Testinator.Core;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Foundation of any server
    /// </summary>
    public abstract class ServerBase
    {
        #region Protected Memebers

        /// <summary>
        /// The socked this server is operating on
        /// </summary>
        protected Socket mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Matches all connected sockets with their client models
        /// </summary>
        protected readonly Dictionary<Socket, NetworkClientModel> mClients = new Dictionary<Socket, NetworkClientModel>();

        #endregion

        #region Private Members

        /// <summary>
        /// Buffer for received data
        /// </summary>
        private byte[] mReciverBuffer;

        /// <summary>
        /// Buffer size for incoming data
        /// </summary>
        private int mBufferSize;

        /// <summary>
        /// Server ip address
        /// </summary>
        private IPAddress mIPAddress;

        /// <summary>
        /// Server port
        /// </summary>
        private int mPort;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the server is currently running
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets and sets server buffer size
        /// NOTE: If server is running buffer size change will NOT be saved
        /// </summary>
        public int BufferSize
        {
            get => mBufferSize;
            set
            {
                if (!IsRunning)
                {
                    mBufferSize = value;
                    mReciverBuffer = new byte[BufferSize];
                }
            }
        }

        /// <summary>
        /// Gets and sets server ip
        /// NOTE: If server is running ip change will NOT be saved
        /// </summary>
        public IPAddress IP
        {
            get => mIPAddress;
            set
            {
                if (!IsRunning)
                    mIPAddress = value;
            }
        }

        /// <summary>
        /// Gets server ip as a string
        /// </summary>
        public string IPString => IP.ToString();

        /// <summary>
        /// Gets and sets server port
        /// NOTE: If server is running port change will NOT be saved
        /// </summary>
        public int Port
        {
            get => mPort;
            set
            {
                if (!IsRunning)
                    mPort = value;
            }
        }

        /// <summary>
        /// Number of clients curently connected
        /// </summary>
        public int ConnectedClientsCount => mClients.Count;

        #endregion

        #region Public Events

        /// <summary>
        /// The event that is fired when any data has been recived from a client
        /// </summary>
        public event Action<NetworkClientModel, DataPackage> OnDataRecived = (sender, data) => { };

        /// <summary>
        /// The event that is fired when a new client has connected
        /// </summary>
        public event Action<NetworkClientModel> OnClientConnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client has disconnected
        /// </summary>
        public event Action<NetworkClientModel> OnClientDisconnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client's data had beed updated
        /// </summary>
        public event Action<NetworkClientModel, NetworkClientModel> OnClientDataUpdated = (oldmodel, newModel) => { };

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            // Initialize 
            try
            {
                mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mServerSocket.Bind(new IPEndPoint(IP, Port));
                mServerSocket.Listen(0);
                mServerSocket.BeginAccept(AcceptCallback, null);
                IsRunning = true;
            }
            catch
            {
                IsRunning = false;
                // TODO: error handling
            }

            OnStartUp();
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Called on server startup
        /// </summary>
        protected virtual void OnStartUp() { }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerBase()
        {
            // Create default values
            BufferSize = 32768;
            Port = 3333;
            IP = NetworkHelpers.GetLocalIPAddress();
        }

        #endregion


    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Testinator.Core;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Provides basic functionalities for network server
    /// </summary>
    public abstract class ServerBase
    {
        #region Protected Members

        /// <summary>
        /// The <see cref="Socket"/> for server
        /// </summary>
        protected Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Conatins all connected clients
        /// </summary>
        protected readonly Dictionary<Socket, ClientModel> Clients = new Dictionary<Socket, ClientModel>();

        /// <summary>
        /// Buffer for received data
        /// </summary>
        protected byte[] ReciverBuffer;

        #endregion

        #region Private Members

        /// <summary>
        /// Indicates if the server is currently running
        /// </summary>
        private bool _IsRunning = false;

        /// <summary>
        /// Buffer size for incoming data
        /// </summary>
        private int _BufferSize;

        /// <summary>
        /// Server ip address
        /// </summary>
        private IPAddress _IPAddress;

        /// <summary>
        /// Server port
        /// </summary>
        private int _Port;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the server is currently running
        /// </summary>
        public bool IsRunning => _IsRunning;

        /// <summary>
        /// Gets and sets server buffer size
        /// NOTE: If server is running buffer size change will NOT be saved
        /// </summary>
        public int BufferSize
        {
            get => _BufferSize;
            set
            {
                if (!IsRunning)
                {
                    _BufferSize = value;
                    ReciverBuffer = new byte[BufferSize];
                }
            }
        }

        /// <summary>
        /// Gets and sets server ip
        /// NOTE: If server is running ip change will NOT be saved
        /// </summary>
        public IPAddress IPAddress
        {
            get => _IPAddress;
            set
            {
                if (!IsRunning)
                    _IPAddress = value;
            }
        }

        /// <summary>
        /// Gets and sets server ip
        /// NOTE: If server is running ip change will NOT be saved
        /// If ip is incorrect no changed will be made
        /// </summary>
        public string Ip
        {
            get => IPAddress.ToString();
            set
            {
                try
                {
                    IPAddress = IPAddress.Parse(value);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets and sets server port
        /// NOTE: If server is running port change will NOT be saved
        /// </summary>
        public int Port
        {
            get => _Port;
            set
            {
                if (!IsRunning)
                    _Port = value;
            }
        }

        /// <summary>
        /// Number of clients curently connected
        /// </summary>
        public int ConnectedClientCount => Clients.Count;

        #endregion

        #region Public Delegates

        /// <summary>
        /// Delegate to the method to be called when data is received
        /// </summary>
        /// <param name="data">Data received</param>
        public delegate void DataReceivedDelegate(ClientModel sender, DataPackage data);

        /// <summary>
        /// Fired when a new client is connected
        /// </summary>
        public delegate void ClientConnectedDelegate(ClientModel sender);

        /// <summary>
        /// Fired when a client disconnects
        /// </summary>
        public delegate void ClientDisconnectedDelegate(ClientModel sender);

        /// <summary>
        /// Method to be called any data is recived from a client
        /// </summary>
        public DataReceivedDelegate DataRecivedCallback { get; set; }

        /// <summary>
        /// Method to be called when a new client is connected
        /// </summary>
        public ClientConnectedDelegate ClientConnectedCallback { get; set; }

        /// <summary>
        /// Method to be called when a client is disconnected
        /// </summary>
        public ClientDisconnectedDelegate ClientDisconnectedCallback { get; set; }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Called when there is a client to be accepted
        /// </summary>
        /// <param name="ar">Parameters</param>
        protected abstract void AcceptCallback(IAsyncResult ar);

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        protected abstract void ReceiveCallback(IAsyncResult ar);

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            if (_IsRunning)
                return;
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress, Port));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                _IsRunning = true;
            }
            catch
            {
                _IsRunning = false;
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            if (!_IsRunning)
                return;

            if (!DataPackageDescriptor.TryConvertToBin(out byte[] buf, new DataPackage(PackageType.DisconnectRequest, null)))
                throw new NotImplementedException();

            // Close all connections
            foreach (Socket item in Clients.Keys.ToList())
            {
                item.Send(buf, 0, buf.Length, SocketFlags.None);
                item.Shutdown(SocketShutdown.Both);
                item.Close();
                Clients.Remove(item);
            }

            serverSocket.Close();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _IsRunning = false;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerBase()
        {
            // Create default values
            BufferSize = 32768;
            Port = 3333;
            IPAddress = IpHelpers.GetLocalIPAddress();
        }

        #endregion
    }
}

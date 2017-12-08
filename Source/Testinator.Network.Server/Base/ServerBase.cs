using System;
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
    public class ServerBase
    {
        #region Private Members

        /// <summary>
        /// The <see cref="Socket"/> for server
        /// </summary>
        private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Conatins all connected clients
        /// </summary>
        private readonly Dictionary<Socket, ClientModel> Clients = new Dictionary<Socket, ClientModel>();

        /// <summary>
        /// Keeps track of id's given to each user
        /// </summary>
        private Dictionary<string, string> MacId = new Dictionary<string, string>();

        /// <summary>
        /// Buffer for received data
        /// </summary>
        private byte[] ReciverBuffer;

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

        #region Private Methods

        /// <summary>
        /// Gets called when there is a client to be accepted
        /// </summary>
        /// <param name="ar">Parameters</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket clientSocket;

            try
            {
                clientSocket = serverSocket.EndAccept(ar);
            }
            catch
            {
                return;
            }

            // Accept this client and start reciving, but until the client doesn't not provide an info package, dont call the higher appliaction level
            Clients.Add(clientSocket, null);
            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            int recived;

            try
            {
                recived = clientSocket.EndReceive(ar);
            }
            catch (SocketException)
            {
                // Client is forcefully disconnected
                clientSocket.Shutdown(SocketShutdown.Both);

                clientSocket.Close();

                // Let them know the client has disconnected
                OnClientDisconnected(Clients[clientSocket]);

                // NOTE: remove client after calling the callback method above
                Clients.Remove(clientSocket);

                return;
            }
            catch (Exception)
            {
                return;
            }

            byte[] recBuf = new byte[recived];
            Array.Copy(ReciverBuffer, recBuf, recived);

            if (DataPackageDescriptor.TryConvertToObj(recBuf, out DataPackage PackageReceived))
            {

                // Everything exepct from info packet and disconnect request packet is going to the higher level layer of the appliaction
                if (PackageReceived.PackageType == PackageType.Info)
                {
                    var content = (PackageReceived.Content as InfoPackage);
                    if (content == null)
                        return;

                    string clientId = string.Empty;

                    // If this client provides information about themselves for the first time...
                    if (Clients[clientSocket] == null)

                        // If the user is known to the server don't make new id for them
                        if (MacId.TryGetValue(content.MacAddress, out string givenId))
                            clientId = givenId;

                        // If the user is connecting for the first time get them new id and save it
                        else
                        {
                            clientId = ClientIdProvider.GetId();
                            MacId.Add(content.MacAddress, clientId);
                        }

                    else
                        clientId = Clients[clientSocket].ID;

                    // Construct new client model to either update information about the client or create new client
                    var model = new ClientModel()
                    {
                        ID = clientId,
                        MachineName = content.MachineName,
                        ClientName = content.ClientName,
                        ClientSurname = content.ClientSurname,
                        IpAddress = clientSocket.GetIp(),
                        MacAddress = content.MacAddress,
                    };

                    // If it's a new client...
                    if (Clients[clientSocket] == null)
                    {
                        // Tell listeners that we got a new client
                        OnClientConnected(model);
                    }

                    // The model is updated...
                    if (Clients[clientSocket] != null && !Clients[clientSocket].Equals(model))
                    {
                        // Call the subscribed method
                        //ClientDataUpdatedCallback(Clients[clientSocket], model);
                    }

                    // Update client model
                    Clients[clientSocket] = model;
                }
                else if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                    // Let them know the client has disconnected
                    OnClientDisconnected(Clients[clientSocket]);

                    // NOTE: remove client after calling the event method above
                    Clients.Remove(clientSocket);

                    // Prevents running callback and starting reciving from not existing socket
                    return;
                }
                else
                    // Call the subscribed method only if the package description was successful
                    OnDataRecived(Clients[clientSocket], PackageReceived);
            }

            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);

        }

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

        #region Public Events

        /// <summary>
        /// Handler to the method to be called when data is received
        /// </summary>
        /// <param name="sender">Client that raised this event</param>
        /// <param name="data">Data received</param>
        public delegate void DataReceivedHandler(ClientModel sender, DataPackage data);

        /// <summary>
        /// Fired when a new client is connected
        /// <param name="sender">Client that raised this event</param>
        /// </summary>
        public delegate void ClientConnectedHandler(ClientModel sender);

        /// <summary>
        /// Handler to the method to be called when a client disconnects
        /// </summary>
        /// <param name="sender">Client that raised this event</param>
        public delegate void ClientDisconnectedHandler(ClientModel sender);

        /// <summary>
        /// Handler to the method to be called when a client's data is updated
        /// </summary>
        /// <param name="oldModel">Old data</param>
        /// <param name="newModel">New data</param>
        public delegate void ClientDataUpdatedHandler(ClientModel oldModel, ClientModel newModel);

        /// <summary>
        /// The event that is fired when any data has been recived from a client
        /// </summary>
        public event DataReceivedHandler OnDataRecived = (sender, data) => { };

        /// <summary>
        /// The event that is fired when a new client has connected
        /// </summary>
        public event ClientConnectedHandler OnClientConnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client has disconnected
        /// </summary>
        public event ClientDisconnectedHandler OnClientDisconnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client's data had beed updated
        /// </summary>
        public event ClientDataUpdatedHandler OnClientDataUpdated = (oldModel, newModel) => { };

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Called when there is a client to be accepted
        /// </summary>
        /// <param name="ar">Parameters</param>
        //protected abstract void AcceptCallback(IAsyncResult ar);

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        //protected abstract void ReceiveCallback(IAsyncResult ar);

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
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress, Port));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                _IsRunning = true;
            }
            catch
            {
                _IsRunning = false;
                // TODO: error handling
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            if (!_IsRunning)
                return;

            // Create a package that contains disconnect info 
            // Sent to the clients
            var data = new DataPackage(PackageType.DisconnectRequest, null);

            // Close all connections
            foreach (Socket item in Clients.Keys.ToList())
            {
                item.SendPackage(data);
                item.Shutdown(SocketShutdown.Both);
                item.Close();
                Clients.Remove(item);
            }

            serverSocket.Close();
            _IsRunning = false;
        }

        /// <summary>
        /// Sends data to the client 
        /// </summary>
        /// <param name="target">Target client</param>
        /// <param name="data">Data to be sent</param>
        public void SendData(ClientModel target, DataPackage data)
        {
            var targetSocket = Clients.FirstOrDefault(x => x.Value == target).Key;

            // It target does not exist return
            if (targetSocket == null)
                return;

            try
            {
                targetSocket.SendPackage(data);
            }
            catch { }
            // TODO: error handling 
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
            IPAddress = NetworkHelpers.GetLocalIPAddress();
        }

        #endregion
    }
}
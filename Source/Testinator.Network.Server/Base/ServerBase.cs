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

        #region Private Helpers 

        /// <summary>
        /// Checks if the given socket has a <see cref="ClientModel"/> associated with it
        /// </summary>
        /// <param name="client"></param>
        /// <returns>True it the model exists, false if it does not</returns>
        private bool ClientModelExists(Socket client)
        {
            return Clients[client] != null;
        }
        
        /// <summary>
        /// Updates client model associated with the given socket
        /// </summary>
        /// <param name="Client">Socket whose model needs to be updated</param>
        /// <param name="NewModel">New data as <see cref="InfoPackage"/></param>
        private void UpdateClientModel(Socket Client, InfoPackage NewModel)
        {
            Clients[Client].ClientName = NewModel.ClientName;
            Clients[Client].ClientSurname = NewModel.ClientSurname;
            Clients[Client].MachineName = NewModel.MachineName;
        }

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
                // Try to get the newly connected socket
                clientSocket = serverSocket.EndAccept(ar);
            }
            catch
            {
                // No need to handle any errors for now 
                return;
            }

            // Accept this client and start reciving, but until the client doesn't not provide an info package, dont call the higher appliaction level
            Clients.Add(clientSocket, null);

            // Begin reciving on this socket
            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);

            // Continue accepting new clients
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket senderSocket = (Socket)ar.AsyncState;
            int recivedCount;

            try
            {
                recivedCount = senderSocket.EndReceive(ar);
            }
            catch (SocketException)
            {
                // Client is forcefully disconnected
                senderSocket.Shutdown(SocketShutdown.Both);
                senderSocket.Close();

                // Let them know the client has disconnected
                OnClientDisconnected.Invoke((Clients[senderSocket]));

                // NOTE: remove client after calling the callback method above
                Clients.Remove(senderSocket);

                return;
            }
            catch (Exception)
            {
                return;
            }

            // Get the recived data
            byte[] recBuf = new byte[recivedCount];
            Array.Copy(ReciverBuffer, recBuf, recivedCount);

            if (DataPackageDescriptor.TryConvertToObj(recBuf, out DataPackage PackageReceived))
            {
                // Everything exepct from info packet and disconnect request packet is going to the higher level layer of the appliaction
                if (PackageReceived.PackageType == PackageType.Info)
                {
                    var content = (PackageReceived.Content as InfoPackage);
                    if (content == null)
                        return;

                    string clientId = string.Empty;

                    // If it is a new user...
                    if (!ClientModelExists(senderSocket))
                    {
                        // Get them an id
                        clientId = ClientIdProvider.GetId(content.MacAddress);

                        // Construct new client model 
                        var model = new ClientModel()
                        {
                            ID = clientId,
                            MachineName = content.MachineName,
                            ClientName = content.ClientName,
                            ClientSurname = content.ClientSurname,
                            IpAddress = senderSocket.GetIp(),
                            MacAddress = content.MacAddress,
                        };

                        // Store client model
                        Clients[senderSocket] = model;

                        // Tell listeners that we got a new client
                        OnClientConnected.Invoke((model));
                    }
                    // Otherwise, update client model
                    else
                    {
                        // Update client model
                        UpdateClientModel(senderSocket, content);

                        // Call the subscribed methods
                        OnClientDataUpdated.Invoke(Clients[senderSocket]);
                    }
                }
                else if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    // Close the socket
                    senderSocket.Shutdown(SocketShutdown.Both);
                    senderSocket.Close();

                    // Let them know the client has disconnected
                    OnClientDisconnected(Clients[senderSocket]);

                    // NOTE: remove client after calling the event method above
                    Clients.Remove(senderSocket);

                    // Return here to prevent running callback and starting reciving from not existing socket
                    return;
                }
                else
                    // Call the subscribed method only if the package description was successful
                    OnDataRecived(Clients[senderSocket], PackageReceived);
            }

            // Continue reciving
            senderSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, senderSocket);

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
        /// The event that is fired when any data has been recived from a client
        /// </summary>
        public event Action<ClientModel, DataPackage> OnDataRecived = (sender, data) => { };

        /// <summary>
        /// The event that is fired when a new client has connected
        /// </summary>
        public event Action<ClientModel> OnClientConnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client has disconnected
        /// </summary>
        public event Action<ClientModel> OnClientDisconnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client's data had beed updated
        /// </summary>
        public event Action<ClientModel> OnClientDataUpdated = (newModel) => { };

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
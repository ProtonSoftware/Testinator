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
        private Socket mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Conatins all connected clients
        /// </summary>
        private readonly Dictionary<Socket, ClientModel> mClients = new Dictionary<Socket, ClientModel>();

        /// <summary>
        /// Buffer for received data
        /// </summary>
        private byte[] mReciverBuffer;

        /// <summary>
        /// Indicates if the server is currently running
        /// </summary>
        private bool mIsRunning = false;

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

        #region Private Helpers 

        /// <summary>
        /// Checks if the given socket has a <see cref="ClientModel"/> associated with it
        /// </summary>
        /// <param name="client"></param>
        /// <returns>True it the model exists, false if it does not</returns>
        private bool ClientModelExists(Socket client)
        {
            return mClients[client] != null;
        }
        
        /// <summary>
        /// Updates client model associated with the given socket
        /// </summary>
        /// <param name="Client">Socket whose model needs to be updated</param>
        /// <param name="NewModel">New data as <see cref="InfoPackage"/></param>
        /// <returns>Original model that has been replaced</returns>
        private ClientModel UpdateClientModel(Socket Client, InfoPackage NewModel)
        {
            var originalModel = new ClientModel(mClients[Client]);

            mClients[Client].ClientName = NewModel.ClientName;
            mClients[Client].ClientSurname = NewModel.ClientSurname;
            mClients[Client].MachineName = NewModel.MachineName;

            return originalModel;
        }

        /// <summary>
        /// Checks if the <see cref="InfoPackage"/>'s content is valid
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool InfoPackageIsValid(InfoPackage content)
        {
            if (content == null)
                return false;
            if (string.IsNullOrEmpty(content.ClientName) ||
                string.IsNullOrEmpty(content.ClientSurname) ||
                string.IsNullOrEmpty(content.MacAddress) ||
                string.IsNullOrEmpty(content.MachineName))
            { return false; }

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets called when there is a client to be accepted
        /// </summary>
        /// <param name="ar">Parameters</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            if (!IsRunning)
                return;

            Socket clientSocket;
            try
            {
                // Try to get the newly connected socket
                clientSocket = mServerSocket.EndAccept(ar);
            }
            catch
            {
                // No need to handle any errors for now 
                return;
            }

            // Accept this client and start reciving, but until the client doesn't not provide an info package, dont call the higher appliaction level
            mClients.Add(clientSocket, null);

            // Begin reciving on this socket
            clientSocket.BeginReceive(mReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);

            // Continue accepting new clients
            mServerSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!IsRunning)
                return;

            var senderSocket = (Socket)ar.AsyncState;
            int recivedCount;

            try
            {
                recivedCount = senderSocket.EndReceive(ar);
            }
            catch (SocketException ex)
            {
                // Client is forcefully disconnected
                senderSocket.Shutdown(SocketShutdown.Both);
                senderSocket.Close();

                // Let them know the client has disconnected
                OnClientDisconnected.Invoke((mClients[senderSocket]));

                // NOTE: remove client after calling the callback method above
                mClients.Remove(senderSocket);

                return;
            }
            catch
            {
                return;
            }

            // Get the recived data
            var recBuf = new byte[recivedCount];
            Array.Copy(mReciverBuffer, recBuf, recivedCount);

            if (DataPackageDescriptor.TryConvertToObj(recBuf, out var PackageReceived))
            {
                // Everything exepct from info packet and disconnect request packet is going to the higher level layer of the appliaction
                if (PackageReceived.PackageType == PackageType.Info)
                {
                    var content = (PackageReceived.Content as InfoPackage);

                    // If content is valid...
                    if (InfoPackageIsValid(content))
                    {
                        var clientId = string.Empty;

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
                            mClients[senderSocket] = model;

                            // Tell listeners that we got a new client
                            OnClientConnected.Invoke((model));
                        }
                        // Otherwise, update client model
                        else
                        {
                            // Update client model
                            var original = UpdateClientModel(senderSocket, content);

                            // Call the subscribed methods
                            OnClientDataUpdated.Invoke(original, mClients[senderSocket]);
                        }
                    }
                }
                else if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    // Close the socket
                    senderSocket.Shutdown(SocketShutdown.Both);
                    senderSocket.Close();

                    // Let them know the client has disconnected
                    OnClientDisconnected(mClients[senderSocket]);

                    // NOTE: remove client after calling the event method above
                    mClients.Remove(senderSocket);

                    // Return here to prevent running callback and starting reciving from not existing socket
                    return;
                }
                else
                    // Call the subscribed method only if the package description was successful
                    OnDataRecived(mClients[senderSocket], PackageReceived);
            }

            // Continue reciving
            senderSocket.BeginReceive(mReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, senderSocket);

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the server is currently running
        /// </summary>
        public bool IsRunning => mIsRunning;

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
        public IPAddress IPAddress
        {
            get => mIPAddress;
            set
            {
                if (!IsRunning)
                    mIPAddress = value;
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
        public int ConnectedClientCount => mClients.Count;

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
        public event Action<ClientModel, ClientModel> OnClientDataUpdated = (oldmodel, newModel) => { };

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            if (mIsRunning)
                return;
            try
            {
                mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mServerSocket.Bind(new IPEndPoint(IPAddress, Port));
                mServerSocket.Listen(0);
                mServerSocket.BeginAccept(AcceptCallback, null);
                mIsRunning = true;
            }
            catch
            {
                mIsRunning = false;
                // TODO: error handling
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            if (!mIsRunning)
                return;

            // Create a package that contains disconnect info 
            // Sent to the clients
            var data = new DataPackage(PackageType.DisconnectRequest, null);

            // Close all connections
            foreach (var item in mClients.Keys.ToList())
            {
                item.SendPackage(data);
                item.Shutdown(SocketShutdown.Both);
                item.Close();
                mClients.Remove(item);
            }

            mServerSocket.Close();

            mIsRunning = false;
        }

        /// <summary>
        /// Sends data to the client 
        /// </summary>
        /// <param name="target">Target client</param>
        /// <param name="data">Data to be sent</param>
        public void SendData(ClientModel target, DataPackage data)
        {
            var targetSocket = mClients.FirstOrDefault(x => x.Value == target).Key;

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
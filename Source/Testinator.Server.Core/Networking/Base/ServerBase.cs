using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Foundation of any server
    /// </summary>
    public abstract class ServerBase : IServer
    {
        #region Protected Memebers

        /// <summary>
        /// Matches all connected sockets with their client models
        /// </summary>
        protected readonly Dictionary<Socket, ClientModel> mClients = new Dictionary<Socket, ClientModel>();

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

        /// <summary>
        /// The socked this server is operating on
        /// </summary>
        private Socket mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        #endregion

        #region Private Methods

        /// <summary>
        /// Sends the data to the specified socket
        /// </summary>
        /// <param name="Target">The target socket</param>
        /// <param name="Data">The data to be sent</param>
        private void Send(Socket Target, DataPackage Data)
        {
            if (Target == null)
            {
                IoCServer.Logger.Log("Could not send the data. Target socket was null");
                throw new ServerException(LocalizationResource.CouldNotSendData, "Target socket was null");
            }

            if (Data == null)
            {
                IoCServer.Logger.Log("Could not send the data as it was null");
                throw new ServerException(LocalizationResource.CouldNotSendData, "Data package was null");
            }

            if (!DataPackageDescriptor.TryConvertToBin(out var senderBuffor, Data))
            {
                IoCServer.Logger.Log("Could not send the data. Binary conversion failed");
                throw new ServerException(LocalizationResource.CouldNotSendData, "Binary conversion failed");
            }

            IoCServer.Logger.Log("Sending data...");

            try
            {
                Target.Send(senderBuffor);
            }
            catch (Exception ex)
            {
                IoCServer.Logger.Log($"Failed to send the data. Error: {ex.Message}");
                throw new ServerException("Failed to send the data", ex);
            }

            IoCServer.Logger.Log("Data sent successfully");
        }

        /// <summary>
        /// Checks if the given socket has a <see cref="NetworkClientModel"/> associated with it
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
            var originalModel = mClients[Client];

            mClients[Client].Name = NewModel.ClientName;
            mClients[Client].LastName = NewModel.ClientSurname;
            mClients[Client].MachineName = NewModel.MachineName;

            return originalModel;
        }
        
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

        #region Public Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            IoCServer.Logger.Log("Server starting...");
            try
            {
                // Initialize the socket
                mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mServerSocket.Bind(new IPEndPoint(IP, Port));
                mServerSocket.Listen(0);
                
                // Begin accepting connections
                mServerSocket.BeginAccept(AcceptCallback, null);

                IsRunning = true;
            }
            catch (Exception ex)
            {
                IsRunning = false;

                IoCServer.Logger.Log($"Failed to start the server. Error: {ex.Message}");

                throw new ServerException(LocalizationResource.CouldNotStartServer, ex);
            }

            IoCServer.Logger.Log("Server has been started correctly");

            // If server has been started crrectly raise the event
            if (IsRunning == true)
                OnStartup();
        }

        /// <summary>
        /// Shuts the server down and disconnects all the clients
        /// </summary>
        public void ShutDown()
        {
            if (!IsRunning)
                return;

            IoCServer.Logger.Log("Attempting to shut the server down...");

            // Create a package that contains disconnect request 
            var DisconnectRequestPackage = new DataPackage(PackageType.DisconnectRequest, null);

            IoCServer.Logger.Log("Sending disconnection requests and closing sockets..");

            try
            {
                // Send the request and close the socket
                foreach (var socket in mClients.Keys.ToList())
                {
                    Send(socket, DisconnectRequestPackage);
                    
                    // Call the even method before closing socket
                    ClientDisconnected(mClients[socket]);

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    mClients.Remove(socket);
                }
            }
            catch (Exception ex)
            {
                IoCServer.Logger.Log($"Server shutdown error: {ex.Message}");
                
                // Clear the clients 
                mClients.Clear();
            }

            IoCServer.Logger.Log("All connected sockets closed\nClosing server socket...");

            // Also close the server socket to prevent from accepting new clients
            mServerSocket.Close();

            IoCServer.Logger.Log("Server socket closed");

            // Save status
            IsRunning = false;

            // Tell the listeners
            OnShutDown();
        }

        /// <summary>
        /// Sends data to the specified client
        /// </summary>
        /// <param name="TargetClient">The target to receive data</param>
        /// <param name="Data">The data to be sent</param>
        public void Send(ClientModel TargetClient, DataPackage Data)
        {
            // Get the target socket
            var targetSocket = mClients.FirstOrDefault(x => x.Value == TargetClient).Key;

            // Attempt to send data
            try
            {
                Send(targetSocket, Data);
            }
            catch (ServerException ex)
            {
                // Notify about the failure
                IoCServer.Logger.Log(ex.Message + ex.Reason);
            }
        }

        #endregion

        #region Socket Logic

        /// <summary>
        /// Gets called when there is a new socket to be accepted
        /// </summary>
        private void AcceptCallback(IAsyncResult ar)
        {
            // If the server is not running discard this connection
            // Should hardly ever get here, but just in case...
            if (!IsRunning)
                return;

            IoCServer.Logger.Log("Server: New connection. Trying to get the socket...");

            // Prapare socket for the new client
            Socket clientSocket;
            try
            {
                // Try to get the new client's socket
                clientSocket = mServerSocket.EndAccept(ar);
            }
            catch
            {
                IoCServer.Logger.Log("Failed to extract the socket");
                return;
            }

            // Add new client to the list, but don't raise any events until this client provides info package
            mClients.Add(clientSocket, null);

            // Begin reciving on this socket
            clientSocket.BeginReceive(mReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);

            IoCServer.Logger.Log("Socket connected succesfully.");

            // Continue accepting new clients
            mServerSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            // If the server is not running discard this data
            // Should hardly ever get here, but just in case...
            if (!IsRunning)
                return;

            // The sender
            Socket senderSocket = null;

            // Number of bytes received
            int recivedCount;

            try
            {  
                // Get the sender
                senderSocket = (Socket)ar.AsyncState;

                // Get the number of bytes received
                recivedCount = senderSocket.EndReceive(ar);
            }
            // TODO: look at this
            catch (SocketException ex)
            {
                // Client is forcefully disconnected
                senderSocket.Shutdown(SocketShutdown.Both);
                senderSocket.Close();

                // Let them know the client has disconnected
                ClientDisconnected(mClients[senderSocket]);

                // NOTE: remove client after calling the callback method above
                mClients.Remove(senderSocket);

                return;
            }
            catch
            {
                return;
            }

            // Prepare buffer for received data
            var recBuf = new byte[recivedCount];

            // Get the received data
            Array.Copy(mReciverBuffer, recBuf, recivedCount);

            // Try to get the package
            if (DataPackageDescriptor.TryConvertToObj<DataPackage>(recBuf, out var ReceivedPackage))
            {
                // Everything except from info packet and disconnect request packet is going to the higher level of the appliaction

                // Info package handling
                if (ReceivedPackage.PackageType == PackageType.Info)
                {
                    InfoPackageHandler(senderSocket, ReceivedPackage);
                }

                // Disconnection Request handling
                else if (ReceivedPackage.PackageType == PackageType.DisconnectRequest)
                {
                    // Close the socket
                    senderSocket.Shutdown(SocketShutdown.Both);
                    senderSocket.Close();

                    // Let them know the client has disconnected
                    ClientDisconnected(mClients[senderSocket]);

                    // NOTE: remove client after calling the event method above
                    mClients.Remove(senderSocket);

                    // Return here to prevent from reciving from not existing socket
                    return;
                }
                else
                    // Call the subscribed method only if the package description was successful
                    DataReceived(mClients[senderSocket], ReceivedPackage);
            }
            else
                IoCServer.Logger.Log("Skipping data package. Binary conversion failed.");

            // Continue receiving
            try
            {
                senderSocket.BeginReceive(mReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, senderSocket);
            }
            // If the socket has been meanwhile disposed(removed from connected users) or is it has already been closed with a previous call, dont do anything
            catch(Exception ex)
            {
                // No need to do anything in fact
                if (ex is ObjectDisposedException || ex is SocketException)
                    return;
            }
        }

        private void InfoPackageHandler(Socket SenderSocket, DataPackage ReceivedPackage)
        {
            var content = (ReceivedPackage.Content as InfoPackage);

            // If content is valid...
            if (content.Validate())
            {
                var clientId = string.Empty;

                // If it is a new user...
                if (!ClientModelExists(SenderSocket))
                {
                    // Construct new client model 
                    var newClientModel = new ClientModel()
                    {
                        MachineName = content.MachineName,
                        Name = content.ClientName,
                        LastName = content.ClientSurname,
                        IP = SenderSocket.GetIp(),
                    };

                    // Store client model
                    mClients[SenderSocket] = newClientModel;

                    // Tell listeners that we got a new client
                    ClientConnected(newClientModel);
                }
                // Otherwise, update client model
                else
                {
                    // Update client model
                    var original = UpdateClientModel(SenderSocket, content);

                    // Call the subscribed methods
                    ClientModelUpdated(original, mClients[SenderSocket]);
                }
            }
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Called on server startup
        /// </summary>
        protected virtual void OnStartup() { }

        /// <summary>
        /// Called as soon as the server is shuted down
        /// </summary>
        protected virtual void OnShutDown() { }

        /// <summary>
        /// Called when there is a new client connects
        /// </summary>
        /// <param name="NewClient">The client that has been connected to the server</param>
        protected abstract void ClientConnected(ClientModel NewClient);

        /// <summary>
        /// Called when a client disconnects
        /// </summary>
        /// <param name="DisconnectedClient">The client that has disconnected</param>
        protected abstract void ClientDisconnected(ClientModel DisconnectedClient);

        /// <summary>
        /// Called when a client model gets updated
        /// </summary>
        /// <param name="OldClientModel">The old model of this client</param>
        /// <param name="NewClientModel">The new model of this client</param>
        protected abstract void ClientModelUpdated(ClientModel OldClientModel, ClientModel NewClientModel);

        /// <summary>
        /// Called when there is data received from a client
        /// </summary>
        /// <param name="SenderClient">Client that sent the data</param>
        /// <param name="DataReceived">Received data</param>
        protected abstract void DataReceived(ClientModel SenderClient, DataPackage DataReceived);

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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Testinator.Core;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Base server 
    /// </summary>
    public class ServerBase
    {
        #region Protected Members

        /// <summary>
        /// The <see cref="Socket"/> for server
        /// </summary>
        protected Socket serverSocket;

        /// <summary>
        /// Conatins all connected clients
        /// </summary>
        protected readonly Dictionary<Socket, Client> Clients = new Dictionary<Socket, Client>();

        /// <summary>
        /// Buffer for received data
        /// </summary>
        protected byte[] ReceiverBuffer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the server is currently running
        /// </summary>
        public bool IsRunning { get; set; } = false;

        /// <summary>
        /// Default buffer size 
        /// </summary>
        public int BufferSize { get; set; } = 32768;

        /// <summary>
        /// Default port the server listens for
        /// </summary>
        public int Port { get; set; } = 3333;

        /// <summary>
        /// Default server IP
        /// </summary>
        public IPAddress IPAddress { get; set; } = IPAddress.Any;

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
        public delegate void DataReceivedDelegate(Client sender, DataPackage data);

        /// <summary>
        /// Fired when a new client is connected
        /// </summary>
        public delegate void ClientConnectedDelegate(Client sender);

        /// <summary>
        /// Fired when a client disconnects
        /// </summary>
        public delegate void ClientDisconnectedDelegate(Client sender);

        /// <summary>
        /// Method to be called any data is received from a client
        /// </summary>
        public DataReceivedDelegate ReceiverCallback { get; set; }

        /// <summary>
        /// Method to be called when a new client is connected
        /// </summary>
        public ClientConnectedDelegate ClientConnectedCallback { get; set; }

        /// <summary>
        /// Method to be called when a client is disconnected
        /// </summary>
        public ClientDisconnectedDelegate ClientDisconnectedCallback { get; set; }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Sets server Ip address
        /// </summary>
        /// <param name="Ip"></param>
        public void SetIP(IPAddress Ip)
        {
            if (IsRunning)
                return;

            IPAddress = Ip;
        }

        /// <summary>
        /// Sets server Ip address
        /// </summary>
        /// <param name="Ip"></param>
        public void SetIP(string Ip)
        {
            if (IsRunning)
                return;

            if (IPAddress.TryParse(Ip, out IPAddress address))
                IPAddress = address;
        }

        /// <summary>
        /// Sets server port
        /// </summary>
        /// <param name="port"></param>
        public void SetPort(int port)
        {
            if (IsRunning)
                return;

            Port = port;
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress, Port));
                serverSocket.Listen(25);
                serverSocket.BeginAccept(AcceptCallback, null);
                ReceiverBuffer = new byte[BufferSize];
                IsRunning = true;
            }
            catch
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            if (!IsRunning)
                return;

            // Close all connections
            foreach (KeyValuePair<Socket, Client> item in Clients)
            {
                item.Key.Shutdown(SocketShutdown.Both);
                item.Key.Close();
                Clients.Remove(item.Key);
            }

            serverSocket.Close();
            IsRunning = false;
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
                clientSocket = serverSocket.EndAccept(ar);
            }
            catch
            {
                return;
            }

            string clientsIp = ((IPEndPoint)(clientSocket.RemoteEndPoint)).Address.ToString();
            Clients.Add(clientSocket, new Client(ClientIdProvider.GetId(), clientsIp));
            clientSocket.BeginReceive(ReceiverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            serverSocket.BeginAccept(AcceptCallback, null);

            // Let them know client has connected
            ClientConnectedCallback(Clients[clientSocket]);

        }

        /// <summary>
        /// Called when when data is received 
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            int received;
            
            try
            {
                received = clientSocket.EndReceive(ar);
            }
            catch(SocketException)
            {
                clientSocket.Close();
                Clients.Remove(clientSocket);

                // Let them know the client has disconnected
                ClientDisconnectedCallback(Clients[clientSocket]);

                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(ReceiverBuffer, recBuf, received);
            
            if(DataPackageDescriptor.TryDescript(recBuf, out DataPackage PackageReceived))
            {
                
                // Everything exepct from info packet is going to the higher level layer of appliaction
                if (PackageReceived.PackageType == PackageType.Info)
                {
                    var content = (PackageReceived.Content as InfoPackage);
                    if (content == null)
                        return;

                    Clients[clientSocket].ID = content.ID;
                    Clients[clientSocket].MachineName = content.MachineName;
                }
                else
                {
                    // Call the subscribed method only if the package description was successful
                    ReceiverCallback(Clients[clientSocket], PackageReceived);
                }
            }

            clientSocket.BeginReceive(ReceiverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);

        }

        #endregion

        #region Construcotrs

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerBase() { }

        /// <summary>
        /// Constructs server with the given IP
        /// </summary>
        /// <param name="Ip">Server IP</param>
        public ServerBase(string Ip)
        {
            SetIP(Ip);
        }

        /// <summary>
        /// Constructs server with the given IP and port
        /// </summary>
        /// <param name="Ip">Server IP</param>
        /// <param name="port">Srver Port</param>
        public ServerBase(string Ip, int port)
        {
            SetIP(Ip);
            Port = port;
        }

        /// <summary>
        /// Constructs server with the given port
        /// </summary>
        /// <param name="port">Server port</param>
        public ServerBase(int port)
        {
            Port = port;
        }

        #endregion
    }
}
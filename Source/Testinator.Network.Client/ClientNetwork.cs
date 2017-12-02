using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Testinator.Core;

namespace Testinator.Network.Client
{
    /// <summary>
    /// Provides network support for connecting to the server
    /// </summary>
    public class ClientNetwork
    {

        #region Protected Members

        /// <summary>
        /// Used to connect to the sever
        /// </summary>
        protected Socket clientSocket;

        /// <summary>
        /// Buffer for received data
        /// </summary>
        protected byte[] ReceiverBuffer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the client is trying to connect to the server
        /// </summary>
        public bool Connecting { get; set; }

        /// <summary>
        /// Indicates if the client is connected to the server
        /// </summary>
        public bool IsConnected => clientSocket.Connected;

        /// <summary>
        /// Default buffer size 
        /// </summary>
        public int BufferSize { get; set; } = 32768;

        /// <summary>
        /// Default server IP address
        /// </summary>
        public IPAddress IPAddress { get; set; } = IPAddress.Parse("127.0.0.1");

        /// <summary>
        /// Default port the clients try to connect server at
        /// </summary>
        public int Port { get; set; } = 3333;

        /// <summary>
        /// The connection attempt number
        /// </summary>
        public int Attempt { get; set; }

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

        #region Public Methods

        /// <summary>
        /// Sets server Ip address
        /// </summary>
        /// <param name="Ip"></param>
        public void SetIP(IPAddress Ip)
        {
            if (IsConnected && Connecting)
                return;

            IPAddress = Ip;
        }

        /// <summary>
        /// Sets server Ip address
        /// </summary>
        /// <param name="Ip"></param>
        public void SetIP(string Ip)
        {
            if (IsConnected && Connecting)
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
            if (IsConnected && Connecting)
                return;

            Port = port;
        }

        /// <summary>
        /// Starts the connecting sequence
        /// </summary>
        public void StartConnecting()
        {
            if (IsConnected && Connecting)
                return;

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ReceiverBuffer = new byte[BufferSize];

            Thread connectingThread = new Thread(new ThreadStart(TryConnecting))
            {
                IsBackground = true,
                Name = "ConnectingThread"
            };

            Connecting = true;
            connectingThread.Start();
        }

        /// <summary>
        /// Either disconnects from the server or stops connecting to the server
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            Connecting = false;
        }

        public void SendData(DataPackage data)
        {
            if (DataPackageDescriptor.TryConvertToBin(out byte[] sendBuffor, data))
            {
                clientSocket.Send(sendBuffor, 0, sendBuffor.Length, SocketFlags.None);
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Tries to connected to the server
        /// </summary>
        private void TryConnecting()
        {
            while (!IsConnected && Connecting)
            {
                try
                {
                    Attempt++;
                    clientSocket.Connect(IPAddress, Port);
                }
                catch(SocketException)
                {  }
            }
            if(IsConnected)
            {
                clientSocket.BeginReceive(ReceiverBuffer, 0, BufferSize, SocketFlags.None, ReciveCallback, clientSocket);
                
                // Let them know we have connected to the server
                ConnectedCallback();
            }
            Connecting = false;
        }

        /// <summary>
        /// Called when data is recived
        /// </summary>
        /// <param name="ar"></param>
        private void ReciveCallback(IAsyncResult ar)
        {
            int recived;

            try
            {
                recived = clientSocket.EndReceive(ar);
            }
            catch
            {
                Disconnect();
                
                // Let them know we have been disconnected
                DisconnectedCallback();

                return;
            }

            byte[] recBuf = new byte[recived];
            Array.Copy(ReceiverBuffer, recBuf, recived);

            if(DataPackageDescriptor.TryConvertToObj(recBuf, out DataPackage PackageReceived))
            {
                // Call the subscribed method only if the package description was successful
                DataRecivedCallback(PackageReceived);
            }

            clientSocket.BeginReceive(ReceiverBuffer, 0, BufferSize, SocketFlags.None, ReciveCallback, clientSocket);
        }

        #endregion

        #region Construcotrs

        /// <summary>
        /// Default construcotr
        /// </summary>
        public ClientNetwork()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion
    }
}

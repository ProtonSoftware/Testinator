using System;
using System.Net;
using System.Net.Sockets;
using Testinator.Core;

namespace Testinator.Network.Client
{
    /// <summary>
    /// Provides network support for connecting to the server
    /// </summary>
    public class ClientNetwork : ClientBase
    {
        #region Private Members

        /// <summary>
        /// Called when data is recived
        /// </summary>
        /// <param name="ar"></param>
        protected override void ReciveCallback(IAsyncResult ar)
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

            if (DataPackageDescriptor.TryConvertToObj(recBuf, out DataPackage PackageReceived))
            {
                if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    
                    //DisconnectedCallback();
                    // TODO: uncomment this when callback system is changed to event system
                }
                else
                {
                    // Call the subscribed method only if the package description was successful
                    DataRecivedCallback(PackageReceived);
                }

            }
            if (IsConnected)
                clientSocket.BeginReceive(ReceiverBuffer, 0, BufferSize, SocketFlags.None, ReciveCallback, clientSocket);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the object with the given data
        /// </summary>
        /// <param name="ip">Ip address of the host</param>
        /// <param name="port">The port of the host</param>
        public void Initialize(string ip, string port)
        {
            IPAddress = IPAddress.Parse(ip);
            Port = Int32.Parse(port);
            // TODO: potential error handling
            // Should not be any problems here however. 
        }

        #endregion

        #region Construcotrs

        /// <summary>
        /// Sets server up with default settings [recommended]
        /// </summary>
        public ClientNetwork() { }

        /// <summary>
        /// Sets client up with the given ip, port and default buffer size if not specified
        /// </summary>
        /// <param name="I">Client ip</param>
        /// <param name="port">Client port</param>
        /// <param name="bufferSize">Size of the reciver buffer</param>
        public ClientNetwork(string Ip, int port, int bufferSize = 32768)
        {
            IPAddress = IPAddress.Parse(Ip);
            Port = port;
            BufferSize = bufferSize;
        }

        #endregion
    }
}
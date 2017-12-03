using System;
using System.Net;
using System.Net.Sockets;
using Testinator.Core;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Base server 
    /// </summary>
    public class Server : ServerBase
    {
        #region Private Methods

        /// <summary>
        /// Gets called when there is a client to be accepted
        /// </summary>
        /// <param name="ar">Parameters</param>
        protected override void AcceptCallback(IAsyncResult ar)
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
            Clients.Add(clientSocket, new ClientModel(ClientIdProvider.GetId(), clientsIp));
            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            serverSocket.BeginAccept(AcceptCallback, null);

            // Let them know client has connected
            ClientConnectedCallback(Clients[clientSocket]);
        }

        /// <summary>
        /// Called when data is recived 
        /// </summary>
        /// <param name="ar"></param>
        protected override void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            int recived;
            
            try
            {
                recived = clientSocket.EndReceive(ar);
            }
            catch(SocketException)
            {
                // Client is forcefully disconnected
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                // Let them know the client has disconnected
                ClientDisconnectedCallback(Clients[clientSocket]);

                // NOTE: remove client after calling the event method above
                Clients.Remove(clientSocket);

                return;
            }

            byte[] recBuf = new byte[recived];
            Array.Copy(ReciverBuffer, recBuf, recived);
            
            if(DataPackageDescriptor.TryConvertToObj(recBuf, out DataPackage PackageReceived))
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

                if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                    // Let them know the client has disconnected
                    ClientDisconnectedCallback(Clients[clientSocket]);

                    // NOTE: remove client after calling the event method above
                    Clients.Remove(clientSocket);

                    // Prevents from running callback to the higher level of the app
                    return;
                }

                // Call the subscribed method only if the package description was successful
                DataRecivedCallback(Clients[clientSocket], PackageReceived);
            }

            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);

        }

        #endregion

        #region Construcotrs

        /// <summary>
        /// Sets server up with default settings
        /// </summary>
        public Server() { }

        /// <summary>
        /// Sets server up with the given ip, port and default buffer size if not specified
        /// </summary>
        /// <param name="I">Server ip</param>
        /// <param name="port">Server port</param>
        /// <param name="bufferSize">Size of the reciver buffer</param>
        public Server(string Ip, int port, int bufferSize = 32768)
        {
            this.IPAddress = IPAddress.Parse(Ip);
            this.Port = port;
            this.BufferSize = bufferSize;

            ReciverBuffer = new byte[BufferSize];
        }

        #endregion
    }
}
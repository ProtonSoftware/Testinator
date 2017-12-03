using System;
using System.Collections.Generic;
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
        #region Private Members

        /// <summary>
        /// Keeps track of id's given to each user
        /// </summary>
        private Dictionary<string, string> MacId = new Dictionary<string, string>();

        #endregion

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

            // Accept this client and start reciving, but until the client doesn't not provide an info package, dont call the higher appliaction level
            Clients.Add(clientSocket, null);
            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            serverSocket.BeginAccept(AcceptCallback, null);
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
            catch(Exception)
            {
                return;
            }

            byte[] recBuf = new byte[recived];
            Array.Copy(ReciverBuffer, recBuf, recived);
            
            if(DataPackageDescriptor.TryConvertToObj(recBuf, out DataPackage PackageReceived))
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

                    // New client
                    if (Clients[clientSocket] == null)
                    {
                        // Tell listeners that we got a new client
                        ClientConnectedCallback(model);
                    }
                    
                    // Update client model
                    Clients[clientSocket] = model;
                }
                else if (PackageReceived.PackageType == PackageType.DisconnectRequest)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                    // Let them know the client has disconnected
                    ClientDisconnectedCallback(Clients[clientSocket]);

                    // NOTE: remove client after calling the event method above
                    Clients.Remove(clientSocket);

                    // Prevents running callback and starting reciving from not existing socket
                    return;
                }
                else
                    // Call the subscribed method only if the package description was successful
                    DataRecivedCallback(Clients[clientSocket], PackageReceived);
            }

            clientSocket.BeginReceive(ReciverBuffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            
        }

        #endregion

        #region Construcotrs

        /// <summary>
        /// Sets server up with default settings [recommended]
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
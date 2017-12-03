using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Testinator.Core;
using Testinator.Network.Server;

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
        /// Sets server up with default settings
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
            this.IPAddress = IPAddress.Parse(Ip);
            this.Port = port;
            this.BufferSize = bufferSize;
        }

        #endregion
    }
}

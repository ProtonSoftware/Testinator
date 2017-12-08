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
        }

        #endregion
    }
}
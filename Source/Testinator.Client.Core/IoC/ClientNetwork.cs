using System;
using System.Net;
using System.Net.Sockets;
using Testinator.Core;
using Testinator.Network.Client;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Provides network support for connecting to the server
    /// </summary>
    public class ClientNetwork : ClientBase
    {
        #region Private Members

        /// <summary>
        /// Indicates if the client has received test
        /// </summary>
        private bool _IsTestReceived = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the client has received test
        /// </summary>
        public bool IsTestReceived => _IsTestReceived;

        #endregion

        #region Construcotrs

        /// <summary>
        /// Sets server up with default settings [recommended]
        /// </summary>
        public ClientNetwork()
        {
            OnConnected += ClientNetwork_OnConnected;
        }

        #endregion

        #region Private Members
        
        /// <summary>
        /// Fired when client manages to connect to the sever
        /// </summary>
        private void ClientNetwork_OnConnected()
        {
            IoCClient.Application.PropertyChangedInvoke(nameof(IoCClient.Application.IsConnected));
        }

        #endregion
    }
}
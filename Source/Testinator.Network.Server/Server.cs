
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Testinator.Core;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Base server 
    /// </summary>
    public class ServerNetwork : ServerBase
    {
        #region Public Members

        /// <summary>
        /// A list of connected clients
        /// </summary>
        public ObservableCollection<ClientModel> Clients { get; set; } = new ObservableCollection<ClientModel>();

        #endregion

        #region Public Events


        #endregion

        #region Private Events

        /// <summary>
        /// Fired when a new user connectes to the server
        /// </summary>
        /// <param name="client">A model for this hte client</param>
        private void ServerClientConnected(ClientModel client)
        {
            // Jump on the dispatcher thread
            var uiContext = SynchronizationContext.Current;
            uiContext.Send(x => Clients.Add(client), null);
        }

        /// <summary>
        /// Fired when a client disconnected from the server
        /// </summary>
        /// <param name="client">The client that has disconnected</param>
        private void ServerClientDisconnected(ClientModel client)
        {
            // Jump on the dispatcher thread
            var uiContext = SynchronizationContext.Current;
            uiContext.Send(x => Clients.Remove(client), null);
        }

        #endregion

        #region Construcotrs

        /// <summary>
        /// Sets server up with default settings [recommended]
        /// </summary>
        public ServerNetwork()
        {
            // Subscribe to the server events
            OnClientConnected += ServerClientConnected;
            OnClientDisconnected += ServerClientDisconnected;
        }

        #endregion
    }
}
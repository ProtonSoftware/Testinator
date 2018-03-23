using System;
using System.Collections.ObjectModel;
using System.Threading;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Handles networking on server side using TCP data transfer
    /// </summary>
    public class ServerNetwork : ServerBase
    {
        #region Public Methods

        /// <summary>
        /// The list of all clients currently connected to the server 
        /// NOTE: Can be used as a source of binding
        /// </summary>
        public ObservableCollection<ClientModel> ConnectedClients { get; set; } = new ObservableCollection<ClientModel>();

        #endregion

        #region Public Events

        /// <summary>
        /// The event that is fired when any data has been recived from a client
        /// </summary>
        public event Action<ClientModel, DataPackage> OnDataReceived = (sender, data) => { };

        /// <summary>
        /// The event that is fired when a new client has connected
        /// </summary>
        public event Action<ClientModel> OnClientConnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client has disconnected
        /// </summary>
        public event Action<ClientModel> OnClientDisconnected = (sender) => { };

        /// <summary>
        /// The event that is fired when a client's data had beed updated
        /// </summary>
        public event Action<ClientModel, ClientModel> OnClientDataUpdated = (oldmodel, newModel) => { };

        #endregion

        #region Private Events

        protected override void ClientConnected(ClientModel NewClient)
        {
            // Jump on the dispatcher thread
            var uiContext = SynchronizationContext.Current;
            uiContext.Send(x => ConnectedClients.Add(NewClient), null);
            

            OnClientConnected.Invoke(NewClient);
        }

        protected override void ClientDisconnected(ClientModel DisconnectedClient)
        {
            // Jump on the dispatcher thread
            var uiContext = SynchronizationContext.Current;
            uiContext.Send( x => ConnectedClients.Remove(DisconnectedClient), null);

            OnClientDisconnected(DisconnectedClient);
        }

        protected override void ClientModelUpdated(ClientModel OldClientModel, ClientModel NewClientModel)
        {
            OnClientDataUpdated(OldClientModel, NewClientModel);
        }

        protected override void DataReceived(ClientModel SenderClient, DataPackage DataReceived)
        {
            OnDataReceived(SenderClient, DataReceived);
        }

        #endregion
    }
}
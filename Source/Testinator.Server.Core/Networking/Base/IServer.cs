using System;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The base server interface
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Starts the server 
        /// </summary>
        void Start();

        /// <summary>
        /// Shuts the server and disconnect all the clients
        /// </summary>
        void ShutDown();

        /// <summary>
        /// Sends a data package to the client
        /// </summary>
        /// <param name="TargetClient">Client to be sent the data</param>
        /// <param name="Data">The data to be sent to the client</param>
        void Send(ClientModel TargetClient, DataPackage Data);

        /// <summary>
        /// Disconnets a single client
        /// </summary>
        /// <param name="ClientToDisconnect">Client to be disconnected</param>
        //void DisconnectClient(NetworkClientModel ClientToDisconnect);
    }
}

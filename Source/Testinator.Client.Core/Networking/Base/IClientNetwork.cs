using System;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Defines behaviour of a client in network
    /// </summary>
    public interface IClientNetwork
    {
        /// <summary>
        /// Connects to the remote server
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects from the remote server if connected
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sends data to the remote server
        /// </summary>
        /// <param name="Data"></param>
        void SendData(DataPackage Data);

        /// <summary>
        /// Fired when there is data received from the remote server
        /// </summary>
        event Action<DataPackage> DataReceived;
    }

}

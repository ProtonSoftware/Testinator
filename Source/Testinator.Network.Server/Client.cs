using System.Net;
using System.Net.Sockets;

namespace Testinator.Network.Server
{
    /// <summary>
    /// Defines the structure of the client connected to the sever
    /// </summary>
    public class ClientModel
    {
        #region Public Properties

        /// <summary>
        /// Clinet id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The client's machine name
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The client's ip address
        /// </summary>
        public string IpAddress { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructs a client from the given socket
        /// </summary>
        public ClientModel(string id, string ip)
        {
            ID = id;
            IpAddress = ip;
        }

        #endregion
    }
}

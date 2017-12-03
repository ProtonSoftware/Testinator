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
        /// Mac address of the client's ethernet card.
        /// Helps to distinguish clients 
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// The client's machine name
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The client's ip address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Client's name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Client's surname
        /// </summary>
        public string ClientSurname { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        public ClientModel() { }

        #endregion
    }
}

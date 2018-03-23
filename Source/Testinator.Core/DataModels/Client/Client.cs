using System;
using System.Net;

namespace Testinator.Core
{
    /// <summary>
    /// Base class for any client to derive from
    /// </summary>
    [Serializable]
    public class Client : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Client's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Client's last name
        /// </summary>
        public string LastName { get; set; }
   
        /// <summary>
        /// Client's machine name
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Client's IP address
        /// </summary>
        public IPAddress IP { get; set; }

        /// <summary>
        /// Client's ip address as string
        /// </summary>
        public string IPString => IP.ToString();

        #endregion
    }
}

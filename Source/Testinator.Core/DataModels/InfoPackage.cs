using System;
using Testinator.Core;

namespace Testinator.Network.Server
{
    [Serializable]
    public class InfoPackage : PackageContent
    {
        /// <summary>
        /// The sender's machine name
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Mac address of the client's ethernet card.
        /// Helps to distinguish clients 
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// Client's name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Client's surname
        /// </summary>
        public string ClientSurname { get; set; }

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="MachineName">Sedner's machine name</param>
        /// <param name="mac">PC mac address</param>
        /// <param name="name">Clients name</param>
        /// <param name="surname">Clients surname</param>
        public InfoPackage(string MachineName, string mac, string name, string surname)
        {
            ClientName = name;
            ClientSurname = surname;
            this.MachineName = MachineName;
            MacAddress = mac;
        }

        #endregion
    }
}

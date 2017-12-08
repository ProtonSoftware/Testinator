using System;

namespace Testinator.Core
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
        /// Constructs the info package
        /// </summary>
        /// <param name="MachineName">Sedner's machine name</param>
        /// <param name="mac">PC mac address</param>
        /// <param name="name">Clients name</param>
        /// <param name="surname">Clients surname</param>
        public InfoPackage(string name, string surname, string MachineName, string mac)
        {
            ClientName = name;
            ClientSurname = surname;
            this.MachineName = MachineName;
            MacAddress = mac;
        }

        /// <summary>
        /// Constructs the package from a model
        /// </summary>
        /// <param name="model">The model this package is based on</param>
        public InfoPackage(ClientModel model)
        {
            ClientName = model.ClientName;
            ClientSurname = model.ClientSurname;
        }
        #endregion
    }
}
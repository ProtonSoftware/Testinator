using System;
using Testinator.Core.Network;

namespace Testinator.Core
{
    /// <summary>
    /// Defines the structure of the client connected to the sever
    /// </summary>
    [Serializable]
    public class ClientModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Client's id
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
        public string ClientName { get; set; } = "";

        /// <summary>
        /// Client's surname
        /// </summary>
        public string ClientSurname { get; set; } = "";

        /// <summary>
        /// Indicates if the user can start the test, meaning: is not in the result page but in the waiting for test page
        /// </summary>
        public bool CanStartTest { get; set; } = true;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a <see cref="DataPackage"/> of type <see cref="PackageType.Info"/> initialized 
        /// with the data stored in this client model
        /// </summary>
        /// <returns>Info package</returns>
        public DataPackage GetPackage() => new DataPackage(PackageType.Info)
        {
            Content = new InfoPackage(this),
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientModel()
        {
            // Create defaults
            MacAddress = MacAddressHelpers.GetMac();
            MachineName = Environment.MachineName;
        }

        /// <summary>
        /// Constructor with the source
        /// </summary>
        public ClientModel(ClientModel source) : this()
        {
            // Add source informations
            ID = source.ID;
            MacAddress = source.MacAddress;
            MachineName = source.MachineName;
            IpAddress = source.IpAddress;
            ClientName = source.ClientName;
            ClientSurname = source.ClientSurname;
        }

        #endregion
    }
}
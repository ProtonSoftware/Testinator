using System;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Defines the structure of ourselves
    /// </summary>
    public class ClientModel : Testinator.Core.Client
    {

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
            MachineName = Environment.MachineName;
        }

        #endregion
    }
}
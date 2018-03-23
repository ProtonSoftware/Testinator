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
        }

        /// <summary>
        /// Constructs the package from a model
        /// </summary>
        /// <param name="model">The model this package is based on</param>
        public InfoPackage(Client model)
        {
            ClientName = model.Name;
            ClientSurname = model.LastName;
            MachineName = model.MachineName;
        }

        #endregion
    }

    public static class InfoPackageExtensionMethods
    {
        /// <summary>
        /// Validates the package content
        /// </summary>
        /// <param name="package"></param>
        /// <returns>True if package is valid, false if not</returns>
        public static bool Validate(this InfoPackage package)
        {
            if (package == null)
                return false;
            if (string.IsNullOrEmpty(package.ClientName) ||
                string.IsNullOrEmpty(package.ClientSurname) ||
                string.IsNullOrEmpty(package.MachineName))
            { return false; }

            return true;
        }
    }
}


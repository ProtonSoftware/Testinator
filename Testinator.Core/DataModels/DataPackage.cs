using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Testinator.Core
{
    /// <summary>
    /// Defines a package that is send between client and server
    /// </summary>
    [Serializable]
    public class DataPackage
    {

        #region Public Properties

        /// <summary>
        /// The type of this package
        /// </summary>
        public PackageType PackageType { get; set; }

        /// <summary>
        /// The content of this package
        /// </summary>
        public PackageContent Content { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataPackage(PackageType Type, PackageContent Content)
        {
            PackageType = Type;
            this.Content = Content;
        }

        #endregion
    }
}
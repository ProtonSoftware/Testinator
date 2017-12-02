using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Testinator.Core
{
    /// <summary>
    /// Defines a package that is send between client and server
    /// </summary>
    public class DataPackage
    {

        #region Public Properties

        /// <summary>
        /// The sender's id
        /// </summary>
        public string SenderId { get; set; }
        
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
        public DataPackage(string SenderId, PackageType Type, PackageContent Content)
        {
            this.SenderId = SenderId;
            PackageType = Type;
            this.Content = Content;
        }

        #endregion
        
        /// <summary>
        /// Gets the binary of this class
        /// Usefull for sending operation
        /// </summary>
        /// <returns></returns>
        public byte[] GetBinary()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                return ms.ToArray();
            }
        }
    }
}

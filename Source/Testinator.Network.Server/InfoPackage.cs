using Testinator.Core;

namespace Testinator.Network.Server
{
    public class InfoPackage : PackageContent
    {
        /// <summary>
        /// Sender's id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The sender's machine name
        /// </summary>
        public string MachineName { get; set; }

        #region Constructor
        
        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="id">Sender's id</param>
        /// <param name="MachineName">Sedner's machine name</param>
        public InfoPackage(string id, string MachineName)
        {
            ID = id;
            this.MachineName = MachineName;
        }

        #endregion
    }
}

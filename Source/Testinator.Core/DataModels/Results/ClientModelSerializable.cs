using System;

namespace Testinator.Core
{
    /// <summary>
    /// A clientmodel that can be serialized 
    /// Copy of <see cref="ClientModel"/>
    /// </summary>
    [Serializable]
    public class ClientModelSerializable
    {

        /// <summary>
        /// Client's name
        /// </summary>
        public string ClientName { get; set; } = "";

        /// <summary>
        /// Client's surname
        /// </summary>
        public string ClientSurname { get; set; } = "";

        /// <summary>
        /// The client's machine name
        /// </summary>
        public string MachineName { get; set; }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientModelSerializable(ClientModel model)
        {
            ClientName = model.ClientName;
            ClientSurname = model.ClientSurname;
            MachineName = model.MachineName;
        }

        #endregion
    }
}

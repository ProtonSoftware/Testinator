using System;
using Testinator.Core;

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

        /// <summary>
        /// Points scored by the user
        /// </summary>
        public int PointsScored { get; set; }

        /// <summary>
        /// The client mark
        /// </summary>
        public Marks Mark { get; set; }

        #region Constructor

        /// <summary>
        /// Constructos object based on another model
        /// </summary>
        public ClientModelSerializable(ClientModelExtended model)
        {
            ClientName = model.ClientName;
            ClientSurname = model.ClientSurname;
            MachineName = model.MachineName;
            PointsScored = model.PointsScored;
            Mark = model.Mark;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientModelSerializable()
        {

        }
        #endregion
    }
}

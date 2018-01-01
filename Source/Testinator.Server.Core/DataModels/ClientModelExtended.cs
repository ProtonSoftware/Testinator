using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Extended version of <see cref="ClientModel"/> class
    /// </summary>
    public class ClientModelExtended : ClientModel
    {
        #region Public Properties

        /// <summary>
        /// Number of questions this client have done so far
        /// </summary>
        public int CurrentQuestion { get; set; }

        /// <summary>
        /// The value for the progress bar, substract one from the CurrentQuestion to show the progress correctly
        /// </summary>
        public int ProgressBarValue => CurrentQuestion - 1;

        /// <summary>
        /// Indicates if there is any connection problems with this client
        /// </summary>
        public bool ConnectionProblem { get; set; }

        /// <summary>
        /// The answer given by this user
        /// </summary>
        public List<Answer> Answers { get; set; }

        /// <summary>
        /// Points scored by the user
        /// </summary>
        public int PointsScored { get; set; }

        /// <summary>
        /// The client mark
        /// </summary>
        public Marks Mark { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="baseClient">The object this model is beased on</param>
        public ClientModelExtended(ClientModel baseClient)
        {

#pragma warning disable IDE0003 // Remove qualification
            this.ID = baseClient.ID;
            this.MacAddress = baseClient.MacAddress;
            this.MachineName = baseClient.MachineName;
            this.IpAddress = baseClient.IpAddress;
            this.ClientName = baseClient.ClientName;
            this.ClientSurname = baseClient.ClientSurname;
#pragma warning restore IDE0003 // Remove qualification

        }

        #endregion
    }
}

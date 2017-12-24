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
        public int QuestionsDone { get; set; }

        /// <summary>
        /// Indicates if there is any connection problems with this client
        /// </summary>
        public bool ConnectionProblem { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="baseClient">The object this model is beased on</param>
        public ClientModelExtended(ClientModel baseClient)
        {
            this.ID = baseClient.ID;
            this.MacAddress = baseClient.MacAddress;
            this.MachineName = baseClient.MachineName;
            this.IpAddress = baseClient.IpAddress;
            this.ClientName = baseClient.ClientName;
            this.ClientSurname = baseClient.ClientSurname;

        }

        #endregion
    }
}

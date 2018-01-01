using System;
using Testinator.Core;
using Testinator.Network.Client;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : PageHostViewModel
    {
        #region Public Properties

        /// <summary>
        /// Handles network communication in the application
        /// </summary>
        public ClientNetwork Network { get; set; } = new ClientNetwork();

        /// <summary>
        /// Indicates if the client is currently connected to the server
        /// </summary>
        public bool IsConnected => Network.IsConnected;

        /// <summary>
        /// Indicates how much time is left 
        /// </summary>
        public TimeSpan TimeLeft => IoCClient.TestHost.TimeLeft;
        

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationViewModel()
        {

        }

        #endregion

    }
}

using System;
using Testinator.Core;

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

        #region Public Methods

        /// <summary>
        /// Returns to the login page if there is no connection to the sevrer
        /// or to the waitingForTestPage is still connected
        /// </summary>
        public void ReturnMainScreen()
        {
            if (Network.IsConnected)
            {
                IoCClient.UI.ChangePage(ApplicationPage.WaitingForTest);

                // Indicate we are ready for another test now
                IoCClient.Application.Network.SendData(new DataPackage(PackageType.ReadyForTest));
            }
            else
                IoCClient.UI.ChangePage(ApplicationPage.Login);
        }

        #endregion

    }
}

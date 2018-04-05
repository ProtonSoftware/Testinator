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
        /// Indicates how much time is left 
        /// </summary>
        public TimeSpan TimeLeft => IoCClient.TestHost.TimeLeft;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns to the login page if there is no connection to the sevrer
        /// or to the waitingForTestPage if it is still connected
        /// </summary>
        public void ReturnMainScreen()
        {
            if (Network.IsConnected)
            {
                IoCClient.UI.ChangePage(ApplicationPage.WaitingForTest);

                // This shouldn't be here
                // Indicate we are ready for another test now
                IoCClient.Application.Network.SendData(new DataPackage(PackageType.ReadyForTest));
            }
            else
                IoCClient.UI.ChangePage(ApplicationPage.Login);
        }

        #endregion
    }
}

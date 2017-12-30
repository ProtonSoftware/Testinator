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
        /// Indicates if the test is in progress
        /// </summary>
        public bool IsTestInProgress => IoCClient.TestHost.IsTestInProgress;

        /// <summary>
        /// Indicates if the client is currently connected to the server
        /// </summary>
        public bool IsConnected => Network.IsConnected;

        /// <summary>
        /// Indicates how much time is left 
        /// </summary>
        public TimeSpan TimeLeft => IoCClient.TestHost.TimeLeft;

        /// <summary>
        /// Indicates if the client has received test
        /// </summary>
        public bool IsTestReceived { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            // Bind to the connected event
            Network.OnConnected += NetworkConnected;
            Network.OnDisconnected += NetworkDisconnect;
            Network.OnDataReceived += NetworkDataReceived;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when data has been received from the server
        /// </summary>
        /// <param name="data"></param>
        private void NetworkDataReceived(DataPackage data)
        {
            switch (data.PackageType)
            {
                case PackageType.TestForm:
                    // Bind the newly received test
                    IoCClient.TestHost.BindTest(data.Content as Test);
                    break;

                case PackageType.BeginTest:
                    // Start the test
                    IoCClient.TestHost.StartTest();
                    break;
            }
        }

        /// <summary>
        /// Fired when connection gets stoped
        /// </summary>
        private void NetworkDisconnect()
        {
            // Log it
            IoCClient.Logger.Log("Network disconnected");

            // Simply go back to the initial login page
            IoCClient.UI.ChangePage(ApplicationPage.Login);
        }

        /// <summary>
        /// Fired when connection with the server has been established
        /// </summary>
        private void NetworkConnected()
        {
            // If we aren't on login page, don't bother doing anything
            if (CurrentPage != ApplicationPage.Login)
                return;

            // Log it
            IoCClient.Logger.Log("Network connected");

            // Change page to waiting for test page
            IoCClient.UI.ChangePage(ApplicationPage.WaitingForTest);

            // Send info package with that information
            Network.SendData(IoCClient.Client.GetPackage());
        }

        #endregion
    }
}

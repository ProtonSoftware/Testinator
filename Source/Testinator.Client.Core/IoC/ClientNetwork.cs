using Testinator.Core;
using Testinator.Network.Client;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Provides network support for connecting to the server
    /// </summary>
    public class ClientNetwork : ClientBase
    {
        #region Public Properties

        /// <summary>
        /// Indicates if attempting to reconnect is in progress
        /// </summary>
        public bool AttemptingToReconnect { get; set; }

        #endregion

        #region Constructor

        public ClientNetwork()
        {
            // Bind to the connected event
            OnConnected += NetworkConnected;
            OnDisconnected += NetworkDisconnect;
            OnDataReceived += NetworkDataReceived;
        }

        #endregion

        public new void Disconnect()
        {
            base.Disconnect();
            AttemptingToReconnect = false;
        }

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

            // Dont'try to reconnect if in the result page, because the test result has been already sent to the server
            if (IoCClient.TestHost.IsShowingResultPage)
                return;

            // If the test in progress
            if (IoCClient.TestHost.IsTestInProgress)
            {
                // Notify the test host about the disconnection
                IoCClient.TestHost.NetworkDisconnected();

                // Set attempting to reconnect
                AttemptingToReconnect = true;

                // Start connecting to the server
                StartConnecting();
            }

            // In any other case return to the login page
            else
                IoCClient.UI.ChangePage(ApplicationPage.Login);
        }

        /// <summary>
        /// Fired when connection with the server has been established
        /// </summary>
        private void NetworkConnected()
        {
            // Log it
            IoCClient.Logger.Log("Network connected");

            // Send info package with the information
            SendData(IoCClient.Client.GetPackage());
            IoCClient.Logger.Log("Sending client info...");

            // Reset AttemptingToReconnect flag
            AttemptingToReconnect = false;

            // If we're in login page change page to the waiting for test page
            if (IoCClient.Application.CurrentPage == ApplicationPage.Login)
            {
                IoCClient.UI.ChangePage(ApplicationPage.WaitingForTest);
            }
            else
                // Notify the test host
                IoCClient.TestHost.NetworkReconnected();
        }


        #endregion
    }
}
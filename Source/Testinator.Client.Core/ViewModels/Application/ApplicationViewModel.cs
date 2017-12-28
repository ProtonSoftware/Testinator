using System;
using Testinator.Core;
using Testinator.Network.Client;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Handles network communication in the application
        /// </summary>
        public ClientNetwork Network { get; set; } = new ClientNetwork();

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Login;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

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
        /// Shows which question is currently shown
        /// </summary>
        public string QuestionNumber { get; set; }

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

        #region Public Helpers

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Set the view model
            CurrentPageViewModel = viewModel;

            // Set the current page
            CurrentPage = page;

            // Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));
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

            // Change page to waiting for test page
            IoCClient.UI.ChangePage(ApplicationPage.WaitingForTest);

            // Send info package with that information
            Network.SendData(IoCClient.Client.GetPackage());
        }

        #endregion
    }
}

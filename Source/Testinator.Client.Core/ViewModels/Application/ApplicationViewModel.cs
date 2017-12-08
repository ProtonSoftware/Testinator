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
        #region Private Members

        /// <summary>
        /// The test received from server
        /// </summary>
        private Test mTest;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.WaitingForTest;

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
        public bool IsTestInProgress { get; set; }

        /// <summary>
        /// Indicates if the client is currently connected to the server
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Indicates if the client has received test
        /// </summary>
        public bool IsTestReceived { get; set; }

        /// <summary>
        /// An event to fire whenever client receives test from server
        /// </summary>
        public event Action<bool> TestReceived = (s) => { };

        /// <summary>
        /// The test user has to complete
        /// </summary>
        public Test Test
        {
            get => mTest;
            set
            {
                // If client hasn't received any test yet
                if (!IsTestReceived)
                {
                    // Get this test
                    mTest = value;

                    // Indicate that we have received it
                    IsTestReceived = true;
                    TestReceived.Invoke(IsTestReceived);
                }
            }
        }

        #endregion

        #region Application Methods

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
    }
}

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

        /// <summary>
        /// Indicates if the test has started
        /// </summary>
        private bool mIsTestInProgress;

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
        public bool IsTestInProgress
        {
            get => mIsTestInProgress;
            set
            {
                // Check if we have any test
                if (Test == null)
                    return;

                // If we do, fire an event to start doing the test
                TestStarted.Invoke(true);

                // And set this flag as true
                mIsTestInProgress = true;
            }
        }

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
        public event Action<Test> TestReceived = (s) => { };

        /// <summary>
        /// An event to fire when server send request to start the test (if we already have test of course)
        /// </summary>
        public event Action<bool> TestStarted = (s) => { };

        /// <summary>
        /// Indicates how much time is left 
        /// </summary>
        public TimeSpan TimeLeft { get; set; }

        /// <summary>
        /// Shows which question is currently shown
        /// </summary>
        public string QuestionNumber { get; set; }

        /// <summary>
        /// The test which user needs to complete
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
                    TestReceived.Invoke(mTest);
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

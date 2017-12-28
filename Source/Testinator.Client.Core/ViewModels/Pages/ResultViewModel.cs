using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for test result page 
    /// </summary>
    public class ResultViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The original test received from server
        /// </summary>
        public Test Test { get; private set; }

        /// <summary>
        /// List of questions in the test user has accomplished
        /// </summary>
        public List<Question> Questions { get; private set; }

        /// <summary>
        /// List of answers user has sent
        /// </summary>
        public List<Answer> UserAnswers { get; private set; }

        /// <summary>
        /// The time in which user has completed the test
        /// </summary>
        public TimeSpan CompletionTime => Test.Duration - IoCClient.Application.TimeLeft;

        #endregion

        #region Commands

        /// <summary>
        /// The command to exit the result page and go back to waiting for test page
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultViewModel()
        {
            // Load data from TestHost
            LoadTestHostData();

            // Create commands
            ExitCommand = new RelayCommand(Exit);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Cleans the previous test and goes back to the waiting for test page
        /// </summary>
        private void Exit()
        {
            // Clean previous test from this application
            IoCClient.TestHost.UnloadTest();

            // Go to the waiting for test page
            IoCClient.Application.GoToPage(ApplicationPage.WaitingForTest);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads data from TestHost to this view model
        /// </summary>
        private void LoadTestHostData()
        {
            // Get the original test
            Test = IoCClient.TestHost.CurrentTest;

            // Get the list of questions
            Questions = IoCClient.TestHost.Questions;

            // Get user answers
            UserAnswers = IoCClient.TestHost.UserAnswers;
        }

        #endregion
    }
}

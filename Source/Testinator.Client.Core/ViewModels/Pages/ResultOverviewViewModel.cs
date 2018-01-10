using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for test result page 
    /// </summary>
    public class ResultOverviewViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The time in which user has completed the test
        /// </summary>
        public TimeSpan CompletionTime => IoCClient.TestHost.CurrentTest.Duration - IoCClient.Application.TimeLeft;

        /// <summary>
        /// The score user achieved
        /// </summary>
        public int UserScore => IoCClient.TestHost.UserScore;

        /// <summary>
        /// Indicates if the server app has allowed user to check his answers just after he finishes his test
        /// </summary>
        public bool IsResultPageAllowed => IoCClient.TestHost.AreResultsAllowed;

        /// <summary>
        /// The mark user has achieved by doing the test
        /// </summary>
        public Marks UserMark => IoCClient.TestHost.CurrentTest.Grading.GetMark(UserScore);

        #endregion

        #region Commands

        /// <summary>
        /// The command to exit the result page and go back to waiting for test page
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// The command to open question list with answers
        /// </summary>
        public ICommand GoToQuestionsCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultOverviewViewModel()
        {
            // Create commands
            ExitCommand = new RelayCommand(Exit);
            GoToQuestionsCommand = new RelayCommand(GoToQuestions);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Cleans the previous test and goes back to the waiting for test page
        /// </summary>
        private void Exit()
        {
            // Reset the test host
            IoCClient.TestHost.Reset();

            IoCClient.Application.ReturnMainScreen();
        }

        /// <summary>
        /// Changes page to the question list with answers
        /// </summary>
        private void GoToQuestions()
        {
            // Create view model for the page
            var viewmodel = new ResultQuestionsViewModel();

            // Change page
            IoCClient.Application.GoToPage(ApplicationPage.ResultQuestionsPage, viewmodel);

            // And shot the first question
            viewmodel.ShowFirstQuestion();
        }

        #endregion

    }
}

using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for result questions page
    /// </summary>
    public class ResultQuestionsViewModel : PageHostViewModel
    {
        #region Public Properties

        /// <summary>
        /// Viewmodels for the questions
        /// </summary>
        public List<BaseViewModel> Questions { get; set; } = new List<BaseViewModel>();

        /// <summary>
        /// Name of the student that took the test
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The results that were previously selected to show more details
        /// NOTE: used only to get back to the previous page and show it correctly. Will be redone anyway
        /// </summary>
        public ServerTestResults Results { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to select a test from the list
        /// </summary>
        public ICommand SelectQuestionCommand { get; set; }
        
        /// <summary>
        /// The command to go back to the previous page
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultQuestionsViewModel(List<BaseViewModel> Items)
        {
            // Create commands
            SelectQuestionCommand = new RelayParameterizedCommand(SelectQuestion);
            ReturnCommand = new RelayCommand(Return);

            // Save items to display
            Questions = Items;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultQuestionsViewModel()
        {
            // Create commands
            SelectQuestionCommand = new RelayParameterizedCommand(SelectQuestion);
            ReturnCommand = new RelayCommand(Return);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the first question from the list
        /// </summary>
        public void ShowFirstQuestion()
        {
            // If we have any questions
            if (Questions.Count != 0)
                // Go to the first one at the start
                GoToPageByViewModel(Questions[0]);
            else
                GoToPage(ApplicationPage.None, null);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes the question being displayed
        /// </summary>
        /// <param name="obj">The index of sender item</param>
        private void SelectQuestion(object obj)
        {
            // Get the index
            var index = (int)obj;

            // Go to the corresponding page
            GoToPageByViewModel(Questions[index]);
        }

        /// <summary>
        /// Return to the previous page
        /// </summary>
        private void Return()
        {
            var viewmodel = new TestResultsDetailsViewModel(Results);
            IoCServer.Application.GoToPage(ApplicationPage.TestResultsDetails, viewmodel); 
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Changes the page based on the given viewmodel
        /// </summary>
        /// <param name="viewmodel">The view model the new page will use</param>
        private void GoToPageByViewModel(BaseViewModel viewmodel)
        {
            // Based on the type...
            if (viewmodel is QuestionMultipleCheckboxesViewModel)
            {
                GoToPage(ApplicationPage.QuestionMultipleCheckboxes, viewmodel as QuestionMultipleCheckboxesViewModel);
            }
            else if (viewmodel is QuestionMultipleChoiceViewModel)
            {
                GoToPage(ApplicationPage.QuestionMultipleChoice, viewmodel as QuestionMultipleChoiceViewModel);
            }
            else if (viewmodel is QuestionSingleTextBoxViewModel)
            {
                GoToPage(ApplicationPage.QuestionSingleTextBox, viewmodel as QuestionSingleTextBoxViewModel);
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for result questions page
    /// </summary>
    public class ResultQuestionsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current page of the subpage
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.None;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// Viewmodels for the questions
        /// </summary>
        public List<BaseViewModel> Questions => IoCClient.TestHost.QuestionViewModels;

        #endregion

        #region Commands

        /// <summary>
        /// The command to select a test from the list
        /// </summary>
        public ICommand SelectQuestionCommand { get; set; }

        #endregion

        #region Page Methods

        private void GoToPage(ApplicationPage page, BaseViewModel viewmodel)
        {
            // Set the view model
            CurrentPageViewModel = viewmodel;

            // Set the current page
            CurrentPage = page;

            // Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultQuestionsViewModel()
        {
            // Create commands
            SelectQuestionCommand = new RelayParameterizedCommand(SelectQuestion);

            // Show the first question
            if (Questions.Count != 0)
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
            // get the index
            var index = (int)obj;

            // Go to the corresponding page
            GoToPageByViewModel(Questions[index]);

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

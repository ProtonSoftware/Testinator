using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// A viewmodel for <see cref="MultipleCheckboxesQuestion"/>
    /// </summary>
    public class QuestionMultipleCheckboxesViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The question this view model is based on
        /// </summary>
        private MultipleCheckboxesQuestion mQuestion;

        #endregion

        #region Public Properties

        /// <summary>
        /// The title which shows question id
        /// </summary>
        public string QuestionPageCounter => "Pytanie " + (mQuestion.ID + 1).ToString();

        /// <summary>
        /// The task of the question
        /// </summary>
        public string Task => mQuestion.Task;

        /// <summary>
        /// Options for the questions to check or uncheck
        /// </summary>
        public List<CheckboxAnswerItemViewModel> Options;

        /// <summary>
        /// Indexs of answer currently checked and unchecked
        /// false means unchecked, true means unchecked
        /// </summary>
        public ObservableCollection<bool> CurrentlyChecked { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

        /// <summary>
        /// Sets the visibility of the no-answer warning
        /// When user chooses the answer automatically is set to false
        /// </summary>
        public bool NoAnswerWarning { get; set; } = false;

        /// <summary>
        /// Points gained for each correct answer
        /// </summary>
        public int PointScore => mQuestion.PointScore;

        #endregion

        #region Commands

        /// <summary>
        /// Submits the current question and procceds to the next question
        /// </summary>
        public ICommand SubmitCommand { get; set; }

        /// <summary>
        /// Check or unchecks the option
        /// NOTE: command parameter MUST be the answer index, INDEXING STARTS AT 1!
        /// </summary>
        public ICommand SelectCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuestionMultipleCheckboxesViewModel()
        {
            // Create commands
            SubmitCommand = new RelayCommand(Submit);
            SelectCommand = new RelayParameterizedCommand(Select);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Selects/unselects the answer
        /// </summary>
        /// <param name="obj">The index of the answer, also look at <see cref="CurrentlySelectedItdx"/></param>
        private void Select(object obj)
        {
            // Cast parameter
            var idxStr = obj as string;
            if (!Int32.TryParse(idxStr, out int idx))
                throw new NotImplementedException();

            // idx - 1 because indexing starts at 1 not 0 !
            CurrentlyChecked[idx - 1] ^= true;

            NoAnswerWarning = false;
        }

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            // If none of the options is checked show the warning to the user
            if (!CurrentlyChecked.Contains(true))
            {
                NoAnswerWarning = true;
                return;
            }

            /*
            // Save the answer
            var answer = new MultipleCheckboxesAnswer(mQuestion, new List<bool>(CurrentlyChecked));
            //IoCClient.Application.Test.AddAnswer(answer);

            // Go to next page
            var question = IoCClient.Application.Test.GetNextQuestion();

            // Based on type...
            switch(question.Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(question as MultipleChoiceQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(question as MultipleCheckboxesQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(question as SingleTextBoxQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionSingleTextBox, questionViewModel);
                        break;
                    }
            }
            */
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Adds question this view model will be based on
        /// NOTE: needs to be done before attaching this view model to the page
        /// </summary>
        /// <param name="question">The question to be attached to this viewmodel</param>
        public void AttachQuestion(MultipleCheckboxesQuestion question)
        {
            // Get the question
            mQuestion = question;

            // Convert from dictionary to answer items list
            Options = ListConvertFromDictionaryQuestion(mQuestion.OptionsAndAnswers);

            // Make all the answers unchecked
            CurrentlyChecked = new ObservableCollection<bool>();
            for (int i = 0; i < Count; i++)
                CurrentlyChecked.Add(false);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Takes in a list of strings and converts it to actual list of answer items
        /// </summary>
        /// <param name="options">Possible answers to the question as list of string</param>
        /// <returns></returns>
        private List<CheckboxAnswerItemViewModel> ListConvertFromDictionaryQuestion(Dictionary<string, bool> options)
        {
            // Initialize the list that we are willing to return 
            var FinalList = new List<CheckboxAnswerItemViewModel>();

            // Loop each answer to create answer item from it
            foreach (var option in options)
            {
                // Create new answer item
                var answerItem = new CheckboxAnswerItemViewModel();

                // Rewrite answer string content
                answerItem.Text = option.Key.ToString();

                // If answer should be checked, indicate that
                answerItem.ShouldBeChecked = option.Value;

                // Don't select any answer at the start
                answerItem.IsChecked = false;

                // Add this item to the list
                FinalList.Add(answerItem);
            }

            // We have our list done, return it
            return FinalList;
        }

        #endregion
    }
}

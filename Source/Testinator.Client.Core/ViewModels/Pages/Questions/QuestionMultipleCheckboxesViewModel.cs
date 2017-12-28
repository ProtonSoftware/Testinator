using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// A viewmodel for <see cref="MultipleCheckboxesQuestion"/>
    /// </summary>
    public class QuestionMultipleCheckboxesViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The question for this view model to show
        /// </summary>
        public MultipleCheckboxesQuestion Question { get; set; } 

        /// <summary>
        /// The title which shows question id
        /// </summary>
        public string QuestionPageCounter => "Pytanie " + (Question?.ID + 1).ToString();

        /// <summary>
        /// Options for the questions to check or uncheck
        /// </summary>
        public List<CheckboxAnswerItemViewModel> Options { get; set; }

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
        /// Indicates whether the view should be enabled for changes
        /// ReadOnly mode is used while presenting the result to the user
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Indicates if the answer given by the user is correct
        /// Makes sense only if <see cref="IsReadOnly"/> is set to true
        /// </summary>
        public bool IsAnswerCorrect { get; set; }

        /// <summary>
        /// The answer give by the user 
        /// Makes sense only if <see cref="IsReadOnly"/> is set to true
        /// </summary>
        public MultipleCheckboxesAnswer UserAnswer { get; set; }

        /// <summary>
        /// The index of the viewmodel in a viewmodels list
        /// Usefull to match the click event
        /// Used in ResultPage
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionType Type => QuestionType.MultipleCheckboxes;

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
            if (!int.TryParse(idxStr, out var idx))
                throw new ArgumentException();

            // Check the answer at idx - 1 because indexing starts at 1 not 0 !
            Options[idx - 1].IsChecked ^= true;
            
            // Disable errors
            NoAnswerWarning = false;
        }

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            // Get the list of answers
            var checkedList = new List<bool>();
            foreach (var item in Options)
            {
                if (item.IsChecked) checkedList.Add(true);
                else checkedList.Add(false);
            }

            // If none of the options is checked show the warning to the user
            if (!checkedList.Contains(true))
            {
                NoAnswerWarning = true;
                return;
            }

            // Save the answer
            var answer = new MultipleCheckboxesAnswer(checkedList);
            IoCClient.TestHost.SaveAnswer(answer);

            // Go to next question page
            IoCClient.TestHost.GoNextQuestion();
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
            Question = question;

            // Convert from dictionary to answer items list
            Options = ListConvertFromDictionaryQuestion(Question.OptionsAndAnswers);

            // Make all the answers unchecked
            foreach (var item in Options)
                item.IsChecked = false;
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
                var answerItem = new CheckboxAnswerItemViewModel
                {
                    // Rewrite answer string content
                    Text = option.Key.ToString(),

                    // Don't select any answer at the start
                    IsChecked = false
                };

                // Add this item to the list
                FinalList.Add(answerItem);
            }

            // We have our list done, return it
            return FinalList;
        }

        #endregion
    }
}

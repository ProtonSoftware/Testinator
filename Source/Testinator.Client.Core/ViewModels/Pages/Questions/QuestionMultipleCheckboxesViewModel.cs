using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// A viewmodel for <see cref="MultipleCheckBoxesQuestion"/>
    /// </summary>
    public class QuestionMultipleCheckboxesViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The question for this view model to show
        /// </summary>
        public MultipleCheckBoxesQuestion Question { get; set; }

        /// <summary>
        /// The title which shows question id
        /// </summary>
        public string QuestionPageCounter =>
            IsReadOnly ? "Pytanie " + DisplayIndex + " / " + IoCClient.TestHost.Questions.Count : "Pytanie " + IoCClient.TestHost.CurrentQuestionString;

        /// <summary>
        /// Options for the questions to check or uncheck
        /// </summary>
        public List<CheckboxAnswerItemViewModel> Options { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

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
        /// Indicates if the answer is empty (used to show now answer notification
        /// Makes sense only if ReadOnlyMode is enabled
        /// </summary>
        public bool NoAnswer { get; set; }

        /// <summary>
        /// The answer give by the user 
        /// Makes sense only if <see cref="IsReadOnly"/> is set to true
        /// </summary>
        public MultipleCheckBoxesAnswer UserAnswer { get; set; }

        /// <summary>
        /// The index of the viewmodel in a viewmodels list
        /// Usefull to match the click event
        /// Used in ResultPage
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The index of the question displayed on the list
        /// (Just index+1 to start indexing from 1 not from 0)
        /// Makes sense if ReadOnly mode is active
        /// </summary>
        public int DisplayIndex => Index + 1;

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

            // Save the answer
            var answer = new MultipleCheckBoxesAnswer()
            {
                UserAnswer = checkedList,
            };

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
        /// <param name="ReadOnly">Indicates if this viewmodel is readonly</param>
        public void AttachQuestion(MultipleCheckBoxesQuestion question, bool ReadOnly = false)
        {
            // Get the question
            Question = question;

            // Convert from dictionary to answer items list
            Options = ListConvertQuestion();

            IsReadOnly = ReadOnly;

            // If not in readonly mode...
            if (!IsReadOnly)
            {
                // Make all the answers unchecked
                foreach (var item in Options)
                    item.IsChecked = false;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Converts the question to actual list of answer items
        /// </summary>
        /// <returns></returns>
        private List<CheckboxAnswerItemViewModel> ListConvertQuestion()
        {
            // Initialize the list that we are willing to return 
            var FinalList = new List<CheckboxAnswerItemViewModel>();

            var i = 0;
            // Loop each answer to create answer item from it
            foreach (var option in Question.Options)
            {
                // Create new answer item
                var answerItem = new CheckboxAnswerItemViewModel
                {
                    // Rewrite answer string content
                    Text = option,

                    // Set the read only property if required
                    IsReadOnly = IsReadOnly,
    
                };

                // In readonly mode set the check box
                if (IsReadOnly)
                {
                    // If the user answer is null (meaning the user didnt answer this question), skip this iteration
                    if (UserAnswer != null)
                    {
                        answerItem.IsChecked = UserAnswer.UserAnswer[i];
                        answerItem.IsCorrect = Question.CorrectAnswer[i] == UserAnswer.UserAnswer[i];
                    }
                    else
                    {
                        // Mark the checkboxes in the correct order if the user didn't give the answer
                        answerItem.IsChecked = Question.CorrectAnswer[i];
                        NoAnswer = true;
                    }
                }
                else
                    // Don't select any answer at the start
                    answerItem.IsChecked = true;

                // Add this item to the list
                FinalList.Add(answerItem);

                i++;
            }

            // We have our list done, return it
            return FinalList;
        }

        #endregion
    }
}

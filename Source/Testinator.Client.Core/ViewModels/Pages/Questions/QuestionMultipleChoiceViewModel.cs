using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for <see cref="MultipleChoiceQuestion"/>
    /// </summary>
    public class QuestionMultipleChoiceViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The question for this view model to show
        /// </summary>
        public MultipleChoiceQuestion Question { get; set; }

        /// <summary>
        /// The title which shows question id
        /// </summary>
        public string QuestionPageCounter => "Pytanie " + IoCClient.TestHost.QuestionNumber;

        /// <summary>
        /// Options for the questions to choose from eg. A, B, C...
        /// </summary>
        public List<ABCAnswerItemViewModel> Options { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

        /// <summary>
        /// Sets the visibility of the no answer warning
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
        public MultipleChoiceAnswer UserAnswer { get; set; }

        /// <summary>
        /// The index of the viewmodel in a viewmodels list
        /// Usefull to match the click event
        /// Used in ResultPage
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionType Type => QuestionType.MultipleChoice;

        #endregion

        #region Commands

        /// <summary>
        /// Submits the current question and procceds to the next question
        /// </summary>
        public ICommand SubmitCommand { get; set; }

        /// <summary>
        /// Selects an answer
        /// </summary>
        public ICommand SelectCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        public QuestionMultipleChoiceViewModel()
        {
            // Create commands
            SubmitCommand = new RelayCommand(Submit);
            SelectCommand = new RelayParameterizedCommand(Select); 
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            // Check which answer is selected
            var CurrentlySelectedIdx = CheckWhichAnswerIsSelected();

            // If none, show error and don't submit
            if (CurrentlySelectedIdx == 0)
            {
                NoAnswerWarning = true;
                return;
            }

            // Save the answer
            var answer = new MultipleChoiceAnswer(CurrentlySelectedIdx);
            IoCClient.TestHost.SaveAnswer(answer);

            // Go to next question page
            IoCClient.TestHost.GoNextQuestion();
        }

        /// <summary>
        /// Fired when the user clicks on an answer
        /// </summary>
        /// <param name="idx">The index of the answer being cliked (int)</param>
        private void Select(object idx)
        {
            // If read only dont let the user select answer
            if (IsReadOnly)
                return;

            // Get the index
            var index = (int)idx;

            // If the user clicks on the answer that is already selected don't do anything
            if (Options[index - 1].IsSelected == true)
                return;

            UnCheckAllAnswers();

            // Select the answer
            Options[index - 1].IsSelected = true;
        }

        #endregion

        #region Public Helpers
        
        /// <summary>
        /// Adds question this view model will be based on
        /// NOTE: needs to be done before attaching this view model to the page
        /// </summary>
        /// <param name="question">The question to be attached to this viewmodel</param>
        public void AttachQuestion(MultipleChoiceQuestion question)
        {
            // Get the question
            Question = question;

            // Convert the list of string to list of ABCAnswerItemViewModel
            Options = ListConvertFromStringQuestion(Question.Options);

            if (IsReadOnly)
            {
                Options[UserAnswer.SelectedAnswerIdx - 1].IsSelected = true;
                Options[Question.CorrectAnswerIndex - 1].IsSelected = true;

                Options[UserAnswer.SelectedAnswerIdx - 1].IsAnswerGivenByTheUser = true;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks which item in the answers list is selected
        /// </summary>
        /// <returns></returns>
        private int CheckWhichAnswerIsSelected()
        {
            // Keep track of the index of the item which is selected
            var index = 0;

            // Loop each item
            for(var i = 1; i <= Count; i++)
            {
                // Check if any item is selected
                if (Options[i-1].IsSelected) index = i;
            }

            // Return index (if none items were selected, we return 0
            return index;
        }

        /// <summary>
        /// Takes in a list of strings and converts it to actual list of answer items
        /// </summary>
        /// <param name="options">Possible answers to the question as list of string</param>
        /// <returns></returns>
        private List<ABCAnswerItemViewModel> ListConvertFromStringQuestion(List<string> options)
        {
            // Initialize the list that we are willing to return 
            var FinalList = new List<ABCAnswerItemViewModel>();

            // Loop each answer to create answer item from it
            var answerCounter = 1;
            foreach (var option in options)
            {
                // Create new answer item with appropriate index
                var answerItem = new ABCAnswerItemViewModel()
                {
                    Index = answerCounter,
                };

                // Attach proper letter to it
                switch(answerCounter)
                {
                    case 1: answerItem.Letter = "A"; break;
                    case 2: answerItem.Letter = "B"; break;
                    case 3: answerItem.Letter = "C"; break;
                    case 4: answerItem.Letter = "D"; break;
                    case 5: answerItem.Letter = "E"; break;
                }

                // Rewrite answer string content
                answerItem.Text = option;

                // Don't select any answer at the start
                answerItem.IsSelected = false;

                // Add this item to the list
                FinalList.Add(answerItem);

                // Go to the next answer
                answerCounter++;
            }

            // We have our list done, return it
            return FinalList;
        }

        /// <summary>
        /// Marks all the answers not selected
        /// </summary>
        private void UnCheckAllAnswers()
        {
            // Uncheck all the answers
            foreach (var option in Options)
            {
                option.IsSelected = false;
            }
        }

        #endregion
    }
}

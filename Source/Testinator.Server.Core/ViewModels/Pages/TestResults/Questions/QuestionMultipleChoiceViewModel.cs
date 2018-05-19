using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
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
        public string QuestionPageCounter => $"Pytanie {DisplayIndex}";

        /// <summary>
        /// The ammout of points the user can get for a good answer
        /// </summary>
        public string PointScore => $"{Question.Scoring.FullPointScore}p.";

        /// <summary>
        /// Options for the questions to choose from eg. A, B, C...
        /// </summary>
        public List<ABCAnswerItemViewModel> Options { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

        /// <summary>
        /// Indicates if the answer is empty (used to show now answer notification
        /// </summary>
        public bool NoAnswer { get; set; }

        /// <summary>
        /// Indicates if the answer given by the user is correct
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
        /// The index of the question displayed on the list
        /// (Just index+1 to start indexing from 1 not from 0)
        /// Makes sense if ReadOnly mode is active
        /// </summary>
        public int DisplayIndex => Index + 1;

        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionType Type => QuestionType.MultipleChoice;

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


            // Set the correct answer selected so it's green initially
            Options[Question.CorrectAnswerIndex].IsSelected = true;

            // If the answer is null, return (means that the user gave no answer to this question) 
            if (UserAnswer == null)
            {
                NoAnswer = true;
                return;
            }

            // Set the user's answer selected so it's green
            Options[UserAnswer.SelectedAnswerIndex].IsSelected = true;

            // Mark the user's answer to display "Your answer" sign
            Options[UserAnswer.SelectedAnswerIndex].IsAnswerGivenByTheUser = true;

            // Indicate if the user's answer is correct
            Options[UserAnswer.SelectedAnswerIndex].IsAnswerCorrect = IsAnswerCorrect;
            
        }

        #endregion

        #region Private Helpers

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

        #endregion
    }
}

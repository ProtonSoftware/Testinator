using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Server.Core
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
        public string QuestionPageCounter => $"Pytanie {DisplayIndex}";

        /// <summary>
        /// The ammout of points the user can get for a good answer
        /// </summary>
        public string PointScore => $"{Question.Scoring.FullPointScore}p.";

        /// <summary>
        /// Options for the questions to check or uncheck
        /// </summary>
        public List<CheckboxAnswerItemViewModel> Options { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

        /// <summary>
        /// Indicates if the answer given by the user is correct
        /// </summary>
        public bool IsAnswerCorrect { get; set; }

        /// <summary>
        /// Indicates if the answer is empty (used to show now answer notification
        /// </summary>
        public bool NoAnswer { get; set; }

        /// <summary>
        /// The answer given by the user 
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

        #region Public Helpers

        /// <summary>
        /// Adds question this view model will be based on
        /// NOTE: needs to be done before attaching this view model to the page
        /// </summary>
        /// <param name="question">The question to be attached to this viewmodel</param>
        public void AttachQuestion(MultipleCheckBoxesQuestion question)
        {
            // Get the question
            Question = question;

            // Convert from dictionary to answer items list
            Options = ListConvertQuestions();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Takes in a list of strings and converts it to actual list of answer items
        /// </summary>
        /// <param name="options">Possible answers to the question as list of string</param>
        /// <returns></returns>
        private List<CheckboxAnswerItemViewModel> ListConvertQuestions()
        {
            // Initialize the list that we are willing to return 
            var FinalList = new List<CheckboxAnswerItemViewModel>();

            var i = 0;
            // Loop each answer to create answer item from it
            foreach (var option in Question.Options)
            {
                // Create new answer item
                var answerItemViewModel = new CheckboxAnswerItemViewModel
                {
                    // Rewrite answer string content
                    Text = option,  
                };

                // If user gave no answer
                if (UserAnswer == null)
                {

                    // Mark the checkboxes in the correct order if the user didn't give the answer
                    answerItemViewModel.IsChecked = Question.CorrectAnswer[i];
                    NoAnswer = true;
                }
                
                // If user gave an answer
                else
                {
                    // Show their answer
                    answerItemViewModel.IsChecked = UserAnswer.UserAnswer[i];

                    // Set if it is correct
                    answerItemViewModel.IsCorrect = Question.CorrectAnswer[i] == UserAnswer.UserAnswer[i];
                }
                
                // Add this item viewmodel to the list
                FinalList.Add(answerItemViewModel);

                i++;
            }

            return FinalList;
        }

        #endregion
    }
}

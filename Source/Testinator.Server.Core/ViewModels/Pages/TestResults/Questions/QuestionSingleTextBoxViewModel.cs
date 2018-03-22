using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// A viewmodel for <see cref="SingleTextBoxQuestion"/>
    /// </summary>
    public class QuestionSingleTextBoxViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The question for this view model to show
        /// </summary>
        public SingleTextBoxQuestion Question { get; set; }

        /// <summary>
        /// The title which shows question id
        /// </summary>
        public string QuestionPageCounter => $"Pytanie {DisplayIndex}";

        /// <summary>
        /// The ammout of points the user can get for a good answer
        /// </summary>
        public string PointScore => $"{Question.PointScore}p.";

        /// <summary>
        /// Current answer written by the user
        /// </summary>
        public string UserAnswer { get; set; }

        /// <summary>
        /// Indicates if the answer given by the user is correct
        /// </summary>
        public bool IsAnswerCorrect { get; set; }

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
        /// Indicates if the text field with correct answer should be visible
        /// </summary>
        public bool IsCorrectAnswerVisible => !IsAnswerCorrect;

        /// <summary>
        /// The color of the textbox background
        /// Light White if not in readonly mode
        /// Green if answer is correct, red if not
        /// </summary>
        public string TextBoxBackground => IsAnswerCorrect ? "57A598" : "FF0000";

        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionType Type => QuestionType.SingleTextBox;

        #endregion

        #region Public Helpers

        /// <summary>
        /// Adds question this view model will be based on
        /// NOTE: needs to be done before attaching this view model to the page
        /// </summary>
        /// <param name="question">The question to be attached to this viewmodel</param>
        public void AttachQuestion(SingleTextBoxQuestion question)
        {
            // Save the question
            Question = question;
        }

        #endregion
    }
}

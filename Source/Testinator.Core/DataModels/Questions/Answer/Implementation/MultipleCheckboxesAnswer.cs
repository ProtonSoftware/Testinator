using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    #region OLD

    /// <summary>
    /// The answer for multiple checkboxes question
    /// </summary>
    [Serializable]
    public class MultipleCheckboxesAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// The answer itself
        /// </summary>
        public List<bool> Answers { get; set; } = new List<bool>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="answer">The answer to the question</param>
        public MultipleCheckboxesAnswer(List<bool> answer)
        {
            Type = QuestionType.MultipleCheckboxes;
            Answers = answer;
        }
        
        #endregion
    }

    #endregion

    #region NEW

    /// <summary>
    /// The answer for <see cref="MultipleCheckBoxesQuestionXXX"/>
    /// </summary>
    [Serializable]
    public class MultipleCheckBoxesAnswerXXX : AnswerXXX
    {
        #region Public Properties

        /// <summary>
        /// The type of question this answer is for
        /// </summary>
        public override QuestionType Type => QuestionType.MultipleCheckboxes;

        /// <summary>
        /// The answer user gave to the question
        /// </summary>
        public List<bool> UserAnswer { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the answer is empty, meaning the user did not answer this question
        /// </summary>
        /// <returns>True if empty, otherwise false</returns>
        public override bool IsEmpty() => UserAnswer == null;

        #endregion
    }

    #endregion

}

using System;

namespace Testinator.Core
{

    #region OLD

    /// <summary>
    /// The answer for multiple choice question
    /// </summary>
    [Serializable]
    public class SingleTextBoxAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// The answer itself
        /// </summary>
        public string Answer { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="answer">The answer for the question</param>
        public SingleTextBoxAnswer(string answer)
        {
            Type = QuestionType.SingleTextBox;
            Answer = answer;
        }
        
        #endregion
    }

    #endregion

    #region NEW

    /// <summary>
    /// The answer for <see cref="SingleTextBoxAnswerXXX"/>
    /// </summary>
    [Serializable]
    public class SingleTextBoxAnswerXXX : AnswerXXX
    {
        #region Public Proprties

        /// <summary>
        /// The type of question this answer is for
        /// </summary>
        public override QuestionType Type => QuestionType.SingleTextBox;

        /// <summary>
        /// The answer user gave during the test
        /// </summary>
        public string UserAnswer { get; set; }

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

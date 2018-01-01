using System;

namespace Testinator.Core
{
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
}

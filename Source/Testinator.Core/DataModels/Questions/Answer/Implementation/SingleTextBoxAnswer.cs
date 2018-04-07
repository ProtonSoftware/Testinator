using System;

namespace Testinator.Core
{
    /// <summary>
    /// The answer for <see cref="SingleTextBoxAnswer"/>
    /// </summary>
    [Serializable]
    public class SingleTextBoxAnswer : Answer
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
}

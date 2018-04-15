using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The answer for <see cref="MultipleCheckBoxesQuestion"/>
    /// </summary>
    [Serializable]
    public class MultipleCheckBoxesAnswer : Answer
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
}

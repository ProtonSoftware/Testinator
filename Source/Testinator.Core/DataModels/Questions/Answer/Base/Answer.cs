using System;

namespace Testinator.Core
{
    /// <summary>
    /// Base class for any answer to derive from
    /// </summary>
    [Serializable]
    public abstract class Answer : IAnswer
    {
        #region Public Properties

        /// <summary>
        /// The type of question this answer is for
        /// </summary>
        public abstract QuestionType Type { get; }

        #endregion

        #region Public Abstract Methods

        /// <summary>
        /// Checks if the answer is empty, meaning the user did not answer this question
        /// </summary>
        /// <returns>True if empty, otherwise false</returns>
        public abstract bool IsEmpty();

        #endregion
    }
}

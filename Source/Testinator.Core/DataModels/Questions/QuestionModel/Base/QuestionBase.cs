using System;

namespace Testinator.Core
{
    /// <summary>
    /// A base model for all questions
    /// </summary>
    [Serializable]
    public abstract class Question
    {
        #region Public Properties 

        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionType Type { get; protected set; }

        /// <summary>
        /// Identifier of this question
        /// Useful for matching answers to questions
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Number of points given for a good answer
        /// </summary>
        public int PointScore { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public abstract Question Clone();

        #endregion
    }
}

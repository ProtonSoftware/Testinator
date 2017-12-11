using System;

namespace Testinator.Core
{
    /// <summary>
    /// A base model for all questions
    /// </summary>
    [Serializable]
    public abstract class Question
    {
        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionType Type { get; protected set; }

        /// <summary>
        /// Identifier of this question
        /// Usefull for matching answers to questions
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public abstract Question Clone();
    }
}

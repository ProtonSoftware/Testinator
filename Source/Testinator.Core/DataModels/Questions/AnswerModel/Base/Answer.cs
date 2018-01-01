using System;

namespace Testinator.Core
{
    /// <summary>
    /// The base class for every answer
    /// </summary>
    [Serializable]
    public abstract class Answer
    {
        /// <summary>
        /// The type of the question this answer is for
        /// </summary>
        public QuestionType Type { get; protected set; }

        /// <summary>
        /// Id of this answer. NOTE: useful to match this answer to the question
        /// </summary>
        public int ID { get; set; }
    }
}

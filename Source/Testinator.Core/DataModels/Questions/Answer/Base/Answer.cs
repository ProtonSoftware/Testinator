using System;

namespace Testinator.Core
{
    #region OLD

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
        /// Id of this answer. 
        /// NOTE: useful to match this answer to the question
        /// </summary>
        public int ID { get; set; }
    }

    #endregion


    #region NEW

    /// <summary>
    /// Base class for any answer to derive from
    /// </summary>
    [Serializable]
    public abstract class AnswerXXX : IAnswer
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

    #endregion
}

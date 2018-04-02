using System;

namespace Testinator.Core
{
    #region OLD
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
    #endregion

    #region NEW

    /// <summary>
    /// Base class for any question to derive from
    /// </summary>
    [Serializable]
    public abstract class QuestionXXX
    {
        #region Public Properties

        /// <summary>
        /// The task of this question
        /// </summary>
        public TaskContent Task { get; set; }

        /// <summary>
        /// The type of this question
        /// </summary>
        public abstract QuestionType Type { get; }

        /// <summary>
        /// The evaluation criteria for this question
        /// </summary>
        public Scoring Scoring { get; set; }

        #endregion
        
        #region Public Abstract Methods

        /// <summary>
        /// Checks if the answer is correct 
        /// </summary>
        /// <param name="Answer">Answer to validate</param>
        /// <returns>Number of points for this answer depending on current valuation criteria</returns>
        public abstract int CheckAnswer(AnswerXXX Answer);

        /// <summary>
        /// Gets the number of points for a correct answer in format that is easy to display
        /// </summary>
        /// <returns>Number of points that can be displayed easily</returns>
        public abstract string GetDisplayPointScore();

        #endregion
    }

    #endregion
}

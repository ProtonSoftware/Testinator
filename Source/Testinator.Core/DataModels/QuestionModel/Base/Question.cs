namespace Testinator.Core
{
    /// <summary>
    /// A model for a question
    /// </summary>
    public class Question
    {
        #region Public Properties

        /// <summary>
        /// Number of points given for a good answer
        /// </summary>
        public int PointScore { get; set; }

        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionTypes Type { get; set; }

        /// <summary>
        /// The content of the question, task, correct answer etc.
        /// </summary>
        public QuestionContent Content { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Question()
        {

        }

        #endregion
    }
}

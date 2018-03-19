using System;

namespace Testinator.Core
{
    /// <summary>
    /// A question with a single box to write the answer in
    /// </summary>
    [Serializable]
    public class SingleTextBoxQuestion : Question
    {
        #region Public Properties

        /// <summary>
        /// The task itself
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Correct answer to the question as a string
        /// </summary>
        public string CorrectAnswer { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>A copy of the given object</returns>
        public override Question Clone() => new SingleTextBoxQuestion()
        {
            Task = this.Task,
            CorrectAnswer = this.CorrectAnswer,
            PointScore = this.PointScore,
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SingleTextBoxQuestion()
        {
            // Set the type
            Type = QuestionType.SingleTextBox;
        }

        #endregion
    }
}

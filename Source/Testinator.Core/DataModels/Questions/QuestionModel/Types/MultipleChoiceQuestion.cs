using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// A multiple choice question, A, B, C...
    /// </summary>
    [Serializable]
    public class MultipleChoiceQuestion : Question
    {
        #region Public Properties

        /// <summary>
        /// Options for the question to chose from. 
        /// A, B, C etc.
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// The task itself
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Gets or sets the correct answer 
        /// WARNING: indexing starts at 1 NOT 0
        /// A is 1, B is 2, etc.
        /// </summary>
        public int CorrectAnswerIndex { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public override Question Clone() => new MultipleChoiceQuestion()
        {
            Task = this.Task,
            CorrectAnswerIndex = this.CorrectAnswerIndex,
            Options = this.Options,
            PointScore = this.PointScore,
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleChoiceQuestion()
        {
            // Set the type
            Type = QuestionType.MultipleChoice;
        }

        #endregion
    }
}

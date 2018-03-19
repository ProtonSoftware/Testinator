using System;
using System.Collections.Generic;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// A multiple checkboxes question type
    /// </summary>
    [Serializable]
    public class MultipleCheckboxesQuestion : Question
    {
        #region Public Properties

        /// <summary>
        /// Options for the question to be checked or not
        /// </summary>
        public Dictionary<string, bool> OptionsAndAnswers { get; set; }

        /// <summary>
        /// The task itself
        /// </summary>
        public string Task { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public override Question Clone() => new MultipleCheckboxesQuestion()
        {
            Task = this.Task,
            OptionsAndAnswers = this.OptionsAndAnswers,
            PointScore = this.PointScore,
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleCheckboxesQuestion()
        {
            // Set the type
            Type = QuestionType.MultipleCheckboxes;
        }

        #endregion
    }
}

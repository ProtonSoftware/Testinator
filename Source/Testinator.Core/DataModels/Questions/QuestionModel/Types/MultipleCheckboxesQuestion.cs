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
        #region Private Members

        /// <summary>
        /// The task itself
        /// </summary>
        private string mTask;

        /// <summary>
        /// Options for the question and at this same type model answer
        /// 'false' means that it should be unchecked
        /// 'ture' that checked
        /// </summary>
        private Dictionary<string,bool> mOptionsAndAnswers = new Dictionary<string, bool>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Options for the question to be checked or not
        /// </summary>
        public Dictionary<string, bool> OptionsAndAnswers
        {
            get => mOptionsAndAnswers;
            set
            {
                if (value.Count < 1)
                    throw new QuestionException(QuestionExceptionTypes.NotEnoughOptions);
                mOptionsAndAnswers = value;
            }
        }

        /// <summary>
        /// The task itself
        /// </summary>
        public string Task
        {
            get => mTask;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new QuestionException(QuestionExceptionTypes.NullOrEmptyString);
                mTask = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public override Question Clone()
        {
            return new MultipleCheckboxesQuestion()
            {
                Task = this.Task,
                OptionsAndAnswers = this.OptionsAndAnswers,
                PointScore = this.PointScore,
            };
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleCheckboxesQuestion()
        {
            Type = QuestionType.MultipleCheckboxes;

            // Create defaults
            Task = "Wpisz pytanie";
            OptionsAndAnswers = new Dictionary<string, bool>() { { "opcja1", true } };
            PointScore = 1;
        }

        #endregion
    }
}

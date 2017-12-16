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
        #region Private Members

        /// <summary>
        /// Index of the correct answer in options list
        /// </summary>
        private int mCorrectIdx;

        /// <summary>
        /// The task itself
        /// </summary>
        private string mTask;

        /// <summary>
        /// Options for the question to chose from. 
        /// A, B, C etc.
        /// </summary>
        private List<string> mOptions;

        #endregion

        #region Public Properties

        /// <summary>
        /// Options for the question to chose from. 
        /// A, B, C etc.
        /// </summary>
        public List<string> Options
        {
            get => mOptions;
            set
            {
                //if (value.Count < 2)
                //    throw new QuestionException(QuestionExceptionTypes.NotEnoughOptions);
                mOptions = value;
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

        /// <summary>
        /// Gets or sets the correct answer 
        /// WARNING: indexing starts at 1 NOT 0
        /// A is 1, B is 2, etc.
        /// </summary>
        public int CorrectAnswerIndex
        {
            get => mCorrectIdx;
            set
            {
                // <= because we presume that indexing starts at 1 not 0
                if (value <= 0)
                    throw new QuestionException(QuestionExceptionTypes.WrongIndex);
                mCorrectIdx = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public override Question Clone( )
        {
            return new MultipleChoiceQuestion()
            {
                Task = this.Task,
                CorrectAnswerIndex = this.CorrectAnswerIndex,
                Options = this.Options,
                PointScore = this.PointScore,
            };
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleChoiceQuestion()
        {
            Type = QuestionType.MultipleChoice;

            // Create defaults
            Task = "Wpisz pytanie";
            Options = new List<string>() { "Odpowiedź A", "Odpowiedź B" };
            CorrectAnswerIndex = 1;
            PointScore = 1;
        }

        #endregion
    }
}

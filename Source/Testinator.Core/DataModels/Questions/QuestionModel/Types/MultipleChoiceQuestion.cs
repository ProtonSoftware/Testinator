using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// A multiple choice question, A, B, C...
    /// </summary>
    public class MultipleChoiceQuestion : Question
    {
        #region Private Members

        /// <summary>
        /// Index of the correct answer in options list
        /// </summary>
        private int mCorrectIdx;

        /// <summary>
        /// Question value in points
        /// </summary>
        private int mScore;

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
        /// Number of points given for a good answer
        /// </summary>
        public int PointScore
        {
            get => mScore;
            set
            {
                if (value < 0)
                    throw new QuestionException(QuestionExceptionTypes.PointScoreLessThanZero);
                mScore = value;
            }
        }

        /// <summary>
        /// Options for the question to chose from. 
        /// A, B, C etc.
        /// </summary>
        public List<string> Options
        {
            get => mOptions;
            set
            {
                if (value.Count < 2)
                    throw new QuestionException(QuestionExceptionTypes.NotEnoughOptions);
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
                if (value > Options.Count || value <= 0)
                    throw new QuestionException(QuestionExceptionTypes.WrongIndex);
                mCorrectIdx = value - 1;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleChoiceQuestion()
        {
            Type = QuestionTypes.MultipleChoice;

            // Create defaults
            Task = "Wpisz pytanie";
            Options = new List<string>() { "Odpowiedź A", "Odpowiedź B" };
            CorrectAnswerIndex = 1;
            PointScore = 1;
        }

        #endregion
    }
}

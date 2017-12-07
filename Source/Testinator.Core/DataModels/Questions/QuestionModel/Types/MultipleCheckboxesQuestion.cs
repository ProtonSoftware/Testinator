using System.Collections.Generic;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// A multiple chceckboxes question type
    /// </summary>
    public class MultipleCheckboxesQuestion : Question
    {
        #region Private Properties

        /// <summary>
        /// Question value in points
        /// </summary>
        private int mScore;

        /// <summary>
        /// The task itself
        /// </summary>
        private string mTask;

        /// <summary>
        /// Options for the question and at this same type model answer
        /// 'false' means that it should be unchecked
        /// 'ture' that checked
        /// </summary>
        private Dictionary<string,bool> mOptionsAndAnswers;

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
        /// Gets the options for the question as list
        /// </summary>
        /// <returns>The options as a list</returns>
        public List<string> OptionList()
        {
            return OptionsAndAnswers.Keys.ToList();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleCheckboxesQuestion()
        {
            Type = QuestionTypes.MultipleCheckboxes;

            // Create defaults
            Task = "Wpisz pytanie";
            OptionsAndAnswers = new Dictionary<string, bool>() { { "opcja1", true } };
            PointScore = 1;
        }

        #endregion
    }
}

namespace Testinator.Core
{
    /// <summary>
    /// A question with a single box to write the answer in
    /// </summary>
    public class SingleTextBoxQuestion : Question
    {
        #region Private Members 

        /// <summary>
        /// Question value in points
        /// </summary>
        private int mScore;

        /// <summary>
        /// The task itself
        /// </summary>
        private string mTask;

        /// <summary>
        /// Index of the correct answer in options list
        /// </summary>
        private string mCorrectAnswer;

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
        /// Correct answer to the question as a string
        /// </summary>
        public string CorrectAnswer
        {
            get => mCorrectAnswer;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new QuestionException(QuestionExceptionTypes.NullOrEmptyString);

                mCorrectAnswer = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SingleTextBoxQuestion()
        {
            Type = QuestionType.SingleTextBox;

            // Create defaults
            Task = "Wpisz pytanie";
            CorrectAnswer = "odpowiedz";
            PointScore = 1;
        }

        #endregion
    }
}

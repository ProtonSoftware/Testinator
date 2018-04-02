using System;

namespace Testinator.Core
{
    #region Old
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

    #endregion

    #region NEW

    /// <summary>
    /// A question with a single box input to write the answer in
    /// </summary>
    [Serializable]
    public class SingleTextBoxQuestionXXX : QuestionXXX
    {
        #region Public Properties

        /// <summary>
        /// The type of this question
        /// </summary>
        public override QuestionType Type => QuestionType.SingleTextBox;
        
        /// <summary>
        /// The correct answer for this question.
        /// NOTE: Case sensitive property can be set in <see cref="Evaluation"/> object; false by default.
        /// </summary>
        public string CorrectAnswer { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the answer is correct
        /// </summary>
        /// <param name="Answer">The answer to be checked</param>
        /// <returns>Number of points for this answer depending on current valuation criteria</returns>
        public override int CheckAnswer(AnswerXXX Answer)
        {
            if (Answer.IsEmpty())
                return 0;

            var TargetAnswer = Answer as SingleTextBoxAnswerXXX;

            // The type should match but need to check it just in case
            if (TargetAnswer == null)
                return 0;

            // Depending on evluation criteria thease can change
            var FinalUserAnswer = TargetAnswer.UserAnswer;
            var FinalCorrectAnswer = CorrectAnswer;

            // Take into account all criteria

            // If not case sensitive make everything lower case
            if (!Scoring.IsCaseSensitive)
            {
                FinalUserAnswer = FinalUserAnswer.ToLower();
                FinalCorrectAnswer = FinalCorrectAnswer.ToLower();
            }
            
            // If white spaces are unimportant delete them
            if (!Scoring.AreWhiteSpacesImportant)
            {
                FinalUserAnswer = FinalUserAnswer.Replace(" ", string.Empty);
                FinalCorrectAnswer = FinalCorrectAnswer.Replace(" ", string.Empty);
            }

            return FinalUserAnswer == FinalCorrectAnswer ? Scoring.FullPointScore : 0;
        }

        /// <summary>
        /// Gets point score as a handy string that can be displayed to the user
        /// </summary>
        /// <returns>Handy <see cref="PointScore"/> string</returns>
        public override string GetDisplayPointScore() => Scoring.FullPointScore.ToString();

        #endregion
    }

    #endregion
}

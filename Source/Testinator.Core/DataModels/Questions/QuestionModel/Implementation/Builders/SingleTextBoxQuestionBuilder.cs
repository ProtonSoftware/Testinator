using Testinator.Core.QuestionBuilders;

namespace Testinator.Core
{
    /// <summary>
    /// The concrete builder to create <see cref="SingleTextBoxQuestionXXX"/>
    /// </summary>
    public class SingleTextboxQuestionBuilder : QuestionBuilder<SingleTextBoxQuestionXXX, string, string>
    {
        #region Private Specific Methods

        /// <summary>
        /// Checks if the question's content is valid
        /// </summary>
        /// <returns>True if everything is correct, otherwise false</returns>
        protected override bool IsValid()
        {
            // There is nothing to check as everything has been already checked during construction process
            return true;
        }

        /// <summary>
        /// Checks if <see cref="CreatedQuestion"/> has options attached to it
        /// NOTE: always true as this type of question doesn't support options
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected override bool HasOptions() => true;

        /// <summary>
        /// Checks if <see cref="CreatedQuestion"/> has correct answer attached to it
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected override bool HasCorrectAnswer() => !string.IsNullOrEmpty(CreatedObject.CorrectAnswer);

        #endregion

        #region Public Construction Methods

        /// <summary>
        /// Adds correct answer to the test
        /// </summary>
        /// <param name="CorrectAnswer"></param>
        public override void AddCorrectAnswer(string CorrectAnswer)
        {
            // Make sure the string is valid
            if (string.IsNullOrWhiteSpace(CorrectAnswer))
                return;

            CreatedObject.CorrectAnswer = CorrectAnswer;
            IsCorrectAnswerAttached = true;
        }

        /// <summary>
        /// Not used in this type of question
        /// </summary>
        /// <param name="Options"></param>
        public override void AddOptions(string Options)
        {
            return;
        }

        /// <summary>
        /// Adds scoring evaluation criteria for this question
        /// </summary>
        /// <param name="Scoring">Specific evaluation criteria. 
        /// NOTE: In this case <see cref="Scoring.Mode"/> is automatically set to <see cref="ScoringMode.FullAnswer"/> 
        /// as nothing else makes sense here.
        /// Meaningful paremeters: <see cref="Scoring.IsCaseSensitive"/></param>
        public override void AddScoring(Scoring Scoring)
        {
            // Make sure data is valid
            if (Scoring == null || Scoring.FullPointScore <= 0)
                return;

            // Set the mode automatically
            Scoring.Mode = ScoringMode.FullAnswer;

            // Attach it to the question
            CreatedObject.Scoring = Scoring;
            IsScoringAttached = true;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SingleTextboxQuestionBuilder() : base()
        {
            // As this type of question has no options to attached pretend they were added so the
            // builder can return the result
            AreOptionsAttached = true;
        }

        /// <summary>
        /// Constructor accepting the model question to work with
        /// Works as an editor in this context
        /// </summary>
        /// <param name="Prototype">The prototype question</param>
        public SingleTextboxQuestionBuilder(SingleTextBoxQuestionXXX Prototype) : base(Prototype) { }

        #endregion
    }

}

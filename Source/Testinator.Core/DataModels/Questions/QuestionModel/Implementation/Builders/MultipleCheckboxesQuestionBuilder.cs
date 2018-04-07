using System.Collections.Generic;
using Testinator.Core.QuestionBuilders;

namespace Testinator.Core
{
    /// <summary>
    /// The concrete builder to create <see cref="MultipleCheckBoxesQuestion"/>
    /// </summary>
    public class MultipleCheckBoxesQuestionBuilder : QuestionBuilder<MultipleCheckBoxesQuestion, List<string>, List<bool>>
    {
        #region Private Specific Methods

        /// <summary>
        /// Checks if the question's content is valid
        /// </summary>
        /// <returns>True if everything is correct, otherwise false</returns>
        protected override bool IsValid()
        {
            // First check the length
            var AreOptionsCountValid = CreatedObject.Options.Count != 0;

            if (!AreOptionsCountValid)
                return false;

            var AreOptionsValid = true;

            // Then check if options aren't empty
            for (var i = 0; i < CreatedObject.Options.Count; i++)
            {
                // Trim all the options for checking if they are not identical
                var option = CreatedObject.Options[i].Trim();
                CreatedObject.Options[i] = option;

                // Null or empty string are also not accepted
                if (string.IsNullOrEmpty(option))
                    AreOptionsValid = false;
            }

            if (!AreOptionsValid)
                return false;

            // Check the list if it contains only unique strings
            var AreOptionUnique = !CreatedObject.Options.HasDuplicates();

            if (!AreOptionUnique)
                return false;

            var AreCorrectAnswersCorrect = CreatedObject.CorrectAnswer.Count == CreatedObject.Options.Count;

            if (!AreCorrectAnswersCorrect)
                return false;

            return true;
            
        }

        /// <summary>
        /// Checks if <see cref="CreatedQuestion"/> has options attached to it
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected override bool HasOptions() => CreatedObject.Options != null;

        /// <summary>
        /// Checks if <see cref="CreatedQuestion"/> has correct answer attached to it
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected override bool HasCorrectAnswer() => CreatedObject.CorrectAnswer != null;

        #endregion

        #region Public Construction Methods

        /// <summary>
        /// Adds correct answer to the test
        /// </summary>
        /// <param name="CorrectAnswer"></param>
        public override void AddCorrectAnswer(List<bool> CorrectAnswer)
        {
            // Make sure the index is valid
            if (CorrectAnswer == null || CorrectAnswer.Count == 0)
                return;

            CreatedObject.CorrectAnswer = CorrectAnswer;
            IsCorrectAnswerAttached = true;
        }

        /// <summary>
        /// Adds answer options to the current question
        /// </summary>
        /// <param name="Options">The options in a format like:
        /// Options[0] -> A - some option
        /// Options[1] -> B - another option</param>
        public override void AddOptions(List<string> Options)
        {
            // Make sure data is valid
            if (Options == null || Options.Count == 0)
                return;

            CreatedObject.Options = Options;
            AreOptionsAttached = true;
        }

        /// <summary>
        /// Adds scoring evaluation criteria for this question
        /// </summary>
        /// <param name="Scoring">Specific evaluation criteria</param>
        public override void AddScoring(Scoring Scoring)
        {
            // Make sure data is valid
            if (Scoring == null || Scoring.FullPointScore <= 0)
                return;

            // Attach it to the question
            CreatedObject.Scoring = Scoring;
            IsScoringAttached = true;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleCheckBoxesQuestionBuilder() : base() { }

        /// <summary>
        /// Constructor accepting the model question to work with
        /// Works as an editor in this context
        /// </summary>
        /// <param name="Prototype">The prototype question</param>
        public MultipleCheckBoxesQuestionBuilder(MultipleCheckBoxesQuestion Prototype) : base(Prototype) { }

        #endregion
    }
}

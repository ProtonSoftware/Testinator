using System.Collections.Generic;
using Testinator.Core.QuestionBuilders;

namespace Testinator.Core
{
    /// <summary>
    /// The concrete builder to create <see cref="MultipleChoiceQuestion"/>
    /// </summary>
    public class MultipleChoiceQuestionBuilder : QuestionBuilder<MultipleChoiceQuestion, List<string>, int>
    {
        #region Private Specific Methods

        /// <summary>
        /// Checks if the question's content is valid
        /// </summary>
        /// <returns>True if everything is correct, otherwise false</returns>
        protected override bool IsValid()
        {
            if (CreatedObject.CorrectAnswerIndex >= CreatedObject.Options.Count)
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
            var AreOptionsUnique = !CreatedObject.Options.HasDuplicates();

            if (!AreOptionsUnique)
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
        /// NOTE: always true as CorrectAnswerIndex is 0 by default
        /// </summary>
        /// <returns>True if it has, otherwise false</returns>
        protected override bool HasCorrectAnswer() => true;
        
        #endregion

        #region Public Construction Methods

        /// <summary>
        /// Adds correct answer to the test
        /// </summary>
        /// <param name="CorrectAnswer"></param>
        public override void AddCorrectAnswer(int CorrectAnswer)
        {
            // Make sure the index is valid
            if (CorrectAnswer < 0)
                return;

            // If questions exist check if correct answer is out of range
            if (CreatedObject.Options != null && CorrectAnswer >= CreatedObject.Options.Count)
                return;

            CreatedObject.CorrectAnswerIndex = CorrectAnswer;
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
        /// <param name="Scoring">Specific evaluation criteria. 
        /// NOTE: In this case <see cref="Scoring.Mode"/> is automatically set to <see cref="ScoringMode.FullAnswer"/> 
        /// as nothing else makes sense here </param>
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
        public MultipleChoiceQuestionBuilder() : base() { }

        /// <summary>
        /// Constructor accepting the model question to work with
        /// Works as an editor in this context
        /// </summary>
        /// <param name="Prototype">The prototype question</param>
        public MultipleChoiceQuestionBuilder(MultipleChoiceQuestion Prototype) : base(Prototype) { }

        #endregion

    }
}

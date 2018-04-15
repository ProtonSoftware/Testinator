using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// A multiple choice question, A, B, C...
    /// </summary>
    [Serializable]
    public class MultipleChoiceQuestion: Question
    {
        #region Public Properties

        /// <summary>
        /// The type of this question
        /// </summary>
        public override QuestionType Type => QuestionType.MultipleChoice;

        /// <summary>
        /// Answer list for this question.
        /// ABC format. E.g.:
        /// Options[0] => A - some option
        /// Options[1] => B - another option
        /// </summary>
        public List<string> Options { get; set; }
        
        /// <summary>
        /// The correct answer for this question, which is the index of <see cref="Options"/> list that
        /// contains correct answer for this question
        /// </summary>
        public int CorrectAnswerIndex { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the answer is correct
        /// </summary>
        /// <param name="Answer">The answer to be checked</param>
        /// <returns>Number of points for this answer depending on current valuation criteria</returns>
        public override int CheckAnswer(Answer Answer)
        {
            if (Answer.IsEmpty())
                return 0;

            var TargetAnswer = Answer as MultipleChoiceAnswer;

            // The type should match but need to check it just in case
            if (TargetAnswer == null)
                return 0;

            return TargetAnswer.SelectedAnswerIndex == CorrectAnswerIndex ? Scoring.FullPointScore : 0;
        }

        /// <summary>
        /// Gets point score as a handy string that can be displayed to the user
        /// </summary>
        /// <returns>Handy <see cref="Evaluation"/> string</returns>
        public override string GetDisplayPointScore() => Scoring.FullPointScore.ToString();

        #endregion
    }
}

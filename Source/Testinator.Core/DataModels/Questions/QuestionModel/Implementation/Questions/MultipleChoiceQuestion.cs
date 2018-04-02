using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    #region OLD
    /// <summary>
    /// A multiple choice question, A, B, C...
    /// </summary>
    [Serializable]
    public class MultipleChoiceQuestion : Question
    {
        #region Public Properties

        /// <summary>
        /// Options for the question to chose from. 
        /// A, B, C etc.
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// The task itself
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Gets or sets the correct answer 
        /// WARNING: indexing starts at 1 NOT 0
        /// A is 1, B is 2, etc.
        /// </summary>
        public int CorrectAnswerIndex { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>Return a clone</returns>
        public override Question Clone() => new MultipleChoiceQuestion()
        {
#pragma warning disable IDE0003 // Remove qualification
            Task = this.Task,
            CorrectAnswerIndex = this.CorrectAnswerIndex,
            Options = this.Options,
            PointScore = this.PointScore,
#pragma warning restore IDE0003 // Remove qualification
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleChoiceQuestion()
        {
            // Set the type
            Type = QuestionType.MultipleChoice;
        }

        #endregion
    }

    #endregion

    #region NEW

    /// <summary>
    /// A multiple choice question, A, B, C...
    /// </summary>
    [Serializable]
    public class MultipleChoiceQuestionXXX : QuestionXXX
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
        public override int CheckAnswer(AnswerXXX Answer)
        {
            if (Answer.IsEmpty())
                return 0;

            var TargetAnswer = Answer as MultipleChoiceAnswerXXX;

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

    #endregion
}

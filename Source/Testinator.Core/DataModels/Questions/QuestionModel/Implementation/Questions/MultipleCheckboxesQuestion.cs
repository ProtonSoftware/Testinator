using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Testinator.Core
{
    /// <summary>
    /// A multiple checkboxes question type
    /// </summary>
    [Serializable]
    public class MultipleCheckBoxesQuestion : Question
    {
        #region Public Properties

        /// <summary>
        /// The type of this question
        /// </summary>
        public override QuestionType Type => QuestionType.MultipleCheckboxes;

        /// <summary>
        /// Correct answer for this question in a list format
        /// CorrectAnswer[0] - true  -> should be checked to get a point
        /// CorrectAnswer[1] - false -> shouldn't be checked to get a point
        /// </summary>
        public List<bool> CorrectAnswer { get; set; }

        /// <summary>
        /// The options for this questions that can be checked or not
        /// </summary>
        public List<string> Options { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the answer is correct 
        /// </summary>
        /// <param name="Answer">Answer to validate</param>
        /// <returns>Number of points for this answer depending on current valuation criteria</returns>
        public override int CheckAnswer(Answer Answer)
        {
            if (Answer == null)
                throw new NullReferenceException();

            var TargetAnswer = Answer as MultipleCheckBoxesAnswer;

            // The type should match but need to check it just in case
            if (TargetAnswer == null)
                throw new Exception("Answer type does not match the question type");

            if (TargetAnswer.UserAnswer.Count != CorrectAnswer.Count)
                throw new Exception("Answers count does not match this question");

            if (Answer.IsEmpty())
                return 0;

            var CorrectAnswersNumber = 0;

            // Get how many answers user got right
            for (var i = 0; i < Options.Count; i++)
            {
                if (TargetAnswer.UserAnswer[i] == CorrectAnswer[i])
                    CorrectAnswersNumber++;
            }

            // TODO: DECIDE ON ROUNDING (CEIL OR FLOOR)
            // Based on evaluation mode givem them proper ammount of points 
            switch (Scoring.Mode)
            {
                case ScoringMode.FullAnswer:
                    return CorrectAnswersNumber == CorrectAnswer.Count ? Scoring.FullPointScore : 0;

                case ScoringMode.HalfTheAnswer:

                    // Less than the half: 0 points
                    if (CorrectAnswersNumber < CorrectAnswer.Count / 2)
                        return 0;

                    // If the score is between the half and max: half the points 
                    if (CorrectAnswersNumber < CorrectAnswer.Count)
                        return Scoring.FullPointScore / 2;

                    // Full score: full points
                    return Scoring.FullPointScore;

                case ScoringMode.EvenParts:
                    
                    // For each asnwer give a proper ammount of poitns
                    // NOTE: Can be done by ceil or floor rounding.
                    return Scoring.FullPointScore * CorrectAnswersNumber / CorrectAnswer.Count;

                default:
                    Debugger.Break();
                    throw new Exception("Answer check failed. No evaluating mode detected.");
            }
        }

        /// <summary>
        /// Gets point score as a handy string that can be displayed to the user
        /// </summary>
        /// <returns>Handy <see cref="Evaluation"/> string</returns>
        public override string GetDisplayPointScore()
        {
            switch (Scoring.Mode)
            {
                case ScoringMode.FullAnswer:
                    return $"Fully correct answer: {Scoring.FullPointScore.ToString()}";
                case ScoringMode.HalfTheAnswer:
                    return $"Half the answer: {(Scoring.FullPointScore / 2).ToString()}";
                case ScoringMode.EvenParts:
                    return $"For each part: {(Scoring.FullPointScore / CorrectAnswer.Count).ToString()}";
                default:
                    return Scoring.FullPointScore.ToString();
            }
        }

        #endregion
    }
}

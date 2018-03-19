namespace Testinator.Core
{
    /// <summary>
    /// Helpers for question class
    /// </summary>
    public static class QuestionHelpers
    {
        /// <summary>
        /// Checks if the given asnwer is correct
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer">The answer to check</param>
        /// <returns>True is correct, false otherwise</returns>
        public static bool IsAnswerCorrect(this MultipleChoiceQuestion question, MultipleChoiceAnswer answer) => question.CorrectAnswerIndex == answer.SelectedAnswerIdx;

        /// <summary>
        /// Checks if the given asnwer is correct
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer">The answer to check</param>
        /// <returns>True is correct, false otherwise</returns>
        public static bool IsAnswerCorrect(this SingleTextBoxQuestion question, SingleTextBoxAnswer answer) => question.CorrectAnswer == answer.Answer;

        /// <summary>
        /// Checks if the given asnwer is correct
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer">The answer to check</param>
        /// <returns>True is correct, false otherwise</returns>
        public static bool IsAnswerCorrect(this MultipleCheckboxesQuestion question, MultipleCheckboxesAnswer answer)
        {
            var i = 0;
            foreach (var option in question.OptionsAndAnswers)
            {
                if (option.Value != answer.Answers[i])
                    return false;
                i++;
            }

            return true;
        }
    }


}

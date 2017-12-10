using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// Helps to build a <see cref="Test"/>
    /// </summary>
    public static class TestHelpers
    {
        #region Public Methods

        /// <summary>
        /// Adds question and correct answer to it to the test
        /// </summary>
        /// <param name="question">Question to be added</param>
        /// <param name="answer">Correct answer to it</param>
        public static void AddQuestion(this Test test, Question question, Answer answer)
        {
            // Don't add null items
            if (question == null || answer == null)
                return;

            // Save them
            test.QuestionsAnswers.Add(question, answer);
        }

        /// <summary>
        /// Removes question from the test
        /// </summary>
        /// <param name="question">The question to be removed</param>
        public static void RemoveQuestion(this Test test, Question question)
        {
            if (question == null)
                return;

            // Remove item if it exists
            if(test.QuestionsAnswers.ContainsKey(question))
            {
                test.QuestionsAnswers.Remove(question);
            }
        }

        /// <summary>
        /// Removes all questions and answers from this test
        /// </summary>
        public static void ClearQuestions(this Test test)
        {
            test.QuestionsAnswers.Clear();
        }
        #endregion

    }
}

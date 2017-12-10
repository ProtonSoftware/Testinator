using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// Helps to build a <see cref="Test"/>
    /// </summary>
    public class TestBuilder : Test
    {
        #region Public Methods

        /// <summary>
        /// Adds question and correct answer to it to the test
        /// </summary>
        /// <param name="question">Question to be added</param>
        /// <param name="answer">Correct answer to it</param>
        public void AddQuestion(Question question, Answer answer)
        {
            // Don't add null items
            if (question == null || answer == null)
                return;

            // Save them
            QuestionsAnswers.Add(question, answer);
        }

        /// <summary>
        /// Removes question from the test
        /// </summary>
        /// <param name="question">The question to be removed</param>
        public void RemoveQuestion(Question question)
        {
            if (question == null)
                return;

            // Remove item if it exists
            if(QuestionsAnswers.ContainsKey(question))
            {
                QuestionsAnswers.Remove(question);
            }
        }

        /// <summary>
        /// Removes all questions and answers from this test
        /// </summary>
        public void ClearQuestions()
        {
            QuestionsAnswers.Clear();
        }

        /// <summary>
        /// Gets test builder from the given test
        /// </summary>
        /// <param name="test">The test this builder will be based on</param>
        /// <returns>The builder</returns>
        public static TestBuilder GetBuilderFrom(Test test)
        {
            return test as TestBuilder;
        }

        /// <summary>
        /// Gets <see cref="Test"/> from this builder
        /// </summary>
        /// <returns>A simpler test object</returns>
        public Test GetTest()
        {
            return this as Test;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestBuilder()
        {
        }

        #endregion

    }
}

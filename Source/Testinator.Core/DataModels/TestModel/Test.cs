using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The model of a test
    /// </summary>
    public class Test
    {
        #region Private Members
        
        /// <summary>
        /// Provides indexes for the question
        /// </summary>
        private int mIdProvider = 0;

        /// <summary>
        /// The list of all questions in the test
        /// </summary>
        private List<Question> mQuestions = new List<Question>();

        /// <summary>
        /// The list of all answers in the test
        /// </summary>
        private List<Answer> mAnswers = new List<Answer>();

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of this test
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// How much time the test is going to take
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// How much questions in the test
        /// </summary>
        public int QuestionCount => Questions.Count;

        /// <summary>
        /// Indicates which question is displayed now
        /// </summary>
        public int QuestionCounter { get; set; } = 1;

        /// <summary>
        /// The list of all questions in the test
        /// </summary>
        public List<Question> Questions
        {
            get => mQuestions;
            set
            {
                if (value == null)
                    return;

                // Reset the indexer 
                mIdProvider = 0;
                for (int i = 0; i < value.Count; i++, mIdProvider++)
                    value[i].ID = mIdProvider;

                mQuestions = value;
            }
        }
        // TODO: question cannot be null, exception here ^^^^

        /// <summary>
        /// The list of all answers in the test
        /// </summary>
        public List<Answer> Answers
        {
            get => mAnswers;
            set
            {
                if (value == null)
                    return;

                // Reset the indexer 
                mIdProvider = 0;
                for (int i = 0; i < value.Count; i++, mIdProvider++)
                    value[i].ID = mIdProvider;

                mAnswers = value;
            }
        }
        // TODO: answer cannot be null, exception here ^^^^

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Test()
        { }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Adds a question to this test
        /// </summary>
        /// <param name="question">The question being added</param>
        public void AddQuestion(Question question)
        {
            if (mQuestions == null || question == null)
                return;
            question.ID = mIdProvider;
            mIdProvider++;
            mQuestions.Add(question);
        }

        /// <summary>
        /// Adds an answer to this test
        /// </summary>
        /// <param name="question">The answer being added</param>
        public void AddAnswer(Answer answer)
        {
            if (mAnswers == null || answer == null)
                return;
            answer.ID = mIdProvider;
            mIdProvider++;
            mAnswers.Add(answer);
        }

        /// <summary>
        /// Gets next question
        /// </summary>
        public Question GetNextQuestion()
        {
            // We try to get next question, so lets move the counter
            QuestionCounter++;

            // Check if last question was the last one
            if (QuestionCounter == Questions.Count)
            {
                // Then end the test and don't show any question
                EndTest();
                return null;
            }

            // Return next question
            return Questions[QuestionCounter - 1];
        }

        /// <summary>
        /// Ends the test
        /// </summary>
        public void EndTest()
        {
            return;
        }

        #endregion
    }
}

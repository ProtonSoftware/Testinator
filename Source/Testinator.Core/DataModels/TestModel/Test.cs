using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Model of a test
    /// </summary>
    public class Test
    {
        #region Private Members
        
        /// <summary>
        /// Provides indexes for the question
        /// </summary>
        private int IdProvider = 0;

        /// <summary>
        /// The list of all questions in the test
        /// </summary>
        private List<Question> mQuestions = new List<Question>();

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
                IdProvider = 0;
                for (int i = 0; i < value.Count; i++, IdProvider++)
                    value[i].ID = IdProvider;

                mQuestions = value;
            }
        }
        // TODO: question cannot be null, exception here ^^^^

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a question to this test
        /// </summary>
        /// <param name="question">The question to be added</param>
        public void AddQuestion(Question question)
        {
            if (mQuestions == null || question == null)
                return;
            question.ID = IdProvider;
            IdProvider++;
            mQuestions.Add(question);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Test()
        { }

        #endregion
    }
}

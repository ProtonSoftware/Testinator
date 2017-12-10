using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The model of a test contaning only essential properties and functions
    /// </summary>
    [Serializable]
    public class Test
    {
        #region Protected Members

        /*
        /// <summary>
        /// Provides indexes for questions
        /// </summary>
        private int mIdProvider = 0;

        /// <summary>
        /// The list of all questions in the test
        /// </summary>
        private List<Question> mQuestions = new List<Question>();

        /// <summary>
        /// The list of all correct answers in the test
        /// </summary>
        private List<Answer> mAnswers = new List<Answer>();
        */

        /// <summary>
        /// Stores all questions and correct answers for them in this test
        /// </summary>
        protected Dictionary<Question,Answer> QuestionsAnswers = new Dictionary<Question, Answer>();

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

        /*
        /// <summary>
        /// Number of questions in this test
        /// </summary>
        public int QuestionCount => Questions.Count;

        
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
       */

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
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
        #region Public Properties

        /// <summary>
        /// Stores all questions and correct answers for them in this test
        /// </summary>
        public Dictionary<Question,Answer> QuestionsAnswers { get; } = new Dictionary<Question, Answer>();

        /// <summary>
        /// The name of this test
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// How much time the test is going to take
        /// </summary>
        public TimeSpan Duration { get; set; }

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
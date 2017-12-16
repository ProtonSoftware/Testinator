using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The model of a test contaning only essential properties and functions
    /// </summary>
    [Serializable]
    public class Test : PackageContent
    {
        #region Public Properties
        
        /// <summary>
        /// Stores all questions and correct answers for them in this test
        /// </summary>
        public List<Question> Questions { get; } = new List<Question>();

        /// <summary>
        /// The name of this test
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// How much time the test is going to take
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The grading system for this test
        /// </summary>
        public Grading Grading { get; set; } = new Grading();
        
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
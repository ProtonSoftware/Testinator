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
        /// Unique ID of this test used to recognise
        /// </summary>
        public int ID { get; set; } = -1;

        /// <summary>
        /// Stores all questions and correct answers for them in this test
        /// </summary>
        public List<Question> Questions { get; } = new List<Question>();

        /// <summary>
        /// The name of this test
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// How much time the test is going to take
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets a sum of point scores in every question
        /// </summary>
        public int TotalPointScore
        {
            get
            {
                // Keep track of point score of every question
                var finalPointScore = 0;

                // Loop each question and add up it's pointscore
                foreach (var question in Questions) finalPointScore += question.PointScore;

                // Return collected value
                return finalPointScore;
            }
        }

        /// <summary>
        /// The grading system for this test
        /// </summary>
        public GradingPoints Grading { get; set; }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Test()
        {

        }

        #endregion
    }
}
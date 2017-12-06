using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Model of a test
    /// </summary>
    public class Test
    {
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
        public List<Question> Questions { get; set; } = new List<Question>();

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Test()
        { }

        public Test(string name, TimeSpan duration)
        {
            Name = name;
            Duration = duration;
        }

        #endregion
    }
}

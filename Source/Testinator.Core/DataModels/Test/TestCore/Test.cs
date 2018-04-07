using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The model of a test
    /// </summary>
    [Serializable]
    public class Test : PackageContent
    {
        #region Public Properties

        /// <summary>
        /// Information about this test
        /// </summary>
        public TestInformation Info { get; set; }

        /// <summary>
        /// All questions attached to this test
        /// </summary>
        public List<Question> Questions { get; set; }

        /// <summary>
        /// Points grading for this test
        /// </summary>
        public GradingPoints Grading { get; set; }

        /// <summary>
        /// The maxiumum ammout of points the user can get from this test
        /// </summary>
        public int TotalPointScore { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Defaults constructor
        /// </summary>
        public Test()
        {
            Questions = new List<Question>();
            Info = new TestInformation();
        }

        #endregion
    }
}
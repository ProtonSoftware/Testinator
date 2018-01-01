using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Test results
    /// </summary>
    [Serializable] 
    public class TestResults
    {

        #region Public Properties

        /// <summary>
        /// The test the users took
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// The date the test was hold
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The results for each user
        /// </summary>
        public Dictionary<ClientModelSerializable, List<Answer>> Results { get; set; } = new Dictionary<ClientModelSerializable, List<Answer>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResults()
        {

        }

        #endregion
    }
}

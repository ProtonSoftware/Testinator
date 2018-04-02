using System;
using System.Collections.Generic;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// Test results
    /// </summary>
    [Serializable] 
    public class ServerTestResults
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
        /// Client matched with answers they gave
        /// </summary>
        public Dictionary<TestResultsClientModel, List<Answer>> ClientAnswers { get; set; } = new Dictionary<TestResultsClientModel, List<Answer>>();

        /// <summary>
        /// A shortcut to clients list
        /// </summary>
        public List<TestResultsClientModel> Clients => ClientAnswers.Keys.ToList();

        #endregion
    }
}

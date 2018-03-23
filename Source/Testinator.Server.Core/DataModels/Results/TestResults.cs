using System;
using System.Collections.Generic;
using System.Linq;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Test results
    /// </summary>
    [Serializable] 
    public class ServerTestResults : ServerTestResultsBase
    {
        #region Public Properties

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

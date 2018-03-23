using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Client.Core
{ 
    /// <summary>
    /// Client side test results to be saved on the client's computer disk
    /// </summary>
    [Serializable]
    public class ClientTestResults : ClientTestResultsBase
    {
        /// <summary>
        /// The mark user has got
        /// </summary>
        public Mark Mark { get; set; }

        /// <summary>
        /// User's answers
        /// </summary>
        public List<Answer> Answers { get; set; }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientTestResults()
        {

        }

        #endregion
    }
}

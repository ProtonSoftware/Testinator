using System;
using System.Collections.Generic;

namespace Testinator.Core
{ 
    /// <summary>
    /// Client side test results to be saved on the client's computer disk
    /// </summary>
    [Serializable]
    public class ClientTestResults
    {
        /// <summary>
        /// The test the user took
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// The model of the client that took the test
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// The date the test was hold
        /// </summary>
        public DateTime Date { get; set; }

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

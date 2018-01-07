using System;
using System.Collections.Generic;

namespace Testinator.Core
{ 
    /// <summary>
    /// Client side test results to be saved on the client's disk
    /// </summary>
    [Serializable]
    public class ClientTestResults
    {
        /// <summary>
        /// The test user has taken
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// The model of the client that took the test
        /// </summary>
        public ClientModelSerializable ClientModel { get; set; }

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

using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// A shortcut to clients list
        /// </summary>
        public List<ClientModelSerializable> Clients => Results.Keys.ToList();

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

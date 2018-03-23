
using System;

namespace Testinator.Core
{
    /// <summary>
    /// A base class for both client and server side results
    /// </summary>
    [Serializable]
    public abstract class ResultsBase
    {
        /// <summary>
        /// The test the users took
        /// </summary>
        public Test Test { get; set; }
    }
}

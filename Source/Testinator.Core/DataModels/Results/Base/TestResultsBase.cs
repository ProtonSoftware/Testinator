using System;

namespace Testinator.Core
{
    /// <summary>
    /// The foundation for server-side test results
    /// </summary>
    [Serializable]
    public abstract class ServerTestResultsBase : ResultsBase
    {
        /// <summary>
        /// The date the test was hold
        /// </summary>
        public DateTime Date { get; set; }
    }
}

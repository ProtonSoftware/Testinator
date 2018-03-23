using System;

namespace Testinator.Core
{
    /// <summary>
    /// The foundation for client-side test results
    /// </summary>
    [Serializable]
    public abstract class ClientTestResultsBase : ResultsBase
    {
        /// <summary>
        /// The model of the client that took the test
        /// </summary>
        public Client Client { get; set; }
    }
}

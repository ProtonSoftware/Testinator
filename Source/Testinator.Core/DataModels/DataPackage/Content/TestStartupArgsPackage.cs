using System;

namespace Testinator.Core
{
    /// <summary>
    /// The package contaning the test startup args
    /// </summary>
    [Serializable]
    public class TestStartupArgs : PackageContent
    {
        #region Public Properties

        /// <summary>
        /// Current session identifier 
        /// </summary>
        public Guid SessionIdentifier { get; set; }

        /// <summary>
        /// Indicates it the result page is allowed to see by the user
        /// </summary>
        public bool IsResultsPageAllowed { get; set; }
        
        /// <summary>
        /// Indicates if the application should be in full screen mode during the test
        /// </summary>
        public bool FullScreenMode { get; set; }

        /// <summary>
        /// The time offset this test should start
        /// </summary>
        public TimeSpan TimerOffset { get; set; }

        #endregion
    }
}
using System;

namespace Testinator.Core
{
    /// <summary>
    /// The package contaning the test startup args
    /// </summary>
    [Serializable]
    public class TestStartupArgsPackage : PackageContent
    {
        #region Public Properties

        /// <summary>
        /// Indicates it the result page is allowed to see by the user
        /// </summary>
        public bool IsResultsPageAllowed { get; set; }
        
        /// <summary>
        /// Indicates if the application should be in full screen mode during the test
        /// </summary>
        public bool FullScreenMode { get; set; }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        public TestStartupArgsPackage()
        {

        }

        #endregion
    }
}
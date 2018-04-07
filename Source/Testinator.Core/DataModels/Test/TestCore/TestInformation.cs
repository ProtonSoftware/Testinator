using System;

namespace Testinator.Core
{
    /// <summary>
    /// Information about a test as a class
    /// </summary>
    [Serializable]
    public class TestInformation
    {
        #region Public Properties

        /// <summary>
        /// The name of this test 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The duration of this test
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The note that was saved with this test
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Tags associated with this test
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Soft version this test was last edited on
        /// </summary>
        public Version SoftwareVersion { get; set; }

        #endregion
    }
}

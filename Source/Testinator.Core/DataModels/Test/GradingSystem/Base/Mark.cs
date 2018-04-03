using System;

namespace Testinator.Core
{
    /// <summary>
    /// Represents a single mark
    /// </summary>
    [Serializable]
    public class Mark
    {
        #region Public Properties

        /// <summary>
        /// The top limit for this mark
        /// </summary>
        public int TopLimit { get; set; }

        /// <summary>
        /// The bottom limit for this mark
        /// </summary>
        public int BottomLimit { get; set; }

        #endregion
    }
}

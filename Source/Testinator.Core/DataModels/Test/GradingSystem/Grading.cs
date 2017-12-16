using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Grading system
    /// </summary>
    [Serializable]
    public class Grading
    {
        #region Public Properties

        /// <summary>
        /// The list of marks
        /// </summary>
        public List<Mark> Marks { get; } = new List<Mark>();

        #endregion
    }
}

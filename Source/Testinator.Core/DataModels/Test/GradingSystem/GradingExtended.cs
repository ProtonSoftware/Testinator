﻿namespace Testinator.Core
{
    /// <summary>
    /// Extended version of <see cref="Grading"/> class
    /// </summary>
    public class GradingExtended : Grading
    {
        #region Public Properties 

        /// <summary>
        /// Name of this grading set
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates if this criteria contains mark A
        /// </summary>
        public bool IsMarkAIncluded { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public GradingExtended()
        {

        }

        #endregion
    }
}
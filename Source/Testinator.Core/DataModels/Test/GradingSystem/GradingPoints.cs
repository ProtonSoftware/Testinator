using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Grading (criteria) system which border values are in points
    /// This grading can be attached to the test
    /// </summary>
    [Serializable]
    public class GradingPoints
    {
        #region Public Properties

        /// <summary>
        /// Indicates if <see cref="Marks.A"/> is included in this set
        /// </summary>
        public bool IsMarkAInluded { get; set; } 

        public Mark MarkA { get; set; }

        public Mark MarkB { get; set; }

        public Mark MarkC { get; set; }

        public Mark MarkD { get; set; }

        public Mark MarkE { get; set; }

        public Mark MarkF { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a mark based on a score 
        /// </summary>
        /// <param name="Grades"></param>
        /// <param name="points">Points the user scored in the test</param>
        /// <returns>The corresponding mark</returns>
        public Marks GetMark(int points)
        {
            // Logike tutaj proszę :)
            return Marks.A;
        }

        #endregion
    }
}

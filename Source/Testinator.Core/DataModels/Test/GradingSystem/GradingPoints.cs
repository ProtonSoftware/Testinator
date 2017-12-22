using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Grading (criteria) system which border values are in points
    /// This grading can be attached to the test
    /// </summary>
    [Serializable]
    public class GradingPoints : GradingBase
    {
        #region Public Methods

        /// <summary>
        /// Gets a mark based on a score 
        /// </summary>
        /// <param name="Grades"></param>
        /// <param name="points">Points the user scored in the test</param>
        /// <returns>The corresponding mark</returns>
        public Marks GetMark(int points)
        {
            if (IsMarkAIncluded)
                if (points >= MarkA.BottomLimit)
                    return Marks.A;

            if (points >= MarkB.BottomLimit)
                return Marks.B;

            if (points >= MarkC.BottomLimit)
                return Marks.C;

            if (points >= MarkD.BottomLimit)
                return Marks.D;

            if (points >= MarkE.BottomLimit)
                return Marks.E;

            if (points >= MarkF.BottomLimit)
                return Marks.F;

            throw new Exception("No grading available for this score!");
        }

        #endregion
    }
}

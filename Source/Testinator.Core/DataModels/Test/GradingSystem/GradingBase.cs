using System;
using System.Collections.Generic;

namespace Testinator.Core
{

    /// <summary>
    /// Base class for any grading to inherit from
    /// </summary>
    [Serializable]
    public abstract class GradingBase
    {
        #region Public Properties

        /// <summary>
        /// Indicates if <see cref="Marks.A"/> is included in this set
        /// </summary>
        public bool IsMarkAIncluded { get; set; } 

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

        /// <summary>
        /// Updates values of a mark
        /// </summary>
        /// <param name="mark">The mark which values we are updating</param>
        /// <param name="top">The top limit in points</param>
        /// <param name="bottom">The bottom limit in points</param>
        public void UpdateMark(Marks mark, int top, int bottom)
        {
            // Based on mark...
            switch (mark)
            {
                // Update top and bottom values
                case Marks.A:
                    {
                        MarkA.TopLimit = top;
                        MarkA.BottomLimit = bottom;
                        IsMarkAIncluded = true;
                    }
                    break;
                case Marks.B:
                    {
                        MarkB.TopLimit = top;
                        MarkB.BottomLimit = bottom;
                    }
                    break;
                case Marks.C:
                    {
                        MarkC.TopLimit = top;
                        MarkC.BottomLimit = bottom;
                    }
                    break;
                case Marks.D:
                    {
                        MarkD.TopLimit = top;
                        MarkD.BottomLimit = bottom;
                    }
                    break;
                case Marks.E:
                    {
                        MarkE.TopLimit = top;
                        MarkE.BottomLimit = bottom;
                    }
                    break;
                case Marks.F:
                    {
                        MarkF.TopLimit = top;
                        MarkF.BottomLimit = bottom;
                    }
                    break;
            }
        }

        #endregion
    }
}

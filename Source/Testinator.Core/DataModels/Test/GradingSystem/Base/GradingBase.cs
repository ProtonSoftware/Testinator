using System;

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
        /// Indicates if the highest mark (<see cref="Marks.A"/>) is included in this set
        /// </summary>
        public bool IsMarkAIncluded { get; set; }

        // All marks in this grading

        public Mark MarkA { get; protected set; } = new Mark();
                               
        public Mark MarkB { get; protected set; } = new Mark();
                               
        public Mark MarkC { get; protected set; } = new Mark();
                               
        public Mark MarkD { get; protected set; } = new Mark();
                               
        public Mark MarkE { get; protected set; } = new Mark();
                               
        public Mark MarkF { get; protected set; } = new Mark();

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates values of a mark
        /// </summary>
        /// <param name="mark">The mark which values we are updating</param>
        /// <param name="top">The top limit in points</param>
        /// <param name="bottom">The bottom limit in points</param>
        public virtual void UpdateMark(Marks mark, int top, int bottom)
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

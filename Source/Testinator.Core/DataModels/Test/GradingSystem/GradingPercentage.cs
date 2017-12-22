using System;

namespace Testinator.Core
{
    /// <summary>
    /// Grading (criteria) system which border values are in percents
    /// This class should be converted to <see cref="GradingPoints"/> and then attached to <see cref="Test"/>
    /// </summary>
    public class GradingPercentage : GradingBase
    {
        #region Public Properties 

        /// <summary>
        /// Name of this grading set
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public GradingPercentage()
        {
            // Set default values
            IsMarkAIncluded = true;
            UpdateMark(Marks.A, 100, 96);
            UpdateMark(Marks.B, 95, 86);
            UpdateMark(Marks.C, 85, 70);
            UpdateMark(Marks.D, 69, 50);
            UpdateMark(Marks.E, 49, 30);
            UpdateMark(Marks.F, 29, 0);
        }

        #endregion

        #region Public Methods

        public GradingPoints ToPoints(int maxPoints)
        {
            var result = new GradingPoints();

            int top = 0;
            int bottom = 0;

            if (IsMarkAIncluded)
            {
                result.IsMarkAIncluded = true;
                top = PercentToPoint(MarkA.TopLimit, maxPoints);
                bottom = PercentToPoint(MarkA.BottomLimit, maxPoints);
                result.UpdateMark(Marks.A, top, bottom);
            }
            else
            {
                result.IsMarkAIncluded = false;
            }

            top = bottom - 1;
            bottom = PercentToPoint(MarkB.BottomLimit, maxPoints);
            result.UpdateMark(Marks.B, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkC.BottomLimit, maxPoints);
            result.UpdateMark(Marks.C, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkD.BottomLimit, maxPoints);
            result.UpdateMark(Marks.D, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkE.BottomLimit, maxPoints);
            result.UpdateMark(Marks.E, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkF.BottomLimit, maxPoints);
            result.UpdateMark(Marks.F, top, bottom);

            return result;
        }

        #endregion

        #region Private Helpers

        private static int PercentToPoint(int percent, int maxPoint)
        {
            return (int)Math.Round((percent / (double)100) * maxPoint, MidpointRounding.AwayFromZero);
        }

        #endregion
    }
}

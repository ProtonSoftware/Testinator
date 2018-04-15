using System;

namespace Testinator.Core
{
    /// <summary>
    /// Grading (criteria) system whose border values are in percents
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
            Name = "NewGrading";
            UpdateMark(Marks.A, 100, 96);
            UpdateMark(Marks.B, 95, 86);
            UpdateMark(Marks.C, 85, 70);
            UpdateMark(Marks.D, 69, 50);
            UpdateMark(Marks.E, 49, 30);
            UpdateMark(Marks.F, 29, 0);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Cnoverts percentage values to points
        /// </summary>
        /// <param name="maxPoints">Maximum possible score</param>
        /// <returns>New points grading</returns>
        public GradingPoints ToPoints(int maxPoints)
        {
            var result = new GradingPoints();

            var top = 0;
            var bottom = 0;

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

            top = PercentToPoint(MarkB.TopLimit, maxPoints);
            bottom = PercentToPoint(MarkB.BottomLimit, maxPoints);
            if (top == result.MarkA.BottomLimit) top--;
            while (bottom > top) bottom--;
            result.UpdateMark(Marks.B, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkC.BottomLimit, maxPoints);
            while (bottom > top) bottom--;
            result.UpdateMark(Marks.C, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkD.BottomLimit, maxPoints);
            while (bottom > top) bottom--;
            result.UpdateMark(Marks.D, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkE.BottomLimit, maxPoints);
            while (bottom > top) bottom--;
            result.UpdateMark(Marks.E, top, bottom);

            top = bottom - 1;
            bottom = PercentToPoint(MarkF.BottomLimit, maxPoints);
            while (bottom > top) bottom--;
            result.UpdateMark(Marks.F, top, bottom);

            return result;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Converts the percentage value to point value
        /// </summary>
        private int PercentToPoint(int percent, int maxPoint) 
            => (int)Math.Round((percent / (double)100) * maxPoint, MidpointRounding.AwayFromZero);

        #endregion
    }
}

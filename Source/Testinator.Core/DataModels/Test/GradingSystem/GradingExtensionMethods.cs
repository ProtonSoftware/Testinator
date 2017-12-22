using System;

namespace Testinator.Core
{
    /// <summary>
    /// Extension methods for Grading system
    /// </summary>
    public static class GradingExtensionMethods
    {
        #region Public Methods

        /// <summary>
        /// Converts values in points to percentage values
        /// </summary>
        /// <returns>New grading with value in percentage</returns>
        public static GradingPercentage ConvertToPercentage(this GradingPoints Grades)
        {
            var result = new GradingPercentage();

            int bottom = 0;
            int top = 0;
            int maxPoints = 0;

            if (Grades.IsMarkAIncluded)
            {
                maxPoints = Grades.MarkA.TopLimit;
                top = PointsToPercent(Grades.MarkA.TopLimit, maxPoints);
                bottom = PointsToPercent(Grades.MarkA.BottomLimit, maxPoints);
                result.UpdateMark(Marks.A, top, bottom);
                result.IsMarkAIncluded = true;
            }
            else
            {
                result.IsMarkAIncluded = false;
                maxPoints = Grades.MarkB.TopLimit;
                top = PointsToPercent(Grades.MarkB.TopLimit, maxPoints);
                bottom = PointsToPercent(Grades.MarkB.BottomLimit, maxPoints);
                result.UpdateMark(Marks.B, top, bottom);
            }

            top = bottom - 1;
            bottom = PointsToPercent(Grades.MarkC.BottomLimit, maxPoints);
            result.UpdateMark(Marks.C, top, bottom);


            top = bottom - 1;
            bottom = PointsToPercent(Grades.MarkD.BottomLimit, maxPoints);
            result.UpdateMark(Marks.D, top, bottom);

            top = bottom - 1;
            bottom = PointsToPercent(Grades.MarkE.BottomLimit, maxPoints);
            result.UpdateMark(Marks.E, top, bottom);

            top = bottom - 1;
            bottom = PointsToPercent(Grades.MarkF.BottomLimit, maxPoints);
            result.UpdateMark(Marks.F, top, bottom);

            return result;
        }

        #endregion

        #region Private Helpers

        private static int PointsToPercent(int points, int maxPoints)
        {
            return (int)((points / (double)maxPoints) * 100);
        }

        #endregion
    }
}

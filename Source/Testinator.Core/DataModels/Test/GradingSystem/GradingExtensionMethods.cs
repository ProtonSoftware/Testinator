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
        /// Converts percentage values in grading to point values 
        /// </summary>
        /// <param name="maxPoints">Max points available</param>
        /// <returns>New Grades with scale in points</returns>
        public static GradingPoints ConvertToPoints(this GradingPercentage Grades, int maxPoints)
        {
            var result = new GradingPoints();

            int bottom = 0;
            int top = 0;

            bool FirstItem = true;

            // Przerob logike tutaj :)
            foreach (var mark in Grades.Marks)
            {
                if (FirstItem)
                {
                    top = PercentToPoint(mark.TopLimit, maxPoints);
                    bottom = PercentToPoint(mark.BottomLimit, maxPoints);
                    FirstItem = false;
                }
                else
                {
                    top = bottom - 1;
                    bottom = PercentToPoint(mark.BottomLimit, maxPoints);
                }

                result.AddMark(mark.Value, top, bottom);
            }

            return result;
        }

        /// <summary>
        /// Converts values in points to percentage values
        /// </summary>
        /// <returns>New grading with value in percentage</returns>
        public static GradingPercentage ConvertToPercentage(this GradingPoints Grades)
        {
            var result = new GradingPercentage();

            bool FirstItem = true;

            int bottom = 0;
            int top = 0;
            int maxPoints = 0;

            // Logika tutaj przerob
            foreach (var mark in Grades.Marks)
            {
                if (FirstItem)
                {
                    maxPoints = mark.TopLimit;

                    top = PointsToPercent(mark.TopLimit, maxPoints);
                    bottom = PointsToPercent(mark.BottomLimit, maxPoints);
                    FirstItem = false;
                }
                else
                {
                    top = bottom - 1;
                    bottom = PointsToPercent(mark.BottomLimit, maxPoints);
                }

                result.AddMark(mark.Value, top, bottom);

            }

            return result;
        }

        #endregion

        #region Private Helpers

        private static int PointsToPercent(int points, int maxPoints)
        {
            return (int)((points / (double)maxPoints) * 100);
        }

        private static int PercentToPoint(int percent, int maxPoint)
        {
            return (int)Math.Round((percent / (double)100) * maxPoint, MidpointRounding.AwayFromZero);
        }

        #endregion
    }
}

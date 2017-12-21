using System;

namespace Testinator.Core
{
    /// <summary>
    /// Extension methods for <see cref="Grading"/> class
    /// </summary>
    public static class GradingExtensionMethods
    {
        #region Public Methods

        /// <summary>
        /// Adds a single grade condition to the scale
        /// </summary>
        /// <param name="Grades"></param>
        /// <param name="mark">The mark to be added</param>
        /// <param name="top">The top limit in points</param>
        /// <param name="bottom">The bottom limit in points</param>
        public static void AddMark(this Grading Grades, Marks mark, int top, int bottom)
        {
            Grades.Marks.Add(new Mark()
            {
                BottomLimit = bottom,
                TopLimit = top,
                Value = mark,
            });
        }

        /// <summary>
        /// Gets a mark based on a score 
        /// </summary>
        /// <param name="Grades"></param>
        /// <param name="points">Points the user scored in the test</param>
        /// <returns>The corresponding mark</returns>
        public static Marks GetMark(this Grading Grades, int points)
        {
            foreach(var mark in Grades.Marks)
            {
                if (points >= mark.BottomLimit)
                    return mark.Value;
            }
            throw new Exception("Wrong value");
        }

        /// <summary>
        /// Converts percentage values in grading to point values 
        /// </summary>
        /// <param name="Grades"></param>
        /// <param name="maxPoints">Max points available</param>
        /// <returns>New Grades with scale in points</returns>
        public static Grading GetPoints(this Grading Grades, int maxPoints)
        {
            var result = new Grading();

            int bottom = 0;
            int top = 0;

            bool FirstItem = true;

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
        /// <param name="Grades"></param>
        /// <returns>New grading with value in percentage</returns>
        public static Grading GetPercentage(this Grading Grades)
        {
            var result = new Grading();

            bool FirstItem = true;

            int bottom = 0;
            int top = 0;
            int maxPoints = 0;

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

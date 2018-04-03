using System;

namespace Testinator.Core
{
    /// <summary>
    /// Extension methods for Grading system
    /// Placed here to reduce the size of a class itself to make trasmission faster and reduce file sizes when writing to file
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

            var bottom = 0;
            var top = 0;
            var maxPoints = 0;

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
            }

            top = PointsToPercent(Grades.MarkB.TopLimit, maxPoints);
            bottom = PointsToPercent(Grades.MarkB.BottomLimit, maxPoints);
            result.UpdateMark(Marks.B, top, bottom);

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
        
        /// <summary>
        /// Gets a mark based on a score 
        /// </summary>
        /// <param name="points">Points the user scored in the test</param>
        /// <returns>The corresponding mark</returns>
        public static Marks GetMark(this GradingPoints Grading, int points)
        {
            if (Grading.IsMarkAIncluded)
                if (points >= Grading.MarkA.BottomLimit)
                    return Marks.A;

            if (points >= Grading.MarkB.BottomLimit)
                return Marks.B;

            if (points >= Grading.MarkC.BottomLimit)
                return Marks.C;

            if (points >= Grading.MarkD.BottomLimit)
                return Marks.D;

            if (points >= Grading.MarkE.BottomLimit)
                return Marks.E;

            if (points >= Grading.MarkF.BottomLimit)
                return Marks.F;

            throw new Exception("No grading available for this score!");
        }

        /// <summary>
        /// Get the max possible score from a grading
        /// </summary>
        /// <param name="Grading">The grading to check maximum score</param>
        /// <returns></returns>
        public static int GetMaxScore(this GradingPoints Grading)
        {
            if (Grading.IsMarkAIncluded)
                return Grading.MarkA.TopLimit;

            return Grading.MarkB.TopLimit;
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

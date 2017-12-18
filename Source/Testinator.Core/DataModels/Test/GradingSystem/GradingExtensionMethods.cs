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

        #endregion
    }
}

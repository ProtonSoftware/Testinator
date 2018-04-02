using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Helpers for Enumerable classes
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Checks if the list contains any duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subjects"></param>
        /// <returns>False if all items in the list are unique, otherwise true</returns>
        public static bool HasDuplicates<T>(this IEnumerable<T> subjects)
        {
            return HasDuplicates(subjects, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Checks if the list contains any duplicates using comparer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subjects"></param>
        /// <param name="comparer"></param>
        /// <returns>False if all items in the list are unique, otherwise true</returns>
        public static bool HasDuplicates<T>(this IEnumerable<T> subjects, IEqualityComparer<T> comparer)
        {
            if (subjects == null)
                throw new ArgumentNullException("subjects");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            var set = new HashSet<T>(comparer);

            foreach (var s in subjects)
                if (!set.Add(s))
                    return true;

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// Helpers for enums
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Gets all values that are contained in an enum
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <returns>Values in a list</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}

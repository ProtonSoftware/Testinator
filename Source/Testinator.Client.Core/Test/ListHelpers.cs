using System;
using System.Collections.Generic;
using System.Linq;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Provides extension methods for <see cref="List{T}"/>
    /// </summary>
    public static class ListHelpers
    {
        /// <summary>
        /// Random numbers generator
        /// </summary>
        private static Random Rng = new Random();

        /// <summary>
        /// Shuffles a list putting objects in a random order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list to shuffle</param>
        /// <returns>The order in which the itmes are now assigned</returns>
        public static IList<int> Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            var ItemsOrder = Enumerable.Range(0, n).ToList();
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);

                var value = list[k];
                list[k] = list[n];
                list[n] = value;

                var swapValue = ItemsOrder[k];
                ItemsOrder[k] = ItemsOrder[n];
                ItemsOrder[n] = swapValue;
            }

            return ItemsOrder;
        }
    }
}

using System;
using System.Collections.Generic;

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
        private static Random rng = new Random();

        /// <summary>
        /// Shuffles a list putting objects in a random order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Determine the least common multiple of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The least common multiple of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ulong LeastCommonMultiple(ulong a, ulong b)
        {
            if (a == 0 && b == 0) { return 0L; }
            return (a * b) / GreatestCommonDivisor(a, b);
        }

        /// <summary>
        /// Determine the least common multiple of an array of numbers.
        /// </summary>
        /// <param name="numbers">An array of numbers.</param>
        /// <returns>The least common multiple of the array of numbers provided.</returns>
        public static ulong LeastCommonMultiple(ulong[] numbers)
        {
            if (numbers.Length == 0
                || numbers.Count(n => n == 0L) == numbers.Length)
            {
                return 0L;
            }

            ulong result = 1L;
            IDictionary<ulong, int> dictionary = new SortedDictionary<ulong, int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                var primes = GetPrimeFactorsDictionary(numbers[i]);

                foreach (var kvp in primes)
                {
                    if (dictionary.ContainsKey(kvp.Key))
                    {
                        if (kvp.Value > dictionary[kvp.Key])
                        {
                            dictionary[kvp.Key] = kvp.Value;
                        }
                    }
                    else
                    {
                        dictionary.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            foreach (var kvp in dictionary)
            {
                result *= Convert.ToUInt64(Math.Pow(kvp.Key, kvp.Value));
            }

            return result;
        }
    }
}

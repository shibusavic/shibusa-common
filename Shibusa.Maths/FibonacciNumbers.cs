using System.Collections.Generic;

namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Get all of the Fibonacci numbers up to a certain value.
        /// </summary>
        /// <param name="maxValue">The max value to return.</param>
        /// <returns>A collection of the Fibonacci numbers up to <paramref name="maxValue"/>.</returns>
        public static IEnumerable<ulong> GetFibonacciNumbersUpToMaxValue(ulong maxValue)
        {
            ulong a = 0L, b = 1L;

            yield return a;

            if (maxValue > 0) { yield return b; }

            while ((a + b) <= maxValue)
            {
                ulong c = (a + b);
                yield return c;
                a = b;
                b = c;
            }
        }

        /// <summary>
        /// Get the first <paramref name="numberToReturn"/> numbers in the Fibonacci series.
        /// </summary>
        /// <param name="numberToReturn">The number if items in the series to return.</param>
        /// <returns>The first <paramref name="numberToReturn"/> numbers in the Fibonacci series.</returns>
        public static IEnumerable<ulong> GetFibonacciNumbersByQuantity(ulong numberToReturn)
        {
            if (numberToReturn > 0)
            {
                yield return 0L;

                if (numberToReturn > 1)
                {
                    yield return 1L;
                }

                ulong[] numbers = new ulong[2] { 0L, 1L };

                for (ulong i = 2L; i < numberToReturn; i++)
                {
                    ulong current = numbers[0] + numbers[1];
                    yield return current;

                    numbers[0] = numbers[1];
                    numbers[1] = current;
                }
            }
        }

        /// <summary>
        /// The Fibonacci number in the <paramref name="index"/> position.
        /// </summary>
        /// <param name="index">The zero-based index in the Fibonacci series to return.</param>
        /// <returns>The Fibonacci number in the zero-based <paramref name="index"/>.</returns>
        public static ulong GetFibonacciNumber(ulong index)
        {
            if (index < 1) { return 0L; }

            return GoIndex(index);
        }

        private static ulong GoIndex(ulong index, ulong a = 0L, ulong b = 1L)
        {
            if (index == 0L) { return a; }
            if (index == 1L) { return b; }

            return GoIndex(index - 1, b, a + b);
        }
    }
}

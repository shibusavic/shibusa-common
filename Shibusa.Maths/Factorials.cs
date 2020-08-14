using System;

namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Gets the factorial value of <paramref name="number"/>.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>The factorial value of <paramref name="number"/>.</returns>
        public static ulong Factorial(int number)
        {
            if (number <= 0) { return 1L; }

            return Go(number);
        }

        private static ulong Go(int number, ulong result = 1)
        {
            if (number == 1) { return result; }
            return Go(number - 1, result * (ulong)number);
        }
    }
}

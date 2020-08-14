namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Sum all of the whole numbers divisible by <paramref name="divisor"/> up to <see cref="max"/>.
        /// </summary>
        /// <param name="divisor">The divisor (the denominator).</param>
        /// <param name="max">The largest number divisible by <paramref name="divisor"/>.</param>
        /// <returns>The sum of all numbers between 1 and <paramref name="max"/>
        /// evenly divisible by <paramref name="divisor"/>.</returns>
        public static ulong SumDivisibleBy(ulong divisor, ulong max)
        {
            return divisor * (max / divisor) * ((max / divisor) + 1) / 2;
        }
    }
}

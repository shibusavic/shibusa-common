namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Determines the number of combinations of a certain size within a set.
        /// </summary>
        /// <param name="sizeOfSet">The size of the set.</param>
        /// <param name="sizeOfCombinations">The size of each combination.</param>
        /// <returns>The number of combinations of a certain size within a set.</returns>
        public static ulong NumberOfCombinations(int sizeOfSet, int sizeOfCombinations) =>
            Factorial(sizeOfSet) / (Factorial(sizeOfCombinations) * Factorial(sizeOfSet - sizeOfCombinations));
    }
}

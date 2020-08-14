namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Determines the number of permutations of a certain size within a certain set..
        /// </summary>
        /// <param name="sizeOfSet">The size of the set.</param>
        /// <param name="sizeOfPermutations">The size of each permutation.</param>
        /// <returns>The number of permutations.</returns>
        public static ulong NumberOfPermutations(int sizeOfSet, int sizeOfPermutations) =>
            Factorial(sizeOfSet) / Factorial(sizeOfSet - sizeOfPermutations);
    }
}

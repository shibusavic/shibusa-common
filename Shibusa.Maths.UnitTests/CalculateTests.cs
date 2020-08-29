using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Shibusa.Maths.UnitTests
{
    public class CalculateTests
    {
        private readonly List<ulong> predefinedPrimes = new List<ulong>() {
            2L, 3L, 5L, 7L, 11L, 13L, 17L, 19L, 23L, 29L, 31L, 37L, 41L,
            43L, 47L, 53L, 59L, 61L, 67L, 71L, 73L, 79L, 83L, 89L, 97L,
            101L, 103L, 107L, 109L, 113L, 127L, 131L, 137L, 139L, 149L,
            151L, 157L, 163L, 167L, 173L, 179L, 181L, 191L, 193L, 197L,
            199L, 211L, 223L, 227L, 229L, 233L, 239L, 241L, 251L, 257L,
            263L, 269L, 271L, 277L, 281L, 283L, 293L, 307L, 311L, 313L,
            317L, 331L, 337L, 347L, 349L, 353L, 359L, 367L, 373L, 379L,
            383L, 389L, 397L, 401L, 409L, 419L, 421L, 431L, 433L, 439L,
            443L, 449L, 457L, 461L, 463L, 467L, 479L, 487L, 491L, 499L,
            503L, 509L, 521L, 523L, 541L, 547L, 557L, 563L, 569L, 571L,
            577L, 587L, 593L, 599L, 601L, 607L, 613L, 617L, 619L, 631L,
            641L, 643L, 647L, 653L, 659L, 661L, 673L, 677L, 683L, 691L,
            701L, 709L, 719L, 727L, 733L, 739L, 743L, 751L, 757L, 761L,
            769L, 773L, 787L, 797L, 809L, 811L, 821L, 823L, 827L, 829L,
            839L, 853L, 857L, 859L, 863L, 877L, 881L, 883L, 887L, 907L,
            911L, 919L, 929L, 937L, 941L, 947L, 953L, 967L, 971L, 977L,
            983L, 991L, 997L
        };

        [Theory]
        [InlineData("2008-02-29", "2012-02-29", 4)]
        [InlineData("2008-02-29", "2009-02-28", 0)]
        [InlineData("2008-02-29", "2009-03-01", 1)]
        [InlineData("1970-01-01", "1970-12-31", 0)]
        [InlineData("1970-01-01", "1971-01-01", 1)]
        [InlineData("1971-01-01", "1970-01-01", -1)]
        [InlineData("2012-02-29", "2008-02-29", -4)]
        public void Age(string birthDateString, string fromDateString, int expected)
        {
            DateTime birthDate = DateTime.ParseExact(birthDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime fromDate = DateTime.ParseExact(fromDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.Equal(expected, Calculate.AgeInYears(birthDate, fromDate));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(5, true)]
        [InlineData(6, false)]
        [InlineData(7, true)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, false)]
        [InlineData(11, true)]
        [InlineData(19, true)]
        [InlineData(647, true)]
        [InlineData(2477, true)]
        public void IsPrime(uint number, bool isPrime)
        {
            if (isPrime)
            {
                Assert.True(Calculate.IsPrime(number));
            }
            else
            {
                Assert.False(Calculate.IsPrime(number));
            }
        }

        [Theory]
        [InlineData(0, 2L)]
        [InlineData(1, 3L)]
        [InlineData(2, 5L)]
        [InlineData(3, 7L)]
        [InlineData(4, 11L)]
        [InlineData(5, 13L)]
        [InlineData(6, 17L)]
        [InlineData(10000, 104743L)]
        public void GetNthPrime(int pos, ulong expected)
        {
            Assert.Equal(expected, Calculate.GetNthPrime(pos));
        }

        [Fact]
        public void GetPrimes_ULong()
        {
            IEnumerable<ulong> actuals = Calculate.GetPrimes(0UL);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimes(1UL);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimes(2UL);
            Assert.True(new List<ulong>() { 2UL }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(3UL);
            Assert.True(new List<ulong>() { 2UL, 3UL }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(4UL);
            Assert.True(new List<ulong>() { 2UL, 3UL }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(5UL);
            Assert.True(new List<ulong>() { 2UL, 3UL, 5UL }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(6UL);
            Assert.True(new List<ulong>() { 2UL, 3UL, 5UL }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(7UL);
            Assert.True(new List<ulong>() { 2UL, 3UL, 5UL, 7UL }.SequenceEqual(actuals));

            var highestPredefinedPrime = predefinedPrimes.Last();

            Assert.True(predefinedPrimes.SequenceEqual(Calculate.GetPrimes(highestPredefinedPrime + 1)));

            Assert.Equal(1009UL, Calculate.GetPrimes(1010UL).Last());
        }

        [Fact]
        public void GetPrimes_Int()
        {
            int[] actuals = Calculate.GetPrimes(0);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimes(1);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimes(2);
            Assert.True(new int[] { 2 }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(3);
            Assert.True(new int[] { 2, 3 }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(4);
            Assert.True(new int[] { 2, 3 }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(5);
            Assert.True(new int[] { 2, 3, 5 }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(6);
            Assert.True(new int[] { 2, 3, 5 }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(7);
            Assert.True(new int[] { 2, 3, 5, 7 }.SequenceEqual(actuals));
        }

        [Fact]
        public void GetPrimeFactorsDictionary()
        {
            var actuals = Calculate.GetPrimeFactorsDictionary(1);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimeFactorsDictionary(2);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(3);
            Assert.True(new Dictionary<ulong, int>() { { 3L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(4);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 2 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(5);
            Assert.True(new Dictionary<ulong, int>() { { 5L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(6);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 1 }, { 3L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(8);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 3 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(9);
            Assert.True(new Dictionary<ulong, int>() { { 3L, 2 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(10);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 1 }, { 5L, 1 } }.SequenceEqual(actuals));
        }

        [Fact]
        public void GetPrimeFactors()
        {
            var actuals = Calculate.GetPrimeFactors(1);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimeFactors(2);
            Assert.True(new List<ulong> { 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(3);
            Assert.True(new List<ulong>() { 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(4);
            Assert.True(new List<ulong>() { 2L, 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(5);
            Assert.True(new List<ulong>() { { 5L } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(6);
            Assert.True(new List<ulong>() { 2L, 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(8);
            Assert.True(new List<ulong>() { 2L, 2L, 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(9);
            Assert.True(new List<ulong>() { 3L, 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(10);
            Assert.True(new List<ulong>() { 2L, 5L }.SequenceEqual(actuals));
        }

        [Fact]
        public void GcdTwoNumbers()
        {
            Assert.Equal<ulong>(5L, Calculate.GreatestCommonDivisor(10L, 5L));
            Assert.Equal<ulong>(12L, Calculate.GreatestCommonDivisor(12L, 12L));
            Assert.Equal<ulong>(6L, Calculate.GreatestCommonDivisor(12L, 18L));
        }

        [Fact]
        public void GcdArrayOfNumbers()
        {
            Assert.Equal<ulong>(5L, Calculate.GreatestCommonDivisor(new ulong[] { 10L, 5L, 15L }));
            Assert.Equal<ulong>(12L, Calculate.GreatestCommonDivisor(new ulong[] { 12L, 12L, 36L }));
            Assert.Equal<ulong>(3L, Calculate.GreatestCommonDivisor(new ulong[] { 3L, 12L, 15L, 9L }));
            Assert.Equal<ulong>(0L, Calculate.GreatestCommonDivisor(new ulong[] { 0L, 0L }));
        }

        [Theory]
        [InlineData(2, 4, 4)]
        [InlineData(2, 9, 18)]
        [InlineData(3, 12, 12)]
        [InlineData(18, 24, 72)]
        [InlineData(21, 6, 42)]
        [InlineData(0, 6, 0)]
        [InlineData(6, 0, 0)]
        [InlineData(0, 0, 0)]
        public void LcmTwoNumbers(ulong a, ulong b, ulong expected)
        {
            Assert.Equal(expected, Calculate.LeastCommonMultiple(a, b));
        }

        [Fact]
        public void LcmArrayOfNumbers()
        {
            Assert.Equal<ulong>(8L, Calculate.LeastCommonMultiple(new ulong[] { 2L, 4L, 8L }));
            Assert.Equal<ulong>(27L, Calculate.LeastCommonMultiple(new ulong[] { 3L, 9L, 27L }));
            Assert.Equal<ulong>(260L, Calculate.LeastCommonMultiple(new ulong[] { 4L, 5L, 13L }));
            Assert.Equal<ulong>(4L, Calculate.LeastCommonMultiple(new ulong[] { 2L, 4L }));
            Assert.Equal<ulong>(2L, Calculate.LeastCommonMultiple(new ulong[] { 2L }));
            Assert.Equal<ulong>(0L, Calculate.LeastCommonMultiple(new ulong[] { 0L }));
            Assert.Equal<ulong>(0L, Calculate.LeastCommonMultiple(new ulong[] { 0L, 0L }));
        }

        [Fact]
        public void GetFibonacciNumbers()
        {
            var fibs = Calculate.GetFibonacciNumbersByQuantity(6);
            var expected = new List<ulong>() { 0L, 1L, 1L, 2L, 3L, 5L };
            Assert.True(expected.SequenceEqual(fibs));

            Assert.Empty(Calculate.GetFibonacciNumbersByQuantity(0));
        }

        [Fact]
        public void GetFibonacciNumber()
        {
            Assert.Equal<ulong>(0, Calculate.GetFibonacciNumber(0));
            Assert.Equal<ulong>(1, Calculate.GetFibonacciNumber(1));
            Assert.Equal<ulong>(1, Calculate.GetFibonacciNumber(2));
            Assert.Equal<ulong>(2, Calculate.GetFibonacciNumber(3));
            Assert.Equal<ulong>(3, Calculate.GetFibonacciNumber(4));
            Assert.Equal<ulong>(5, Calculate.GetFibonacciNumber(5));
            Assert.Equal<ulong>(8, Calculate.GetFibonacciNumber(6));
        }

        [Fact]
        public void GetFibonacciNumbersUpToValue()
        {
            Assert.Single(Calculate.GetFibonacciNumbersUpToMaxValue(0));
            Assert.Equal(3, Calculate.GetFibonacciNumbersUpToMaxValue(1).Count());
            Assert.Equal(4, Calculate.GetFibonacciNumbersUpToMaxValue(2).Count());
            Assert.Equal(5, Calculate.GetFibonacciNumbersUpToMaxValue(3).Count());
            Assert.Equal(5, Calculate.GetFibonacciNumbersUpToMaxValue(4).Count());
            Assert.Equal(6, Calculate.GetFibonacciNumbersUpToMaxValue(5).Count());
            Assert.Equal(7, Calculate.GetFibonacciNumbersUpToMaxValue(8).Count());

            var seq = Calculate.GetFibonacciNumbersUpToMaxValue(9);
            Assert.Equal<ulong>(0, seq.ElementAt(0));
            Assert.Equal<ulong>(1, seq.ElementAt(1));
            Assert.Equal<ulong>(1, seq.ElementAt(2));
            Assert.Equal<ulong>(2, seq.ElementAt(3));
            Assert.Equal<ulong>(3, seq.ElementAt(4));
            Assert.Equal<ulong>(5, seq.ElementAt(5));
            Assert.Equal<ulong>(8, seq.ElementAt(6));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 6)]
        [InlineData(4, 24)]
        [InlineData(5, 120)]
        public void Factorial(int number, ulong result)
        {
            Assert.Equal(result, Calculate.Factorial(number));
        }

        [Theory]
        [InlineData(3, 2, 6)]
        [InlineData(5, 5, 120)]
        public void Permutations(int sizeOfSet, int sizeOfPermutations, ulong result)
        {
            Assert.Equal(result, Calculate.NumberOfPermutations(sizeOfSet, sizeOfPermutations));
        }

        [Theory]
        [InlineData(3, 2, 3)]
        [InlineData(4, 3, 4)]
        public void Combinations(int sizeOfSet, int sizeOfCombinations, ulong result)
        {
            Assert.Equal(result, Calculate.NumberOfCombinations(sizeOfSet, sizeOfCombinations));
        }
    }
}

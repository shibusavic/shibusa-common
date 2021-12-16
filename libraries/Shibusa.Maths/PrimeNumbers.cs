using System.Collections;

namespace Shibusa.Maths
{
    public static partial class Calculate
    {
        private static readonly List<ulong> predefinedPrimes = new()
        {
            2L,
            3L,
            5L,
            7L,
            11L,
            13L,
            17L,
            19L,
            23L,
            29L,
            31L,
            37L,
            41L,
            43L,
            47L,
            53L,
            59L,
            61L,
            67L,
            71L,
            73L,
            79L,
            83L,
            89L,
            97L,
            101L,
            103L,
            107L,
            109L,
            113L,
            127L,
            131L,
            137L,
            139L,
            149L,
            151L,
            157L,
            163L,
            167L,
            173L,
            179L,
            181L,
            191L,
            193L,
            197L,
            199L,
            211L,
            223L,
            227L,
            229L,
            233L,
            239L,
            241L,
            251L,
            257L,
            263L,
            269L,
            271L,
            277L,
            281L,
            283L,
            293L,
            307L,
            311L,
            313L,
            317L,
            331L,
            337L,
            347L,
            349L,
            353L,
            359L,
            367L,
            373L,
            379L,
            383L,
            389L,
            397L,
            401L,
            409L,
            419L,
            421L,
            431L,
            433L,
            439L,
            443L,
            449L,
            457L,
            461L,
            463L,
            467L,
            479L,
            487L,
            491L,
            499L,
            503L,
            509L,
            521L,
            523L,
            541L,
            547L,
            557L,
            563L,
            569L,
            571L,
            577L,
            587L,
            593L,
            599L,
            601L,
            607L,
            613L,
            617L,
            619L,
            631L,
            641L,
            643L,
            647L,
            653L,
            659L,
            661L,
            673L,
            677L,
            683L,
            691L,
            701L,
            709L,
            719L,
            727L,
            733L,
            739L,
            743L,
            751L,
            757L,
            761L,
            769L,
            773L,
            787L,
            797L,
            809L,
            811L,
            821L,
            823L,
            827L,
            829L,
            839L,
            853L,
            857L,
            859L,
            863L,
            877L,
            881L,
            883L,
            887L,
            907L,
            911L,
            919L,
            929L,
            937L,
            941L,
            947L,
            953L,
            967L,
            971L,
            977L,
            983L,
            991L,
            997L
        };
        
        private static readonly ulong largestPredeterminedPrime = predefinedPrimes.Last();

        /// <summary>
        /// Determine if a number is prime.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>An indicator of whether <paramref name="number"/> is prime.</returns>
        public static bool IsPrime(ulong number)
        {
            if (number <= largestPredeterminedPrime)
            {
                return predefinedPrimes.Contains(number);
            }

            ulong boundary = Convert.ToUInt64(Math.Floor(Math.Sqrt(number)));

            for (ulong i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Gets the nth prime number
        /// </summary>
        /// <param name="position">A zero-based index of the prime number to retrieve.</param>
        /// <returns>The Nth prime, starting at the 0 index.</returns>
        public static ulong GetNthPrime(int position)
        {
            int count = predefinedPrimes.Count;
            if (position < count)
            {
                return predefinedPrimes[position];
            }
            else
            {
                int index = count - 1;
                ulong prime = predefinedPrimes.Last();
                ulong num = prime;
                while (index < position)
                {
                    num += 2;
                    if (IsPrime(num))
                    {
                        prime = num;
                        index++;
                        predefinedPrimes.Add(prime);
                    }
                }
                return prime;
            }
        }

        /// <summary>
        /// Get all of the prime numbers up to <paramref name="upperLimit"/>.
        /// </summary>
        /// <param name="upperLimit">The highest possible value returned.</param>
        /// <returns>A collection of primes up to <paramref name="upperLimit"/>.</returns>
        public static IEnumerable<ulong> GetPrimes(ulong upperLimit)
        {
            foreach (ulong val in predefinedPrimes)
            {
                if (val <= upperLimit) { yield return val; }
                else { break; }
            }

            if (upperLimit > largestPredeterminedPrime)
            {
                for (ulong i = largestPredeterminedPrime + 2; i <= upperLimit; i += 2)
                {
                    if (IsPrime(i))
                    {
                        predefinedPrimes.Add(i);
                        yield return i;
                    }
                }
            }
        }

        /// <summary>
        /// Get all of the prime numbers up to <paramref name="upperLimit"/>.
        /// </summary>
        /// <param name="upperLimit">The highest possible value returned.</param>
        /// <returns>A collection of primes up to <paramref name="upperLimit"/>.</returns>
        /// <remarks>This method uses the seive of Eratosthenes algorithm.
        /// <seealso cref="https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes"/></remarks>
        public static int[] GetPrimes(int upperLimit)
        {
            if (upperLimit <= 1) return Array.Empty<int>();
            if (upperLimit == 2) return new int[1] { 2 };

            int sieveBound = (int)(upperLimit - 1) / 2;
            int boundary = ((int)Math.Sqrt(upperLimit) - 1) / 2;

            BitArray primeBits = new(sieveBound + 1, true);

            for (int i = 1; i <= boundary; i++)
            {
                if (primeBits.Get((int)i))
                {
                    for (int j = i * 2 * (i + 1); j <= sieveBound; j += 2 * i + 1)
                    {
                        primeBits.Set(j, false);
                    }
                }
            }

            List<int> numbers = new((int)(upperLimit / (Math.Log(upperLimit) - 1.08366)))
            {
                2
            };

            for (int i = 1; i <= sieveBound; i++)
            {
                if (primeBits.Get(i))
                {
                    numbers.Add((2 * i + 1));
                }
            }

            return numbers.ToArray();
        }

        /// <summary>
        /// Gets a <see cref="IDictionary{TKey, TValue}"/> of prime factors for the
        /// number provided.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>A <see cref="IDictionary{TKey, TValue}"/> wherein the keys are prime
        /// numbers and the values are the counts for those primes.</returns>
        /// <example>
        /// If <paramref name="number"/> is 9, the result would be a dictionary with one
        /// <see cref="KeyValuePair{TKey, TValue}"/> where the key is 3 and the value is 2.
        /// </example>
        public static IDictionary<ulong, int> GetPrimeFactorsDictionary(ulong number)
        {
            IDictionary<ulong, int> results = new SortedDictionary<ulong, int>();

            GetPrimeFactors(number, ref results);

            return results;
        }

        /// <summary>
        /// Get prime factors for a number as a collection.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>A collection of prime factors for the number provided.</returns>
        public static IEnumerable<ulong> GetPrimeFactors(ulong number)
        {
            IDictionary<ulong, int> pairs = GetPrimeFactorsDictionary(number);
            foreach (var kvp in pairs)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    yield return kvp.Key;
                }
            }
        }

        private static void GetPrimeFactors(ulong number, ref IDictionary<ulong, int> dict)
        {
            if (number > 1L)
            {
                int primeIndex = 0;
                ulong p = predefinedPrimes[primeIndex];
                while (p <= number)
                {
                    if (number % p == 0)
                    {
                        AddToPrimeDictionary(p, ref dict);
                        GetPrimeFactors(number / p, ref dict);
                        break;
                    }
                    else
                    {
                        p = (p >= predefinedPrimes.Last())
                            ? p + 2
                            : predefinedPrimes[++primeIndex];
                    }
                }
            }
        }

        private static void AddToPrimeDictionary(ulong prime, ref IDictionary<ulong, int> dict)
        {
            if (dict.ContainsKey(prime))
            {
                dict[prime]++;
            }
            else
            {
                dict.Add(prime, 1);
            }
        }
    }
}

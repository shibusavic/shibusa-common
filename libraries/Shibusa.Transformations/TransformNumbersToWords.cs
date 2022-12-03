using System.Text.RegularExpressions;

namespace Shibusa.Transformations
{
    /// <summary>
    /// Utility class for converting numbers to English text.
    /// </summary>
    public static class TransformNumbersToWords
    {
        private static readonly string[] zeroToNineteen = { "zero",  "one",   "two",  "three", "four",   "five",   "six",
            "seven", "eight", "nine", "ten",   "eleven", "twelve", "thirteen",
            "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

        private static readonly string[] tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        private static readonly string[] denom = { "", "thousand", "million", "billion", "trillion", "quadrillion",
             "quintillion", "sextillion", "septillion", "octillion", "nonillion",
             "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion",
             "sexdecillion", "septendecillion", "octodecillion", "novemdecillion", "vigintillion" };


        /// <summary>
        /// Convert a number string to English words.
        /// </summary>
        /// <param name="val">The string to convert.</param>
        /// <returns>A string representing the number in words.</returns>
        public static string ConvertToWords(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) { throw new ArgumentNullException(nameof(val)); }

            val = val.Trim();

            Regex regex = new("-?\\d+");
            if (!regex.IsMatch(val))
            {
                throw new ArgumentException("Value provided is not a number.");
            }

            string words = string.Empty;

#if NETSTANDARD2_0
            bool isNegative = val.Substring(0, 1) == "-";
#else
            bool isNegative = val[..1] == "-";
#endif
            if (isNegative)
            {
#if NETSTANDARD2_0
                val = val.Substring(1);
#else
                val = val[1..];
#endif
            }

            if (val.All(v => v == '0'))
            {
                words = zeroToNineteen[0];
            }
            else
            {
                if (Convert.ToInt32(Math.Ceiling((double)val.Length / 3)) > denom.Length)
                {
                    throw new ArgumentException("Number to convert is too large");
                }

                int denomIndex = 0;
                while (val.Length > 3)
                {
                    var valToProcess = GetLastThree(val, out string newVal);
                    if (Convert.ToInt32(valToProcess) == 0)
                    {
                        denomIndex++;
                    }
                    else
                    {
                        words = $"{ConvertThreeDigits(Convert.ToInt32(valToProcess))} {denom[denomIndex++]} {words}";
                    }

                    val = newVal;
                }

                words = $"{ConvertThreeDigits(Convert.ToInt32(val))} {denom[denomIndex]} {words}";

                if (isNegative)
                {
                    words = $"negative {words}";
                }

                while (words.Contains("  "))
                {
                    words = words.Replace("  ", " ");
                }
            }

            return words.Trim();
        }

        /// <summary>
        /// Convert an unsigned integer to English words.
        /// </summary>
        /// <param name="val">The unisgned integer.</param>
        /// <returns>A string representing the number in words.</returns>
        public static string ConvertToWords(uint val) => ConvertToWords((ulong)val);

        /// <summary>
        /// Convert an integer to English words.
        /// </summary>
        /// <param name="val">The integer to convert.</param>
        /// <returns>A string representing the number in words.</returns>
        public static string ConvertToWords(int val) => ConvertToWords((long)val);

        /// <summary>
        /// Convert a 64-bit integer to English words.
        /// </summary>
        /// <param name="val">The 64-bit integer to convert.</param>
        /// <returns>A string representing the number in words.</returns>
        public static string ConvertToWords(long val)
        {
            string words;
            if (val < 0)
            {
#if NETSTANDARD2_0
                string str = val.ToString().Substring(1);
#else
                string str = val.ToString()[1..];
#endif
                if (!ulong.TryParse(str, out ulong res))
                {
                    throw new Exception("Number too small to convert.");
                }
                words = $"negative {ConvertToWords(res)}";
            }
            else
            {
                words = ConvertToWords(Convert.ToUInt64(val));
            }
            return words;
        }

        /// <summary>
        /// Convert an unsigned 64-bit integer to English words.
        /// </summary>
        /// <param name="val">The unsigned 64-bit integer to convert.</param>
        /// <returns>A string representing the number in words.</returns>
        public static string ConvertToWords(ulong val)
        {
            string words = string.Empty;
            if (val == 0)
            {
                words = zeroToNineteen[0];
            }
            else
            {
                int denomIndex = 0;
                while (val > 0)
                {
                    var lastThree = GetLastThree(val, out ulong newVal);
                    if (lastThree > 0)
                    {
                        words = $"{ConvertThreeDigits(Convert.ToInt32(lastThree))} {denom[denomIndex]} {words}";
                    }
                    val = newVal;
                    denomIndex++;
                }
            }

            return words.Trim();
        }

        private static ulong GetLastThree(ulong val, out ulong newVal)
        {
            newVal = Convert.ToUInt64(Math.Floor((double)val / 1000));
            return val % 1000;
        }

        private static string GetLastThree(string val, out string newVal)
        {
#if NETSTANDARD2_0
            int len = val.Length;
            newVal = len > 3 ? val.Substring(0, len - 3)
                : string.Empty;
            return val.Substring(len - 3);
#else
            newVal = val.Length > 3 ? val[0..^3]
                : string.Empty;
            return val[^3..];
#endif
        }

        private static string ConvertThreeDigits(int val)
        {
            string words;
            if (val == 0)
            {
                words = string.Empty;
            }
            else if (val < 20)
            {
                words = zeroToNineteen[val];
            }
            else if (val < 100)
            {
                var v = Convert.ToInt32(Math.Floor((decimal)val / 10));
                var r = Convert.ToInt32(val % 10);
                words = $" {tens[v - 2]}";
                if (r != 0) { words += $"-{zeroToNineteen[r]}"; }
            }
            else
            {
                var v = Convert.ToInt32(Math.Floor((decimal)val / 100));
                var r = Convert.ToInt32(val % 100);
                words = $" {zeroToNineteen[v]} hundred {ConvertThreeDigits(r)}";
            }

            return words.Trim();
        }
    }
}

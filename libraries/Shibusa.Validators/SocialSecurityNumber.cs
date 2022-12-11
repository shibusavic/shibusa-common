using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Shibusa.Validators
{
    /// <summary>
    /// Utility for validating social security number.
    /// </summary>
    public static class SocialSecurityNumber
    {
        /// <summary>
        /// Determines if the strcuture of the social security number is valid.
        /// </summary>
        /// <param name="socialSecurityNumber">The SSN to validate.</param>
        /// <returns>An indicator of whether the structure of the SSN is valid.</returns>
        public static bool IsValidStructure(string socialSecurityNumber)
        {
            if (string.IsNullOrWhiteSpace(socialSecurityNumber)) { return false; }

            Regex unformattedSsnRegex = new(@"^\d{3}-?\d{2}-?\d{4}$");

            return unformattedSsnRegex.IsMatch(socialSecurityNumber);
        }

        /// <summary>
        /// Determines if a Social Security Number is a valid issue.
        /// </summary>
        /// <param name="socialSecurityNumber">The SSN to validate.</param>
        /// <returns>An indicator of whether the SSN is valid.</returns>
        public static bool IsValid(string socialSecurityNumber)
        {
            bool isVerified = false;
            if (IsValidStructure(socialSecurityNumber))
            {
                string numbersOnly = socialSecurityNumber.Replace("-", "");
                ushort area = ushort.Parse(numbersOnly[..3]);
                ushort group = ushort.Parse(numbersOnly[3..5]);
                ushort series = ushort.Parse(numbersOnly[5..]);

                isVerified = (area > 0 && group > 0 && series > 0
                    && !unusedAreas.Any(a => a.low <= area && area <= a.high));
            }

            return isVerified;
        }

        /// <summary>
        /// Gets a collection of unused areas.
        /// </summary>
        public static IEnumerable<int> UnusedAreas
        {
            get
            {
                List<int> results = new();
                foreach ((ushort low, ushort high) in unusedAreas)
                {
                    results.AddRange(Enumerable.Range(low, high - low + 1));
                }
                return results;
            }
        }

        /// <summary>
        /// Gets the collection of used areas.
        /// </summary>
        public static IEnumerable<int> UsedAreas => Enumerable.Range(1, 999).Except(UnusedAreas);

        /// <summary>
        /// Unused Areas.
        /// </summary>
        /// <seealso cref="https://www.ssa.gov/employer/stateweb.htm"/>
        private static readonly ReadOnlyCollection<(ushort low, ushort high)> unusedAreas =
            new(new List<(ushort, ushort)> {
                (237,246),
                (587,699),
                (750,999)
            });
    }
}

using System;

namespace Shibusa.Maths
{
    /// <summary>
    /// Utility class for common calculations.
    /// </summary>
    public static partial class Calculate
    {
        /// <summary>
        /// Calculate an age from a <paramref name="birthDate"/>.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <param name="fromDate">The date from which to calculate.</param>
        /// <returns>An integer representation of age.</returns>
        public static int AgeInYears(DateTime birthDate, DateTime? fromDate = null)
        {
            fromDate = fromDate ?? DateTime.Now;
            var age = fromDate.Value.Year - birthDate.Year;
            if (birthDate > fromDate.Value.AddYears(-age)) { age--; }

            return age;
        }
    }
}

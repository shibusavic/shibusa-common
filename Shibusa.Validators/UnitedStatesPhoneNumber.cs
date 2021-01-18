using System.Linq;

namespace Shibusa.Validators
{
    /// <summary>
    /// Utility for validating a phone number.
    /// </summary>
    public static class UnitedStatesPhoneNumber
    {
        /// <summary>
        /// Determines if the structure of the phone number is valid.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <param name="validCounts">An array of counts that are valid (e.g., 4, 7, 10).</param>
        /// <returns>An indicator of whether the structure of the phone number is valid.</returns>
        public static bool IsValidStructure(string phoneNumber, params int[] validCounts)
        {
            string numbersOnly = new string(phoneNumber.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            int length = numbersOnly.Length;

            return (validCounts != null && validCounts.Any())
                ? validCounts.Contains(length)
                : length >= 10 && length <= 11;
        }
    }
}

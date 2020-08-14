using System;
using System.Linq;

namespace Shibusa.Transformations
{
    /// <summary>
    /// Providers formatting for U.S. phone numbers.
    /// </summary>
    public sealed class SocialSecurityNumberFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// Converts the value of a specified object to an equivalent string representation 
        /// using specified format and culture-specific formatting information.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.
        /// Use "F" to format with dashes, "N" to format with only numbers, and "dots"
        /// to format with dots instead of dashes.</param>
        /// <param name="arg">An object to format.</param>
        /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
        /// <returns>The string representation of the value of arg, formatted as specified by format and formatProvider.</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Check whether this is an appropriate callback             
            if (!Equals(formatProvider)) { return null; }

            // Set default format specifier             
            if (string.IsNullOrWhiteSpace(format)) { format = "F"; }

            string numericString = new string(arg.ToString().ToCharArray().Where(c => char.IsDigit(c)).ToArray());

            if (numericString.Length != 9)
            {
                throw new FormatException(string.Format("SSN requires 9 digits.", format, arg.ToString()));
            }

            string result = numericString;

            result = format switch
            {
                "N" => numericString,
                "F" => $"{numericString.Substring(0, 3)}-{numericString.Substring(3, 2)}-{numericString.Substring(5)}",
                "dots" => $"{numericString.Substring(0, 3)}.{numericString.Substring(3, 2)}.{numericString.Substring(5)}",
                _ => throw new FormatException(string.Format("The {0} format specifier is invalid.", format)),
            };
            return result;
        }

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>An instance of the object specified by formatType, if the System.IFormatProvider implementation can supply
        /// that type of object; otherwise, null.</returns>
        public object GetFormat(Type formatType) =>
            (formatType == typeof(ICustomFormatter)) ? this : null;
    }
}

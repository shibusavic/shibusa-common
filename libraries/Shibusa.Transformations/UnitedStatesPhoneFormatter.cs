namespace Shibusa.Transformations
{
    /// <summary>
    /// Providers formatting for U.S. phone numbers.
    /// </summary>
    public sealed class UnitedStatesPhoneFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// Converts the value of a specified object to an equivalent string representation 
        /// using specified format and culture-specific formatting information.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.
        /// Use 'N' for digits only, 'F' for formatted, 'dots' to use dots instead of dashes, 'I' to include country code,
        /// and 'Idots' for country code with dots.</param>
        /// <param name="arg">An object to format.</param>
        /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
        /// <returns>The string representation of the value of arg, formatted as specified by format and formatProvider.</returns>
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            // Check whether this is an appropriate callback             
            if (!Equals(formatProvider)) { return string.Empty; }

            // Set default format specifier             
            if (string.IsNullOrWhiteSpace(format)) { format = "F"; }

            string numericString = new(arg?.ToString()?.ToCharArray().Where(c => char.IsDigit(c)).ToArray());

            string result = numericString;

            switch (format)
            {
                case "N":
                    if (numericString.Length <= 4
                        || numericString.Length == 7
                        || numericString.Length == 10)
                    {
                        result = numericString;
                    }
                    else
                    {
                        throw new FormatException(string.Format("'{0}' cannot be used to format '{1}'.", format, arg?.ToString()));
                    }
                    break;
                case "F":
                    if (numericString.Length <= 4)
                    {
                        result = numericString;
                    }
                    else if (numericString.Length == 7)
                    {
                        result = $"{numericString[..3]}-{numericString[3..]}";
                    }
                    else if (numericString.Length == 10)
                    {
                        result = $"({numericString[..3]}) {numericString[3..6]}-{numericString[6..]}";
                    }
                    else
                    {
                        throw new FormatException(string.Format("'{0}' cannot be used to format '{1}'.", format, arg?.ToString()));
                    }
                    break;
                case "dots":
                    if (numericString.Length <= 4)
                    {
                        result = numericString;
                    }
                    else if (numericString.Length == 7)
                    {
                        result = $"{numericString[..3]}.{numericString[3..]}";
                    }
                    else if (numericString.Length == 10)
                    {
                        result = $"{numericString[..3]}.{numericString[3..6]}.{numericString[6..]}";
                    }
                    else
                    {
                        throw new FormatException(string.Format("'{0}' cannot be used to format '{1}'.", format, arg?.ToString()));
                    }
                    break;
                case "I":
                    if (numericString.Length != 10)
                    {
                        throw new FormatException(string.Format("'{0}' does not have 10 digits.", arg?.ToString()));
                    }
                    else
                    {
                        result = $"+1 ({numericString[..3]}) {numericString[3..6]}-{numericString[6..]}";
                    }
                    break;
                case "Idots":
                    if (numericString.Length != 10)
                    {
                        throw new FormatException(string.Format("'{0}' does not have 10 digits.", arg?.ToString()));
                    }
                    else
                    {
                        result = $"+1.{numericString[..3]}.{numericString[3..6]}.{numericString[6..]}";
                    }
                    break;
                default:
                    throw new FormatException(string.Format("The {0} format specifier is invalid.", format));
            }

            return result;
        }

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>An instance of the object specified by formatType, if the System.IFormatProvider implementation can supply
        /// that type of object; otherwise, null.</returns>
        public object? GetFormat(Type? formatType) => (formatType == typeof(ICustomFormatter)) ? this : null;
    }
}


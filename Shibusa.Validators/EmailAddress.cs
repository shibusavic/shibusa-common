using System.Linq;
using System.Text.RegularExpressions;

namespace Shibusa.Validators
{
    /// <summary>
    /// Provides validation for email adresses.
    /// </summary>
    public static class EmailAddress
    {
        /// <summary>
        /// Determines if a string has the structure of a valid email address.
        /// This function only checks the structure of the input and does not verify that the 
        /// email exists. This function is not perfect, but it'll do.
        /// </summary>
        /// <remarks>The actual RFC 2822 standard cannot be contained in a single expression.</remarks>
        /// <seealso cref="http://codefool.tumblr.com/post/15288874550/list-of-valid-and-invalid-email-addresses"/>
        /// <seealso cref="https://en.wikipedia.org/wiki/Email_address"/>
        /// <seealso cref="https://www.w3.org/Protocols/rfc822/3_Lexical.html"/>
        /// <seealso cref="https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/"/>
        /// <param name="email">Email address to validate.</param>
        /// <returns>True if valid, otherwise false.</returns>
        public static bool IsValid(string email)
        {
            var result = !string.IsNullOrWhiteSpace(email)
                && !email.Contains("..")
                && email.Contains(".")
                && !email.StartsWith("@")
                && (email.Count(e => e == '@') == 1);

            if (result)
            {
                var split = email.Split('@');
                if (split.Length != 2) { result = false; }

                result = split.Length == 2 &&
                    ValidateLeftSideOfEmail(split[0]) &&
                    ValidateRightSideOfEmail(split[1]);
            }

            return result;
        }

        private static bool ValidateLeftSideOfEmail(string left)
        {
            bool result = (Regex.IsMatch(left, "\" +\"")) ? true
                : (left.Contains(" ")) ? false : true;

            result = result && !(left.StartsWith(".") || left.EndsWith("."));

            result = result && (!left.Any(c => char.IsDigit(c))
               || !left.Any(c => char.IsUpper(c))
               || !left.Any(c => char.IsLower(c)));

            if (!result)
            {
                result = left.All(c => c == '_');
            }

            return result;
        }

        private static bool ValidateRightSideOfEmail(string right)
        {
            if (right.Contains(" ")) { return false; }
            if (right.StartsWith("-")) { return false; }

            var result = Regex.IsMatch(right, @"^(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (!result || (!right.Any(c => char.IsUpper(c)) && !right.Any(c => char.IsLower(c))))
            {
                result = Regex.IsMatch(right, @"^\[?\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\]?$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }

            return result;
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace Shibusa.Transformations
{
    /// <summary>
    /// Utility for common cryptography needs.
    /// </summary>
    public static class Cryptography
    {
        /// <summary>
        /// Get the hash for an input.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="input">The content to hash.</param>
        /// <returns>A string hash.</returns>
        public static string GetHash(HashAlgorithm? hashAlgorithm, string? input)
        {
            ArgumentNullException.ThrowIfNull(hashAlgorithm);
            if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentNullException(nameof(input)); }

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            var sBuilder = new StringBuilder();

            for (int d = 0; d < data.Length; d++)
            {
                sBuilder.Append(data[d].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify the hash for a certain input.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="input">The content to hash.</param>
        /// <param name="hash">The expected hash.</param>
        /// <returns>True if the hashes match, otherwise false.</returns>
        public static bool VerifyHash(HashAlgorithm? hashAlgorithm, string? input, string? hash) =>
            StringComparer.OrdinalIgnoreCase.Compare(GetHash(hashAlgorithm, input), hash) == 0;

        /// <summary>
        /// Get the hash for a file.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="fileInfo">The file to hash.</param>
        /// <returns>A string hash.</returns>
        public static string GetHashForFile(HashAlgorithm? hashAlgorithm, FileInfo fileInfo) =>
            GetHash(hashAlgorithm, File.ReadAllText(fileInfo.FullName));

        /// <summary>
        /// Verify the hash of a file.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="fileInfo">The file to hash.</param>
        /// <param name="hash">The expected hash.</param>
        /// <returns>True if the hashes match, otherwise false.</returns>
        public static bool VerifyHashForFile(HashAlgorithm? hashAlgorithm, FileInfo fileInfo, string hash) =>
            VerifyHash(hashAlgorithm, File.ReadAllText(fileInfo.FullName), hash);
    }
}

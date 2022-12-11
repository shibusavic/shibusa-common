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

        /// <summary>
        /// Encrypt a byte array into a byte array using the "AesManaged" algorithm.
        /// </summary>
        /// <param name="original">The bytes to encrypt.</param>
        /// <param name="passkey">The secret passphrase.</param>
        /// <returns>A byte array of the encrypted cipher.</returns>
        public static byte[] EncryptAes(byte[] original, string passkey)
        {
            byte[] key = Enumerable.Repeat<byte>(0, 16).ToArray();
            var passkeyBytes = Encoding.UTF8.GetBytes(passkey).Take(16).ToArray();

            for (int i = 0; i < passkeyBytes.Length; i++)
            {
                key[i] = passkeyBytes[i];
            }

            return EncryptAes(original, key);
        }

        /// <summary>
        /// Encrypt a byte array into a byte array using the "AesManaged" algorithm.
        /// </summary>
        /// <param name="original">The bytes to encrypt.</param>
        /// <param name="key">The key, the length of which must be divisible by 16.</param>
        /// <returns>A byte array of the encrypted cipher.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="key"/>'s length is not divisible by 16.</exception>
        public static byte[] EncryptAes(byte[] original, byte[] key)
        {
            if (key.Length < 1 || key.Length % 16 != 0) { throw new ArgumentException($"{nameof(key)} length must be divisible by 16."); }

            var aes = Aes.Create();

            if (aes == null) { throw new ArgumentException("Could not create AES."); }

            using var encryptor = aes.CreateEncryptor(key, aes.IV);

            using (MemoryStream ms = new())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new(cs))
                    {
                        sw.Write(Encoding.UTF8.GetString(original));
                        sw.Flush();
                        cs.FlushFinalBlock();
                        return ms.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt a byte array into a byte array using the "AesManaged" algorithm.
        /// </summary>
        /// <param name="cipher">The encrypted bytes to transform.</param>
        /// <param name="passkey">The secret passphrase.</param>
        /// <returns>A byte array matching the original.</returns>
        public static byte[] DecryptAes(byte[] original, string passkey)
        {
            byte[] key = Enumerable.Repeat<byte>(0, 16).ToArray();
            var passkeyBytes = Encoding.UTF8.GetBytes(passkey).Take(16).ToArray();

            for (int i = 0; i < passkeyBytes.Length; i++)
            {
                key[i] = passkeyBytes[i];
            }

            return DecryptAes(original, key);
        }

        /// <summary>
        /// Decrypt a byte array into a byte array using the "AesManaged" algorithm.
        /// </summary>
        /// <param name="cipher">The encrypted bytes to transform.</param>
        /// <param name="key">The key, the length of which must be divisible by 16.</param>
        /// <returns>A byte array matching the original.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="key"/>'s length is not divisible by 16.</exception>
        public static byte[] DecryptAes(byte[] cipher, byte[] key)
        {
            if (key.Length < 1 || key.Length % 16 != 0) { throw new ArgumentException($"{nameof(key)} length must be divisible by 16."); }

            var aes = Aes.Create();

            if (aes == null) { throw new ArgumentException("Could not create AES."); }

            aes.IV = cipher[..aes.IV.Length];

            var decryptor = aes.CreateDecryptor(key, aes.IV);

            using (MemoryStream ms = new(cipher[aes.IV.Length..]))
            {
                using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new(cs))
                    {
                        return Encoding.UTF8.GetBytes(sr.ReadToEnd());
                    }
                }
            }
        }
    }
}
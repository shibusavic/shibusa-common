using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class CryptographyTests
    {
        [Fact]
        public void HashAndVerify_Success()
        {
            string? content = File.ReadAllText("CryptoTest.txt");
            HashAlgorithm? algorithm = SHA256.Create();
            string? hash = Cryptography.GetHash(algorithm, content);

            Assert.NotNull(hash);
            Assert.NotEmpty(hash);

            Assert.True(Cryptography.VerifyHash(algorithm, content, hash));
        }

        [Fact]
        public void HashAndVerify_Fail()
        {
            string? content = File.ReadAllText("CryptoTest.txt");
            HashAlgorithm? algorithm = SHA256.Create();
            string? hash = Cryptography.GetHash(algorithm, content);

            string? reverseContent = content.ToArray().Reverse().ToString();
            string? hash2 = Cryptography.GetHash(algorithm, reverseContent);

            Assert.False(Cryptography.VerifyHash(algorithm, content, hash2));
            Assert.False(Cryptography.VerifyHash(algorithm, reverseContent, hash));
        }

        [Fact]
        public async Task HashAndVerify_TwoFilesAsync()
        {
            string? tempFilename = Path.GetTempFileName();

            try
            {
                HashAlgorithm? algorithm = SHA256.Create();

                FileStream? tempFile = File.Create(tempFilename);

                FileInfo? fileInfo1 = new("CryptoTest.txt");
                FileInfo? fileInfo2 = new(tempFilename);

                string? content1 = File.ReadAllText(fileInfo1.FullName);

                // Make the second file identical except for an additional newline character.
                byte[]? buffer = Encoding.UTF8.GetBytes($"{content1}{Environment.NewLine}");
                await tempFile.WriteAsync(buffer.AsMemory(0, buffer.Length));
                await tempFile.FlushAsync();
                tempFile.Close();

                string? hash1 = Cryptography.GetHashForFile(algorithm, fileInfo1);
                string? hash2 = Cryptography.GetHashForFile(algorithm, fileInfo2);

                Assert.NotEqual(hash1, hash2);

                Assert.True(Cryptography.VerifyHashForFile(algorithm, fileInfo1, hash1));
                Assert.True(Cryptography.VerifyHashForFile(algorithm, fileInfo2, hash2));

                Assert.False(Cryptography.VerifyHashForFile(algorithm, fileInfo1, hash2));
                Assert.False(Cryptography.VerifyHashForFile(algorithm, fileInfo2, hash1));
            }
            finally
            {
                File.Delete(tempFilename);
            }
        }

        [Fact]
        public void EncryptDecryptAes_Decrypt_Bytes()
        {
            byte[] key = new byte[16];
            Random rnd = new();
            rnd.NextBytes(key);

            var original = Encoding.UTF8.GetBytes(nameof(EncryptDecryptAes_Decrypt_Bytes));
            var encrypted = Cryptography.EncryptAes(original, key);

            Assert.NotEmpty(encrypted);

            var decrypted = Cryptography.DecryptAes(encrypted, key);

            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void EncryptDecryptAes_Decrypt_PassPhrase()
        {
            string password = "short";

            var original = Encoding.UTF8.GetBytes(nameof(EncryptDecryptAes_Decrypt_PassPhrase));
            var encrypted = Cryptography.EncryptAes(original, password);

            Assert.NotEmpty(encrypted);

            var decrypted = Cryptography.DecryptAes(encrypted, password);

            Assert.Equal(original, decrypted);
        }
    }
}

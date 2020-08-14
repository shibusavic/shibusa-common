using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class CryptographyTests
    {
        [Fact]
        public void HashAndVerify_Success()
        {
            var content = File.ReadAllText("CryptoTest.txt");
            var algorithm = HashAlgorithm.Create("SHA256");
            var hash = Cryptography.GetHash(algorithm, content);

            Assert.NotNull(hash);
            Assert.NotEmpty(hash);

            Assert.True(Cryptography.VerifyHash(algorithm, content, hash));
        }

        [Fact]
        public void HashAndVerify_Fail()
        {
            var content = File.ReadAllText("CryptoTest.txt");
            var algorithm = HashAlgorithm.Create("SHA256");
            var hash = Cryptography.GetHash(algorithm, content);

            var reverseContent = content.ToArray().Reverse().ToString();
            var hash2 = Cryptography.GetHash(algorithm, reverseContent);

            Assert.False(Cryptography.VerifyHash(algorithm, content, hash2));
            Assert.False(Cryptography.VerifyHash(algorithm, reverseContent, hash));
        }

        [Fact]
        public async Task HashAndVerify_TwoFilesAsync()
        {
            var tempFilename = Path.GetTempFileName();

            try
            {
                var algorithm = HashAlgorithm.Create("SHA256");

                var tempFile = File.Create(tempFilename);

                var fileInfo1 = new FileInfo("CryptoTest.txt");
                var fileInfo2 = new FileInfo(tempFilename);

                var content1 = File.ReadAllText(fileInfo1.FullName);

                // Make the second file identical except for an additional newline character.
                var buffer = Encoding.UTF8.GetBytes($"{content1}{Environment.NewLine}");
                await tempFile.WriteAsync(buffer, 0, buffer.Length);
                await tempFile.FlushAsync();
                tempFile.Close();

                var hash1 = Cryptography.GetHashForFile(algorithm, fileInfo1);
                var hash2 = Cryptography.GetHashForFile(algorithm, fileInfo2);

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
    }
}

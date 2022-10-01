using System.Text;
using Xunit;

namespace Shibusa.Extensions.UnitTests
{
    public class StreamExtensionTests
    {
        [Fact]
        public void WriteToStream()
        {
            string message = "hello world";
            var stream = new MemoryStream();
            stream.Write(message);
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(message, actual);
        }

        [Fact]
        public async Task WriteToStreamAsync()
        {
            string message = "hello world";
            var stream = new MemoryStream();
            await stream.WriteAsync(message);
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(message, actual);
        }

        [Fact]
        public void WriteEmptyStringToStream()
        {
            string message = "";
            var stream = new MemoryStream();
            stream.Write(message);
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(message, actual);
        }

        [Fact]
        public async Task WriteEmptyStringToStreamAsync()
        {
            string message = "";
            var stream = new MemoryStream();
            await stream.WriteAsync(message);
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(message, actual);
        }

        [Fact]
        public void WriteLineToStream()
        {
            string message = "hello world";
            string expected = $"{message}{Environment.NewLine}";
            var stream = new MemoryStream();
            stream.WriteLine(message);
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task WriteLineToStreamAsync()
        {
            string message = "hello world";
            string expected = $"{message}{Environment.NewLine}";
            var stream = new MemoryStream();
            await stream.WriteLineAsync(message);
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteEmptyLineToStream()
        {
            string expected = Environment.NewLine;
            var stream = new MemoryStream();
            stream.WriteLine();
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task WriteEmptyLineToStreamAsync()
        {
            string expected = Environment.NewLine;
            var stream = new MemoryStream();
            await stream.WriteLineAsync();
            stream.Close();
            var buffer = stream.ToArray();
            var actual = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Assert.Equal(expected, actual);
        }
    }
}

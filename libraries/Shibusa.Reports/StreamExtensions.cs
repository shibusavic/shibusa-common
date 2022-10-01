using System.Text;

namespace Shibusa.Reports
{
    /// <summary>
    /// Extensions for <see cref="Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Write the specified message to the stream.
        /// </summary>
        /// <param name="stream">The stream to which to write.</param>
        /// <param name="message">The message to write.</param>
        /// <exception cref="ArgumentException">Thrown if the stream is not in a writeable state</exception>
        public static void Write(this Stream stream, string message)
        {
            if (!stream.CanWrite) { throw new ArgumentException($"Stream is not in a writable state."); }

            if (message != null)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);

                lock (stream)
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// Write the specified message to the stream as a line (adds a newline character).
        /// </summary>
        /// <param name="stream">The stream to which to write.</param>
        /// <param name="message">The message to write.</param>
        /// <exception cref="ArgumentException">Thrown if the stream is not in a writeable state</exception>
        public static void WriteLine(this Stream stream, string? message = null) =>
            stream.Write($"{message ?? string.Empty}{Environment.NewLine}");
    }
}

using System.Text;

namespace Shibusa.Extensions;

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
    public static void Write(this Stream stream, string message)
    {
        if (stream.CanWrite && message != null)
        {
            ReadOnlySpan<byte> buffer = Encoding.UTF8.GetBytes(message);

            lock (stream)
            {
                stream.Write(buffer);
            }
        }
    }

    /// <summary>
    /// Write the specified message to the stream.
    /// </summary>
    /// <param name="stream">The stream to which to write.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Task representing the underlying action.</returns>
    public static async Task WriteAsync(this Stream stream, string message,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (stream.CanWrite && message != null && !cancellationToken.IsCancellationRequested)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            await stream.WriteAsync(buffer, cancellationToken);
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

    /// <summary>
    /// Write the specified message to the stream as a line (adds a newline character).
    /// </summary>
    /// <param name="stream">The stream to which to write.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Task representing the underlying action.</returns>
    public static async Task WriteLineAsync(this Stream stream, string? message = null,
        CancellationToken cancellationToken = default) =>
        await WriteAsync(stream, $"{message ?? string.Empty}{Environment.NewLine}", cancellationToken);
}

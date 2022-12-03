namespace Shibusa.Reports
{
    /// <summary>
    /// Represents a delimited, file-based report.
    /// </summary>
    public class DelimitedFileReport : FileReport
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DelimitedFileReport"/> class.
        /// </summary>
        /// <param name="configuration">An optional instance of <see cref="ReportConfiguration"/>.</param>
        public DelimitedFileReport(ReportConfiguration? configuration = null) : base(configuration) { }

        /// <summary>
        /// Write the report to the provided stream.
        /// </summary>
        /// <param name="stream">The stream to which to write.</param>
        /// <returns>A task representing the asyncronous operation.</returns>
        public override async Task SaveAsync(Stream stream)
        {
            if (stream?.CanWrite ?? false)
            {
                if (Headers.Any())
                {
                    await WriteLineToStreamAsync(stream, string.Join(Configuration.Delimiter, Headers));
                }

                foreach (IDictionary<string, string> line in Lines)
                {
                    List<string> lineItems = new();

                    foreach (var header in Headers)
                    {
                        if (line.ContainsKey(header))
                        {
                            lineItems.Add(line[header]);
                        }
                        else
                        {
                            lineItems.Add(string.Empty);
                        }
                    }
                    await WriteLineToStreamAsync(stream, string.Join(Configuration.Delimiter, lineItems));
                }

                await stream.FlushAsync();
            }
        }

        /// <summary>
        /// Write the report to the provided file path and name.
        /// </summary>
        /// <param name="filename">The name of the target file.</param>
        /// <param name="overwrite">An indicator of whether to overwrite the target file.</param>
        /// <returns>A task representing the asyncronous operation.</returns>
        public override async Task SaveAsync(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) { throw new ArgumentNullException(nameof(filename)); }

            if (File.Exists(filename) && !Configuration.OverwriteOnSave)
            {
                throw new ArgumentException($"File exists and overwrite is set to FALSE. " +
                    $"Set {nameof(ReportConfiguration)}.{nameof(ReportConfiguration.OverwriteOnSave)} " +
                    $"to TRUE to overwrite existing files. See inner exception for details.",
                    new Exception($"'{filename}' already exists."));
            }

            string? path = Path.GetDirectoryName(filename);

            if (path != null && !Directory.Exists(path)) { Directory.CreateDirectory(path); }

            using var stream = File.Create(filename);

            await SaveAsync(stream);

            stream.Close();
        }

        /// <summary>
        /// Add a line to the report.
        /// </summary>
        /// <param name="lineItems">The dictionary of line items on the line.
        /// Keys are headers.</param>
        /// <param name="index">The index into which to insert the line.
        /// The default (-1) adds the line to the end of the collection.</param>
        public override void AddLine(IDictionary<string, string> lineItems, int index = -1)
        {
            if (lineItems?.Any() ?? false)
            {
                if (Configuration.DelimiterInContentAction != DelimiterInContentActions.DoNothing)
                {
                    IDictionary<string, string> copy = CreateLineDictionary(lineItems);

                    foreach (var kvp in copy)
                    {
                        if (kvp.Key.Contains(Configuration.Delimiter) ||
                            kvp.Value.Contains(Configuration.Delimiter))
                        {
                            switch (Configuration.DelimiterInContentAction)
                            {
                                case DelimiterInContentActions.Crash:
                                    throw new Exception($"The delimiter '{Configuration.Delimiter}' was found in either '{kvp.Key}' or '{kvp.Value}'");
                                case DelimiterInContentActions.Remove:
                                    lineItems.Remove(kvp.Key);
                                    lineItems.Add(kvp.Key.Replace(Configuration.Delimiter, string.Empty),
                                        kvp.Value.Replace(Configuration.Delimiter, string.Empty));
                                    break;
                                case DelimiterInContentActions.Replace:
                                    lineItems.Remove(kvp.Key);
                                    lineItems.Add(kvp.Key.Replace(Configuration.Delimiter, Configuration.DelimiterInContentReplacement ?? ""),
                                        kvp.Value.Replace(Configuration.Delimiter, Configuration.DelimiterInContentReplacement ?? ""));
                                    break;
                                default:
                                    throw new Exception($"Unknown delimiter-in-content action: {Configuration.DelimiterInContentAction}");
                            }
                        }
                    }
                }

                base.AddLine(lineItems);
            }
        }
    }
}

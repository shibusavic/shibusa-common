using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shibusa.Reports
{
    /// <summary>
    /// Represents a generic report.
    /// </summary>
    public abstract class Report
    {
        readonly IList<IDictionary<string, string>> linesCollection;
        private readonly IList<string> headersCollection;

        protected IDictionary<string, int> columnWidths;

        protected Report(ReportConfiguration configuration = null)
        {
            Configuration = configuration ?? new ReportConfiguration();
            linesCollection = new Collection<IDictionary<string, string>>();
            headersCollection = new Collection<string>();
            columnWidths = Configuration.HeadersAreCaseSensitive
                ? new Dictionary<string, int>()
                : new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the <see cref="ReportConfiguration"/> instance for this report.
        /// </summary>
        public virtual ReportConfiguration Configuration { get; }

        /// <summary>
        /// Gets the collection of headers for this report.
        /// </summary>
        public virtual IReadOnlyCollection<string> Headers =>
            new ReadOnlyCollection<string>(headersCollection.ToList());

        /// <summary>
        /// Gets the collection of lines for this report.
        /// </summary>
        public virtual IReadOnlyCollection<IDictionary<string, string>> Lines =>
            new ReadOnlyCollection<IDictionary<string, string>>(linesCollection.ToList());

        /// <summary>
        /// Add a header to the header collection.
        /// </summary>
        /// <param name="header">The header string.</param>
        /// <param name="index">The index into which to insert the header.
        /// The default (-1) places the header at the end of the collection.</param>
        public virtual void AddHeader(string header, int index = -1)
        {
            header ??= string.Empty;

            if (index < 0 || index >= headersCollection.Count)
            {
                if (!(Configuration.HeadersAreCaseSensitive ?
                    headersCollection.Contains(header) :
                    headersCollection.Contains(header, StringComparer.OrdinalIgnoreCase)))
                {
                    headersCollection.Add(header);
                    UpdateColumnLength(header, header.Length);
                }
            }
            else
            {
                headersCollection.Insert(index, header);
                UpdateColumnLength(header, header.Length);
            }
        }

        /// <summary>
        /// Add a collection of headers to the report.
        /// </summary>
        /// <param name="headers">The headers to add.</param>
        /// <param name="startingIndex">The starting index of where to insert the new collection.
        /// The default (-1) will add them to the end of the collection.</param>
        public virtual void AddHeaders(IEnumerable<string> headers, int startingIndex = -1)
        {
            headers ??= new List<string>();

            if (startingIndex < 0 || startingIndex >= headersCollection.Count)
            {
                foreach (var header in headers)
                {
                    if (!(Configuration.HeadersAreCaseSensitive ?
                        headersCollection.Contains(header) :
                        headersCollection.Contains(header, StringComparer.OrdinalIgnoreCase)))
                    {
                        headersCollection.Add(header);
                    }
                }
            }
            else
            {
                headersCollection.ToList().InsertRange(startingIndex, headers);
                headers.ToList().ForEach(h => UpdateColumnLength(h, h.Length));
            }
        }

        /// <summary>
        /// Remove a header.
        /// </summary>
        /// <param name="header">The header to remove.</param>
        public virtual void RemoveHeader(string header) => headersCollection.Remove(header);

        /// <summary>
        /// Remove a collection of headers.
        /// </summary>
        /// <param name="headers">The headers to remove. The headers require no special order.</param>
        public virtual void RemoveHeaders(IEnumerable<string> headers)
        {
            List<string> matchingHeaders = new List<string>(headersCollection.Where(h => headers.Contains(h)));
            matchingHeaders.ForEach(h => RemoveHeader(h));
        }

        /// <summary>
        /// Add a line to the report.
        /// </summary>
        /// <param name="lineItems">The dictionary of line items forming the line.
        /// Keys are headers.</param>
        /// <param name="index">The index into which to insert the line. The default (-1)
        /// adds to the end of the collection.</param>
        public virtual void AddLine(IDictionary<string, string> lineItems, int index = -1)
        {
            if (lineItems?.Any() ?? false)
            {
                IDictionary<string, string> createdLine = CreateLineDictionary();

                var headers = lineItems.Keys;

                List<string> newHeaders = Configuration.HeadersAreCaseSensitive
                    ? headers.Except(headersCollection).ToList()
                    : headers.Except(headersCollection, StringComparer.OrdinalIgnoreCase).ToList();

                if (newHeaders.Any())
                {
                    if (Configuration.AddDiscoveredHeaders)
                    {
                        AddHeaders(newHeaders);
                    }
                }

                foreach (var item in lineItems)
                {
                    if ((Configuration.HeadersAreCaseSensitive && Headers.Contains(item.Key)) ||
                        (!Configuration.HeadersAreCaseSensitive && Headers.Contains(item.Key, StringComparer.OrdinalIgnoreCase)))
                    {
                        createdLine.Add(item.Key, item.Value);
                        UpdateColumnLength(item.Key, item.Value.Length);
                    }
                    else if (!Configuration.IgnoreExtraLineValues)
                    {
                        throw new Exception($"A value for an unknown header, '{item.Key}' was found.");
                    }
                }

                if (index < 0 || index > -linesCollection.Count)
                {
                    linesCollection.Add(createdLine);
                }
                else
                {
                    linesCollection.Insert(index, createdLine);
                }
            }
        }

        /// <summary>
        /// Add a line to the end of the report.
        /// </summary>
        /// <param name="lineItems">A params array of a (header,value) tuples.</param>
        /// <remarks>To insert a line somewhere specific in the collection, use the <see cref="AddLine(IDictionary{string, string}, int)"/> method.</remarks>
        public virtual void AddLine(params (string Header, string Value)[] lineItems)
        {
            if (lineItems?.Any() ?? false)
            {
                IDictionary<string, string> dictionary = CreateLineDictionary();

                foreach ((string Header, string Value) in lineItems)
                {
                    if (dictionary.ContainsKey(Header))
                    {
                        dictionary[Header] = Value;
                    }
                    else
                    {
                        dictionary.Add(Header, Value);
                    }
                }

                AddLine(dictionary);
            }
        }

        /// <summary>
        /// Remove the line at the specified index.
        /// </summary>
        /// <param name="index">The index of the line to remove.</param>
        public virtual void RemoveLine(int index)
        {
            if (index >= 0 && index < Lines.Count)
            {
                linesCollection.Remove(Lines.ElementAt(index));
            }
        }

        /// <summary>
        /// Creates an empty line, which is a dictionary in which the keys have been populated with the
        /// <see cref="Headers"/> entries, but the values are empty strings.
        /// </summary>
        /// <returns>A dictionary in which the keys represent headers and the values represent line item entries for those headers.</returns>
        public virtual IDictionary<string, string> CreateEmptyLine()
        {
            var result = CreateLineDictionary();

            foreach (string header in Headers)
            {
                result.Add(header, string.Empty);
            }
            return result;
        }

        /// <summary>
        /// Creates a line by extracting from the provided key/value collection all of the entries in which the key
        /// can be found in the <see cref="Headers"/> collection.
        /// </summary>
        /// <param name="lineItems">The collection of key/value pairs representing a line on the report.</param>
        /// <returns>A dictionary in which the keys represent headers and the values represent line item entries for those headers.</returns>
        public virtual IDictionary<string, string> CreateLine(IEnumerable<KeyValuePair<string, string>> lineItems)
        {
            var result = CreateEmptyLine();
            foreach (var kvp in lineItems)
            {
                if (result.ContainsKey(kvp.Key))
                {
                    result[kvp.Key] = kvp.Value;
                }
                else
                {
                    if (Configuration.AddDiscoveredHeaders)
                    {
                        AddHeader(kvp.Key);
                        result.Add(kvp.Key, kvp.Value);
                        UpdateColumnLength(kvp.Key, kvp.Value.Length);
                    }
                    else
                    {
                        throw new Exception($"A previously unknown header '{kvp.Key}' found in " +
                             $"{nameof(CreateLine)}. To dynamically add headers, use the " +
                             $"{nameof(ReportConfiguration.AddDiscoveredHeaders)} configuration setting.");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the header as a string.
        /// </summary>
        /// <returns>The delimited header as a string.</returns>
        public virtual string GetHeaderAsString()
        {
            List<string> headerItems = new List<string>();

            foreach (var header in Headers)
            {
                int maxLen = columnWidths[header];

                string headerItem = header.PadRight(maxLen);

                if (headerItem.Length > maxLen)
                {
                    headerItem = headerItem.Substring(0, maxLen);
                }
                headerItems.Add(headerItem.PadRight(maxLen));
            }

            return string.Join(Configuration.Delimiter, headerItems);
        }

        /// <summary>
        /// Gets the line at the specified index as a string.
        /// </summary>
        /// <param name="index">The zero-based, ordinal posiiton of the line to get.</param>
        /// <returns>A delimited line.</returns>
        public virtual string GetLineAsString(int index)
        {
            List<string> lineItems = new List<string>();

            foreach (var header in Headers)
            {
                int maxLen = columnWidths[header];

                string lineItem = Lines.ElementAt(index).ContainsKey(header) ? Lines.ElementAt(index)[header] : string.Empty;

                if (lineItem.Length > maxLen)
                {
                    lineItem = lineItem.Substring(0, maxLen);
                }
                lineItems.Add(lineItem.PadRight(maxLen));
            }

            return string.Join(Configuration.Delimiter, lineItems);
        }

        protected virtual void UpdateColumnLength(string header, int length)
        {
            if (columnWidths.ContainsKey(header))
            {
                if (length > columnWidths[header]) { columnWidths[header] = Math.Min(length, Configuration.MaxColumnLength); }
            }
            else
            {
                columnWidths.Add(header, Math.Min(length, Configuration.MaxColumnLength));
            }
        }

        protected virtual async Task WriteToStreamAsync(Stream stream, string text)
        {
            if (stream?.CanWrite ?? false)
            {
                var buffer = Encoding.UTF8.GetBytes(text);
                await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
        }

        protected virtual async Task WriteLineToStreamAsync(Stream stream, string text)
            => await WriteToStreamAsync(stream, $"{text}{Environment.NewLine}").ConfigureAwait(false);

        protected virtual IEnumerable<string> ExtractValuesFromLine(IDictionary<string, string> line)
        {
            IList<string> values = new List<string>();
            foreach (var header in Headers)
            {
                values.Add(line.ContainsKey(header) ? line[header] : string.Empty);
            }
            return values;
        }

        protected virtual IDictionary<string, string> CreateLineDictionary() =>
            Configuration.HeadersAreCaseSensitive
                ? new Dictionary<string, string>()
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        protected virtual IDictionary<string, string> CreateLineDictionary(IDictionary<string, string> dictionary) =>
            Configuration.HeadersAreCaseSensitive
                ? new Dictionary<string, string>(dictionary)
                : new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
    }
}

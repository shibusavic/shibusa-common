using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Shibusa.Reports.UnitTests
{
    /// <summary>
    /// I felt like this was a decent test of both <see cref="ConsoleReport"/> and <see cref="Report"/>.
    /// </summary>
    public class ConsoleReportTests
    {
        const string pipeDelimiter = " | ";

        [Fact]
        public void SetHeaders_Succeeds()
        {
            string[] headers = new string[] { "A", "B", "C" };

            string expectedHeaderString = string.Join(pipeDelimiter, headers);

            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false
            });

            headers.ToList().ForEach(h => report.AddHeader(h));

            Assert.Equal(headers.Length, report.Headers.Count);

            Assert.Equal(expectedHeaderString, report.GetHeaderAsString());

        }

        [Fact]
        public void DiscoverHeaders_Succeeds()
        {
            string[] expectedHeaders = new string[] { "A", "B", "C" };

            string expectedHeaderString = string.Join(pipeDelimiter, expectedHeaders);

            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false
            });

            report.AddLine(new Dictionary<string, string>()
            {
                { expectedHeaders[0], "a"},
                { expectedHeaders[1], "b"},
                { expectedHeaders[2], "c"},
            });

            Assert.Equal(expectedHeaders.Length, report.Headers.Count);

            Assert.Equal(expectedHeaderString, report.GetHeaderAsString());
        }

        [Fact]
        public void ExtraLineItemValues_Ignored()
        {
            string[] expectedHeaders = new string[] { "A", "B", "C" };
            string[] expectedLineItems = new string[] { "a", "b", "c" };

            string expectedHeaderString = string.Join(pipeDelimiter, expectedHeaders);
            string expectedLineString = string.Join(pipeDelimiter, expectedLineItems);

            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false
            });

            expectedHeaders.ToList().ForEach(h => report.AddHeader(h));

            report.AddLine(new Dictionary<string, string>()
            {
                { expectedHeaders[0],expectedLineItems[0]},
                { expectedHeaders[1],expectedLineItems[1]},
                { expectedHeaders[2], expectedLineItems[2]},
                { "Unexpected Header", "d"}
            });

            Assert.Equal(expectedHeaders.Length, report.Headers.Count);

            Assert.Equal(expectedHeaderString, report.GetHeaderAsString());

            Assert.Equal(expectedLineString, report.GetLineAsString(0));
        }

        [Fact]
        public void JaggedData()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false
            });

            report.AddLine(("A", "a"), ("B", "b"));
            report.AddLine(("C", "c"), ("D", "d"));

            string expectedHeader = string.Join(report.Configuration.Delimiter, new List<string>() { "A", "B", "C", "D" });
            string expectedLine1 = string.Join(report.Configuration.Delimiter, new List<string>() { "a", "b", " ", " " });
            string expectedLine2 = string.Join(report.Configuration.Delimiter, new List<string>() { " ", " ", "c", "d" });

            Assert.Equal(4, report.Headers.Count);
            Assert.Equal(expectedHeader, report.GetHeaderAsString());
            Assert.Equal(expectedLine1, report.GetLineAsString(0));
            Assert.Equal(expectedLine2, report.GetLineAsString(1));
        }

        [Fact]
        public void IgnoreExtraLineValues_False_ThrowsOnExtraValue()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = false,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false
            });

            report.AddHeaders(new List<string>() { "A", "B" });

            Assert.Throws<Exception>(() => report.AddLine(("A", "a"), ("B", "b"), ("C", "c")));
        }

        [Fact]
        public void HeadersAreCaseSensitive_True_NoMatch()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = false,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = true
            });

            report.AddHeaders(new List<string>() { "A", "B" });

            Assert.Throws<Exception>(() =>
                report.AddLine(new Dictionary<string, string>()
                {
                    { "a", "alpha"},
                    { "b", "beta"}
                }));
        }

        [Fact]
        public void HeadersAreCaseSensitive_False_Match()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = false,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false
            });

            report.AddHeaders(new List<string>() { "A", "B" });

            report.AddLine(new Dictionary<string, string>()
            {
                { "a", "alpha"},
                { "b", "beta"}
            });

            string expectedHeader = string.Join(report.Configuration.Delimiter, new List<string>() { "A    ", "B   " });
            string expectedLine1 = string.Join(report.Configuration.Delimiter, new List<string>() { "alpha", "beta" });

            Assert.Equal(expectedHeader, report.GetHeaderAsString());
            Assert.Equal(expectedLine1, report.GetLineAsString(0));
        }

        [Fact]
        public void MaxLength_Truncates_Match()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false,
                MaxColumnLength = 5
            });

            report.AddHeaders(new List<string>() { "A", "B" });

            report.AddLine(new Dictionary<string, string>()
            {
                { "a", "alphaOne"},
                { "b", "betaTwo"}
            });

            string expectedHeader = string.Join(report.Configuration.Delimiter, new List<string>() { "A    ", "B    " });
            string expectedLine1 = string.Join(report.Configuration.Delimiter, new List<string>() { "alpha", "betaT" });

            Assert.Equal(expectedHeader, report.GetHeaderAsString());
            Assert.Equal(expectedLine1, report.GetLineAsString(0));
        }

        [Fact]
        public void RemoveHeader_Removes()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false,
                MaxColumnLength = 5
            });

            report.AddHeaders(new List<string>() { "A", "B", "C" });

            Assert.Equal(3, report.Headers.Count);

            report.RemoveHeader("B");

            Assert.Equal(2, report.Headers.Count);
            Assert.Equal("A", report.Headers.First());
            Assert.Equal("C", report.Headers.Last());
        }

        [Fact]
        public void RemoveHeaders_Removes()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false,
                MaxColumnLength = 5
            });

            report.AddHeaders(new List<string>() { "A", "B", "C" });

            Assert.Equal(3, report.Headers.Count);

            report.RemoveHeaders(new List<string>() { "B", "C" });

            Assert.Equal(1, report.Headers.Count);
            Assert.Equal("A", report.Headers.First());
        }

        [Fact]
        public void RemoveLine_Removes()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                IgnoreExtraLineValues = false,
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false,
                MaxColumnLength = 5
            });

            report.AddHeaders(new List<string>() { "A" });

            report.AddLine(("A", "a"));
            report.AddLine(("A", "1"));
            report.AddLine(("A", "3"));

            Assert.Equal(3, report.Lines.Count);

            report.RemoveLine(1);

            Assert.Equal(2, report.Lines.Count);

            Assert.Equal("a", report.Lines.First()["A"]);
            Assert.Equal("3", report.Lines.Last()["A"]);

        }

        [Fact]
        public void InsertHeader_Index0_InsertAtFront()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false,
            });

            report.AddHeaders(new List<string>() { "B", "C" });

            report.AddHeader("A", 0);

            Assert.Equal("A", report.Headers.First());
            Assert.Equal("C", report.Headers.Last());
        }

        [Fact]
        public void InsertHeader_IndexOutOfBounds_InsertAtEnd()
        {
            var report = new ConsoleReport(new ReportConfiguration()
            {
                Delimiter = pipeDelimiter,
                HeadersAreCaseSensitive = false,
            });

            report.AddHeaders(new List<string>() { "A", "B" });

            report.AddHeader("C", 5);

            Assert.Equal("A", report.Headers.First());
            Assert.Equal("C", report.Headers.Last());
        }

    }
}

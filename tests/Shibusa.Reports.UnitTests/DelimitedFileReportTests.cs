using Xunit;

namespace Shibusa.Reports.UnitTests
{
    public class DelimitedFileReportTests
    {
        [Fact]
        public async Task FileSavedProperlyAsync()
        {
            DelimitedFileReport? report = new(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = ","
            });

            report.AddLine(("A", "a"), ("B", "b"));

            string expectedHeader = "A,B";
            string expectedLine = "a,b";

            string? fileName = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.csv");

            await report.SaveAsync(fileName);

            IEnumerable<string> fromFileLines = File.ReadLines(fileName);

            Assert.Equal(expectedHeader, fromFileLines.ElementAt(0));
            Assert.Equal(expectedLine, fromFileLines.ElementAt(1));
        }

        [Fact]
        public async Task FileOverwritesAsync()
        {
            DelimitedFileReport? report = new(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = ",",
                OverwriteOnSave = true
            });

            report.AddLine(("A", "a"), ("B", "b"));

            string expectedHeader = "A,B,C";
            string expectedLine1 = "a,b,";
            string expectedLine2 = ",,c";

            string? fileName = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.csv");

            await report.SaveAsync(fileName);

            report.AddLine(("C", "c"));

            await report.SaveAsync(fileName);

            IEnumerable<string> fromFileLines = File.ReadLines(fileName);

            Assert.Equal(expectedHeader, fromFileLines.ElementAt(0));
            Assert.Equal(expectedLine1, fromFileLines.ElementAt(1));
            Assert.Equal(expectedLine2, fromFileLines.ElementAt(2));
        }

        [Fact]
        public void DelimiterInContent_Crash_Throws()
        {
            DelimitedFileReport? report = new(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = ",",
                DelimiterInContentAction = DelimiterInContentActions.Crash
            });

            Assert.Throws<Exception>(() => report.AddLine(("A,", "a"), ("B", "b")));
        }

        [Fact]
        public async Task DelimiterInContent_Replace_ReplacesAsync()
        {
            DelimitedFileReport? report = new(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = ",",
                DelimiterInContentAction = DelimiterInContentActions.Replace,
                DelimiterInContentReplacement = "~"
            });

            report.AddLine(("A,", "a"), ("B", "b"));

            string expectedHeader = "A~,B";
            string expectedLine = "a,b";

            string? fileName = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.csv");

            await report.SaveAsync(fileName);

            IEnumerable<string> fromFileLines = File.ReadLines(fileName);

            Assert.Equal(expectedHeader, fromFileLines.ElementAt(0));
            Assert.Equal(expectedLine, fromFileLines.ElementAt(1));

        }

        [Fact]
        public async Task DelimiterInContent_Remove_ReplacesAsync()
        {
            DelimitedFileReport? report = new(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = ",",
                DelimiterInContentAction = DelimiterInContentActions.Remove,
            });

            report.AddLine(("A,", "a"), ("B", "b"));

            string expectedHeader = "A,B";
            string expectedLine = "a,b";

            string? fileName = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.csv");

            await report.SaveAsync(fileName);

            IEnumerable<string> fromFileLines = File.ReadLines(fileName);

            Assert.Equal(expectedHeader, fromFileLines.ElementAt(0));
            Assert.Equal(expectedLine, fromFileLines.ElementAt(1));

        }

        [Fact]
        public async Task DelimiterInContent_DoNothing_ReplacesAsync()
        {
            DelimitedFileReport? report = new(new ReportConfiguration()
            {
                AddDiscoveredHeaders = true,
                Delimiter = ",",
                DelimiterInContentAction = DelimiterInContentActions.DoNothing,
                DelimiterInContentReplacement = "~"
            });

            report.AddLine(("A,", "a"), ("B", "b"));

            string expectedHeader = "A,,B";
            string expectedLine = "a,b";

            string? fileName = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.csv");

            await report.SaveAsync(fileName);

            IEnumerable<string> fromFileLines = File.ReadLines(fileName);

            Assert.Equal(expectedHeader, fromFileLines.ElementAt(0));
            Assert.Equal(expectedLine, fromFileLines.ElementAt(1));
        }
    }
}

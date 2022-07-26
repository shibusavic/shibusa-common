namespace Shibusa.Reports
{
    /// <summary>
    /// Represents a report targeting the console.
    /// </summary>
    public class ConsoleReport : Report
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ConsoleReport"/> class.
        /// </summary>
        /// <param name="configuration">An instance of <see cref="ReportConfiguration"/> informing
        /// <see cref="ConsoleReport"/> how to behave.</param>
        public ConsoleReport(ReportConfiguration? configuration = null) : base(configuration)
        { }

        /// <summary>
        /// Gets a horizontal line the length of the headers.
        /// </summary>
        /// <returns></returns>
        public virtual string GetHorizontalLine() => "".PadRight(GetHeaderAsString().Length, '-');
    }
}

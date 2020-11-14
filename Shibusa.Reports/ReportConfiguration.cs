namespace Shibusa.Reports
{
    /// <summary>
    /// Represents a report's configuration. Not all values are used each type of report.
    /// </summary>
    public class ReportConfiguration
    {
        /// <summary>
        /// Gets or sets the report's delimiter.
        /// </summary>
        public string Delimiter { get; set; } = ",";

        /// <summary>
        /// Gets or sets an indicator of whether adding lines should add previously unknown headers
        /// to the <see cref="Report.Headers"/> collection. Default value is false.
        /// </summary>
        public bool AddDiscoveredHeaders { get; set; } = false;

        /// <summary>
        /// Gets or sets a value to indicator what the report should do with delimiters found inside
        /// line content. Default value is to throw an exception.
        /// </summary>
        public DelimiterInContentActions DelimiterInContentAction { get; set; } = DelimiterInContentActions.Crash;

        /// <summary>
        /// Gets or sets the string replacement for delimiters found within line item content. This is only applicable
        /// when <see cref="DelimiterInContentAction"/> is set to <see cref="DelimiterInContentActions.Replace"/>.
        /// Default value is '~'.
        /// </summary>
        public string DelimiterInContentReplacement { get; set; } = "~";

        /// <summary>
        /// Gets or sets an indicator of whether extra line values  Default value is true.
        /// </summary>
        public bool IgnoreExtraLineValues { get; set; } = true;

        /// <summary>
        /// Gets or sets a maximum column length. The default is 100.
        /// </summary>
        public int MaxColumnLength { get; set; } = 100;

        /// <summary>
        /// Gets or sets an indicator of whether headers are case sensitive. The affects both header discovering
        /// and matching.
        /// </summary>
        public bool HeadersAreCaseSensitive { get; set; } = false;

        /// <summary>
        /// Gets or sets an indicator of whether to overwrite files when saving. This only applies to file-based
        /// reports, obviously.
        /// </summary>
        public bool OverwriteOnSave { get; set; } = false;

        // TODO: maybe CenterHeaders ??

        /// <summary>
        /// Gets the configuration for Excel report.
        /// </summary>
        public ExcelConfiguration Excel { get; private set; } = new ExcelConfiguration();

        /// <summary>
        /// Represents Excel configuration. This is a WIP.
        /// </summary>
        public class ExcelConfiguration
        {

        }
    }

    /// <summary>
    /// Represents what action to take when discovering the delimiter inside a line's content.
    /// </summary>
    public enum DelimiterInContentActions
    {
        /// <summary>
        /// Ignore
        /// </summary>
        DoNothing = 0,
        /// <summary>
        /// Throw an Exception.
        /// </summary>
        Crash,
        /// <summary>
        /// Replace the value with <see cref="ReportConfiguration.DelimiterInContentReplacement"/>.
        /// </summary>
        Replace,
        /// <summary>
        /// Replace the delimiter (within the content) with an empty string.
        /// </summary>
        Remove
    }
}

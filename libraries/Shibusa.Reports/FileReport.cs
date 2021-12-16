using System.IO;
using System.Threading.Tasks;

namespace Shibusa.Reports
{
    /// <summary>
    /// Represents a file-based report.
    /// </summary>
    public abstract class FileReport : Report
    {
        protected FileReport(ReportConfiguration? configuration = null) : base(configuration)
        { }

        /// <summary>
        /// Write the report to the provided stream.
        /// </summary>
        /// <param name="stream">The stream to which to write.</param>
        /// <returns>A task representing the asyncronous operation.</returns>
        public abstract Task SaveAsync(Stream stream);

        /// <summary>
        /// Write the report to the provided file path and name.
        /// </summary>
        /// <param name="filename">The name of the target file.</param>
        /// <returns>A task representing the asyncronous operation.</returns>
        public abstract Task SaveAsync(string filename);
    }
}

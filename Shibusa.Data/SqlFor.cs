using Shibusa.Data.Abstractions;
using System;
using System.Text;

namespace Shibusa.Data
{
    /// <summary>
    /// Factory for constructing T-SQL FOR clauses for temporal table queries.
    /// </summary>
    public static class SqlFor
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss.fffffff";

        /// <summary>
        /// Constructs a T-SQL FOR statement.
        /// </summary>
        /// <param name="comparison">The comparison type to apply.</param>
        /// <param name="start">The first date in a date range, if applicable.</param>
        /// <param name="end">The second date in a date range, if applicable.</param>
        /// <param name="includeForPrefix">If true, the resulting string is prefixed with the 'FOR' keyword.</param>
        /// <returns>A valid SQL Server FOR clause for use with temporal tables.</returns>
        /// <seealso cref="https://docs.microsoft.com/en-us/sql/relational-databases/tables/querying-data-in-a-system-versioned-temporal-table?view=sql-server-2017"/>
        public static string Create(TemporalComparison comparison = TemporalComparison.All, DateTime? start = null, DateTime? end = null, bool includeForPrefix = true)
        {
            var result = new StringBuilder();

            if (includeForPrefix) { result.Append("FOR "); }

            result.Append("SYSTEM_TIME ");

            switch (comparison)
            {
                case TemporalComparison.All:
                    result.Append("ALL");
                    break;
                case TemporalComparison.AsOf:
                    if (!start.HasValue) { throw new ArgumentNullException(nameof(start)); }
                    result.Append($"AS OF '{start.Value.ToString(DateFormat)}'");
                    break;
                case TemporalComparison.Between:
                    if (!start.HasValue) { throw new ArgumentNullException(nameof(start)); }
                    if (!end.HasValue) { throw new ArgumentNullException(nameof(end)); }
                    result.Append($"BETWEEN '{start.Value.ToString(DateFormat)}' AND '{end.Value.ToString(DateFormat)}'");
                    break;
                case TemporalComparison.ContainedIn:
                    if (!start.HasValue) { throw new ArgumentNullException(nameof(start)); }
                    if (!end.HasValue) { throw new ArgumentNullException(nameof(end)); }
                    result.Append($"CONTAINED IN ('{start.Value.ToString(DateFormat)}','{end.Value.ToString(DateFormat)}')");
                    break;
                case TemporalComparison.FromTo:
                    if (!start.HasValue) { throw new ArgumentNullException(nameof(start)); }
                    if (!end.HasValue) { throw new ArgumentNullException(nameof(end)); }
                    result.Append($"FROM '{start.Value.ToString(DateFormat)}' TO '{end.Value.ToString(DateFormat)}'");
                    break;
                default:
                    throw new ArgumentException($"Unknown temporal comparison type: {comparison}");
            }

            return result.ToString();
        }
    }
}

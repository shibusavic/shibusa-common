using Shibusa.Data.Abstractions;

namespace Shibusa.Data
{
    /// <summary>
    /// A factory for SQL "ORDER BY" clauses.
    /// </summary>
    public static class SqlOrderBy
    {
        /// <summary>
        /// Creates a single column "ORDER BY" clause.
        /// </summary>
        /// <param name="column">The <see cref="KeyValuePair{TKey, TValue}"/> representing the name of the column
        /// (the Key) and the <see cref="SortOrder"/>, the direction by which to sort the column.</param>
        /// <returns>A T-SQL-friendly string containing the column and sort direction.
        /// Note that the "ORDER BY" clause is not inclued in the output.</returns>
        /// <remarks>Note that a <see cref="SortOrder"/> value of <see cref="SortOrder.Unspecified"/>
        /// will be converted to <see cref="SortOrder.Ascending"/>.</remarks>
        public static string Create(KeyValuePair<string, SortOrder> column)
        {
            return $"{column.Key} {ConvertSortOrderToString(column.Value)}";
        }

        /// <summary>
        /// Creates a multiple-column "ORDER BY" clause.
        /// </summary>
        /// <param name="columns">An <see cref="IDictionary{TKey, TValue}"/> of column names (Keys)
        /// and sort directions (Values). Each <see cref="KeyValuePair{TKey, TValue}"/> results in
        /// a column and sort direction being produced in the output. Note that
        /// the columns are sorted in the order in which they appear in the dictionary.</param>
        /// <returns>A T-SQL-friendly string containing the columns and sort directions for
        /// each <see cref="KeyValuePair{TKey, TValue}"/> provided.
        /// <remarks>The "ORDER BY" text is not inclued in the output; instead, the result will
        /// look like "[LastName], [DateOfBirth] DESC" given the appropriate inputs.
        /// </remarks>
        /// </returns>
        /// <remarks>A <see cref="SortOrder"/> value of <see cref="SortOrder.Unspecified"/>
        /// will be converted to <see cref="SortOrder.Ascending"/>.</remarks>
        public static string Create(IDictionary<string, SortOrder> columns)
        {
            if (!columns.Any()) { return string.Empty; }

            List<string> sorts = new();
            foreach (var column in columns)
            {
                sorts.Add(Create(column));
            }
            return string.Join(", ", sorts);
        }

        private static string ConvertSortOrderToString(SortOrder sortOrder)
        {
            return sortOrder == SortOrder.Descending ? "DESC" : "ASC";
        }
    }
}

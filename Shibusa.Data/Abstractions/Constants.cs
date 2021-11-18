namespace Shibusa.Data.Abstractions
{
    /// <summary>
    /// Represents defaults for <see cref="CriteriaBase"/>.
    /// </summary>
    public static class CriteriaDefaults
    {
        /// <summary>
        /// The default page number for queries.
        /// </summary>
        public const int DefaultPageNumber = 1;

        /// <summary>
        /// The default number of records per page.
        /// </summary>
        public const int DefaultNumberPerPage = 50;

        /// <summary>
        /// The default maximum number of records to return per page.
        /// </summary>
        public const int DefaultMaxPerPage = 1000;

        /// <summary>
        /// The default <see cref="LogicalOperator"/> for query criterion conjunctions.
        /// </summary>
        public const LogicalOperator DefaultLogicalOperator = LogicalOperator.And;
    }

    /// <summary>
    /// Represents logical operators to apply to parts of a query.
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// Represents the 'AND' conjunction.
        /// </summary>
        And = 1,

        /// <summary>
        /// Represents the 'OR' conjunction.
        /// </summary>
        Or = 2,

        /// <summary>
        /// Represents the 'AND NOT' conjunction.
        /// </summary>
        AndNot = 3,

        /// <summary>
        /// Represents the 'OR NOT' conjunction.
        /// </summary>
        OrNot = 4
    }

    /// <summary>
    /// Represents comparisons for temporal queries.
    /// <seealso cref="https://docs.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-2017"/>
    /// </summary>
    public enum TemporalComparison
    {
        /// <summary>
        /// Represents the 'AS OF' comparison.
        /// </summary>
        AsOf = 0,

        /// <summary>
        /// Represents the 'FROM TO' comparison.
        /// </summary>
        FromTo,

        /// <summary>
        /// Represents the 'BETWEEN' comparison.
        /// </summary>
        Between,

        /// <summary>
        /// Represents the 'CONTAINED IN' comparison.
        /// </summary>
        ContainedIn,

        /// <summary>
        /// Represents the 'ALL' comparision.
        /// </summary>
        All
    }

    /// <summary>
    /// Represents generic comparison values.
    /// </summary>
    public enum Comparison
    {
        /// <summary>
        /// Represents either the '=' or 'IS' T-SQL operators.
        /// </summary>
        Equal = 0,

        /// <summary>
        /// Represents either the '&lt;&gt;' or 'IS NOT' operators.
        /// </summary>
        NotEqual,

        /// <summary>
        /// Represents the 'LIKE' operator.
        /// </summary>
        Like,

        /// <summary>
        /// Represents the case insensitive 'LIKE' operator.
        /// </summary>
        CiLike,

        /// <summary>
        /// Represents the 'NOT LIKE' operator.
        /// </summary>
        NotLike,

        /// <summary>
        /// Represents the case insensitive 'NOT LIKE' operator.
        /// </summary>
        NotCiLike,

        /// <summary>
        /// Represents the 'IN' operator.
        /// </summary>
        In,

        /// <summary>
        /// Represents the 'NOT IN' operator.
        /// </summary>
        NotIn,

        /// <summary>
        /// Represents the '&lt;' operator.
        /// </summary>
        LessThan,

        /// <summary>
        /// Represents the '&lt;=' operator.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Represents the '&gt;' operator.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Represents the '&gt;=' operator.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Represents the 'BETWEEN' operator.
        /// </summary>
        Between,

        /// <summary>
        /// Represents the 'NOT BETWEEN' operator.
        /// </summary>
        NotBetween
    }

    /// <summary>
    /// Represents three types of sorting: ascending, descending, and unspecified.
    /// </summary>
    /// <remarks>
    /// Borrowed this from System.Data.SqlClient, but didn't want to bring that entire
    /// namespace into this assembly for something so small and universal.
    /// </remarks>
    public enum SortOrder
    {
        /// <summary>
        /// No sort order is specified.
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// Rows are sorted in ascending order.
        /// </summary>
        Ascending = 0,

        /// <summary>
        /// Rows are sorted in descending order.
        /// </summary>
        Descending = 1
    }
}

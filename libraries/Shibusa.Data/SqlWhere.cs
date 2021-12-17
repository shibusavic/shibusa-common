using Shibusa.Data.Abstractions;
using System.Text;

namespace Shibusa.Data
{
    /// <summary>
    /// Represents a complete T-SQL WHERE clause. This class implements the Specification pattern to provide
    /// AND, OR, AND NOT, and OR NOT combinations. Clauses can be joined together in a variety of ways, including
    /// complex nested conditions.
    /// <seealso cref="https://en.wikipedia.org/wiki/Specification_pattern"/>
    /// </summary>
    public sealed class SqlWhere
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss.fffffff";

        /// <summary>
        /// Creates a new instance of the <see cref="SqlWhere"/> clause.
        /// </summary>
        public SqlWhere()
        {
            Raw = string.Empty;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SqlWhere"/> clause.
        /// </summary>
        /// <param name="where">The WHERE clause with which to prime the <see cref="Raw"/> property.</param>
        public SqlWhere(string where)
        {
            Raw = where ?? string.Empty;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SqlWhere"/> clause.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> object to clone.</param>
        public SqlWhere(SqlWhere where)
        {
            Raw = where?.Raw ?? string.Empty;
        }

        /// <summary>
        /// Gets the raw T-SQL string to attach to a "WHERE" statement. This value does NOT begin with the 'WHERE' statement.
        /// </summary>
        public string Raw { get; private set; }

        /// <summary>
        /// Creates a clone of the provided <see cref="SqlWhere"/> instance.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> instance to clone.</param>
        /// <returns>A <see cref="SqlWhere"/> instance.</returns>
        public static SqlWhere Where(SqlWhere where) => new(where);

        /// <summary>
        /// Creates a new <see cref="SqlWhere"/> instance that takes in an applicable string representing the WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause with which to prime the <see cref="Raw"/> property.</param>
        /// <returns>A <see cref="SqlWhere"/> instance</returns>
        public static SqlWhere Where(string where) => new(where);

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="SqlWhere"/> to the existing <see cref="Raw"/> with an AND conjunction.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> instance to attach to the existing WHERE clause.</param>
        /// <returns>A <see cref="SqlWhere"/> instance with the combined WHERE clause.</returns>
        public SqlWhere And(SqlWhere where) => string.IsNullOrWhiteSpace(Raw) ? new(where) : new($"({Raw} AND {where.Raw})");

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="SqlWhere"/> to the existing <see cref="Raw"/> with an OR conjunction.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> instance to attach to the existing WHERE clause.</param>
        /// <returns>A <see cref="SqlWhere"/> instance with the combined WHERE clause.</returns>
        public SqlWhere Or(SqlWhere where) => string.IsNullOrWhiteSpace(Raw) ? new(where) : new($"({Raw} OR {where.Raw})");

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="SqlWhere"/> to the existing <see cref="Raw"/> with an AND NOT conjunction.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> instance to attach to the existing WHERE clause.</param>
        /// <returns>A <see cref="SqlWhere"/> instance with the combined WHERE clause.</returns>
        public SqlWhere AndNot(SqlWhere where) => string.IsNullOrWhiteSpace(Raw) ? new(where) : new($"({Raw} AND NOT {where.Raw})");

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="SqlWhere"/> to the existing <see cref="Raw"/> with an OR NOT conjunction.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> instance to attach to the existing WHERE clause.</param>
        /// <returns>A <see cref="SqlWhere"/> instance with the combined WHERE clause.</returns>
        public SqlWhere OrNot(SqlWhere where) => string.IsNullOrWhiteSpace(Raw) ? new(where) : new($"({Raw} OR NOT {where.Raw})");

        /// <summary>
        /// Creates a <see cref="SqlWhere"/> instance that compares the left to the right using the <see cref="Comparison"/> value provided.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="comparison">The type of <see cref="Comparison"/> to be constructed.</param>
        /// <param name="right">A collection of type T to which the left should be compared.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance representing the comparison specified.</returns>
        public static SqlWhere Where<T>(string left, Comparison comparison, IEnumerable<T> right, bool makeFriendly = true)
        {
            if (string.IsNullOrWhiteSpace(left)) { throw new ArgumentNullException(nameof(left)); }
            if (!right.Any()) { throw new ArgumentException($"{right} cannot be empty."); }

            var whereClause = new StringBuilder();

            List<string> modifiedRight;
            if (makeFriendly)
            {
                modifiedRight = MakeSqlReady(right).ToList();
            }
            else
            {
                modifiedRight = new List<string>();
                right.ToList().ForEach(r => modifiedRight.Add(r?.ToString() ?? string.Empty));
            }

            whereClause.Append($"{left}");

            switch (comparison)
            {
                case Comparison.In:
                    whereClause.Append($" IN ({string.Join(",", modifiedRight)})");
                    break;
                case Comparison.NotIn:
                    whereClause.Append($" NOT IN ({string.Join(",", modifiedRight)})");
                    break;
                case Comparison.Equal:
                case Comparison.NotEqual:
                case Comparison.Like:
                case Comparison.CiLike:
                case Comparison.NotLike:
                case Comparison.NotCiLike:
                case Comparison.LessThan:
                case Comparison.LessThanOrEqual:
                case Comparison.GreaterThan:
                case Comparison.GreaterThanOrEqual:
                case Comparison.Between:
                case Comparison.NotBetween:
                    throw new Exception($"{comparison} is not supported for multiple value comparisons.");
                default:
                    throw new Exception($"Unknown comparison type ('{comparison}') in Where<T>(string left, Comparison comparison, T right, T right2 = default(T))");
            }

            return new SqlWhere(whereClause.ToString());
        }

        /// <summary>
        /// Creates an instance of <see cref="SqlWhere"/> that represents an equality comparison between left and right.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>An instance of <see cref="SqlWhere"/> that has raw SQL representing an equality check.</returns>
        public static SqlWhere WhereEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.Equal, right, default, makeFriendly);

        /// <summary>
        /// Creates an instance of <see cref="SqlWhere"/> that represents a lack of equality comparison between left and right.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>An instance of <see cref="SqlWhere"/> representing an inequality.</returns>
        public static SqlWhere WhereNotEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.NotEqual, right, default, makeFriendly);

        /// <summary>
        /// Creates an instance of <see cref="SqlWhere"/> that represents a left '&gt;' right comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> instance that has raw SQL representing
        /// a '&gt;' comparison. </returns>
        public static SqlWhere WhereGreaterThan<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.GreaterThan, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left '&gt;=' right comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> instance that has raw SQL representing
        /// a '&gt;=' comparison.</returns>
        public static SqlWhere WhereGreaterThanOrEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.GreaterThanOrEqual, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents a left '&lt;' right comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a '&lt;' comparison. </returns>
        public static SqlWhere WhereLessThan<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.LessThan, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents a left '&lt;=' right comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a '&lt;=' comparison. </returns>
        public static SqlWhere WhereLessThanOrEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.LessThanOrEqual, right, default, makeFriendly);

        /// <summary>
        /// Creates a <see cref="SqlWhere"/> instance that represents a check using LIKE to compare left and right.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a LIKE comparison. </returns>
        public static SqlWhere WhereLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.Like, right, default, makeFriendly);

        /// <summary>
        /// Creates a <see cref="SqlWhere" /> instance where the left is compared to the right using a case-insensitive LIKE match.
        /// </summary>
        /// <typeparam name="T">Any type, but typically a <see cref="string"/>.</typeparam>
        /// <param name="left">The value of the left side of the equation.</param>
        /// <param name="right">The instance to which to compare.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a CILIKE comparison. </returns>
        public static SqlWhere WhereCiLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.CiLike, right, default, makeFriendly);

        /// <summary>
        /// Creates a <see cref="SqlWhere"/> instance that represents a check using NOT LIKE to compare left and right.
        /// </summary>
        /// <typeparam name="T">Any type, but typically a <see cref="string"/>.</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a NOT LIKE comparison. </returns>
        public static SqlWhere WhereNotLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.NotLike, right, default, makeFriendly);

        /// <summary>
        /// Creates a <see cref="SqlWhere" /> instance where the left is compared to the right using a case-insensitive NOT LIKE match.
        /// </summary>
        /// <typeparam name="T">The type of instances in the right argument.</typeparam>
        /// <param name="left">The value of the left side of the equation.</param>
        /// <param name="right">The instance to which to compare.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a NOT CILIKE comparison.</returns>
        public static SqlWhere WhereNotCiLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.NotCiLike, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents a BETWEEN comparision.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The first boundary of the BETWEEN comparision.</param>
        /// <param name="right2">The second boundary of the BETWEEN comparison.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> and <paramref name="right2"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a BETWEEN comparison.</returns>
        public static SqlWhere WhereBetween<T>(string left, T right, T right2, bool makeFriendly = true)
            => Where(left, Comparison.Between, right, right2, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents a NOT BETWEEN comparision.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The first boundary of the NOT BETWEEN comparision.</param>
        /// <param name="right2">The second boundary of the NOT BETWEEN comparison.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> and <paramref name="right2"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing a NOT BETWEEN comparison.</returns>
        public static SqlWhere WhereNotBetween<T>(string left, T right, T right2, bool makeFriendly = true)
            => Where(left, Comparison.NotBetween, right, right2, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents either an IN or NOT IN comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="comparison">The type of comparison to be constructed. Only <see cref="Comparison.In"/>
        /// and <see cref="Comparison.NotIn"/> are allowed.
        /// <param name="right">The first boundary of the comparision.</param>
        /// <param name="right2">The second boundary of the comparison.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance that has raw SQL representing the comparison.</returns>
        public static SqlWhere Where<T>(string left, Comparison comparison, T right, T? right2 = default, bool makeFriendly = true)
        {
            if (string.IsNullOrWhiteSpace(left)) { throw new ArgumentNullException(nameof(left)); }

            bool isLikable = (comparison == Comparison.Like || comparison == Comparison.NotLike ||
                              comparison == Comparison.CiLike || comparison == Comparison.NotCiLike);

            string? modifiedRight = (makeFriendly || isLikable) ? MakeSqlReady(right, isLikable) : right?.ToString() ?? string.Empty;

            var whereClause = new StringBuilder();

            whereClause.Append($"{left}");

            switch (comparison)
            {
                case Comparison.Equal:
                    var equalsOperator = modifiedRight.Equals("null", StringComparison.InvariantCultureIgnoreCase) ? "IS" : "=";
                    whereClause.Append($" {equalsOperator} {modifiedRight}");
                    break;
                case Comparison.NotEqual:
                    equalsOperator = modifiedRight.Equals("null", StringComparison.InvariantCultureIgnoreCase) ? "IS NOT" : "<>";
                    whereClause.Append($" {equalsOperator} {modifiedRight}");
                    break;
                case Comparison.Like:
                    whereClause.Append($" LIKE {modifiedRight}");
                    break;
                case Comparison.CiLike:
                    whereClause.Append($" LIKE {modifiedRight}");
                    break;
                case Comparison.NotLike:
                    whereClause.Append($" NOT LIKE {modifiedRight}");
                    break;
                case Comparison.NotCiLike:
                    whereClause.Append($" NOT LIKE {modifiedRight}");
                    break;
                case Comparison.LessThan:
                    whereClause.Append($" < {modifiedRight}");
                    break;
                case Comparison.LessThanOrEqual:
                    whereClause.Append($" <= {modifiedRight}");
                    break;
                case Comparison.GreaterThan:
                    whereClause.Append($" > {modifiedRight}");
                    break;
                case Comparison.GreaterThanOrEqual:
                    whereClause.Append($" >= {modifiedRight}");
                    break;
                case Comparison.Between:
                    if (right2 == null) { throw new ArgumentNullException(nameof(right2), "Two arguments are required when using BETWEEN."); }
                    string modifiedRight2 = makeFriendly ? MakeSqlReady(right2) : right2?.ToString() ?? string.Empty;
                    whereClause.Append($" BETWEEN {modifiedRight} AND {modifiedRight2}");
                    break;
                case Comparison.NotBetween:
                    if (right2 == null) { throw new ArgumentNullException(nameof(right2), "Two arguments are required when using NOT BETWEEN."); }
                    modifiedRight2 = makeFriendly ? MakeSqlReady(right2) : right2?.ToString() ?? string.Empty;
                    whereClause.Append($" NOT BETWEEN {modifiedRight} AND {modifiedRight2}");
                    break;
                case Comparison.In:
                case Comparison.NotIn:
                    throw new Exception($"{comparison} is not supported for single value comparisons.");
                default:
                    throw new Exception($"Unknown comparison type ('{comparison}') in Where<T>(string left, Comparison comparison, T right, T right2 = default(T))");
            }

            return new SqlWhere(whereClause.ToString());
        }

        /// <summary>
        /// Creates a <see cref="SqlWhere"/> instance that represents an IN comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">An <see cref="IEnumerable{T}"/> collection.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance representing an IN comparison. </returns>
        public static SqlWhere WhereIn<T>(string left, IEnumerable<T> right, bool makeFriendly = true)
            => Where(left: left, comparison: Comparison.In, right: right, makeFriendly: makeFriendly);

        /// <summary>
        /// Creates a <see cref="SqlWhere"/> instance that represents a NOT IN comparison.
        /// </summary>
        /// <typeparam name="T">Any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="int"/>, <see cref="string"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">An <see cref="IEnumerable{T}"/> collection.</param>
        /// <param name="makeFriendly">If true, the value of <paramref name="right"/> will be made SQL friendly.</param>
        /// <returns>A <see cref="SqlWhere"/> instance representing a NOT IN comparison. </returns>
        public static SqlWhere WhereNotIn<T>(string left, IEnumerable<T> right, bool makeFriendly = true)
            => Where(left: left, comparison: Comparison.NotIn, right: right, makeFriendly: makeFriendly);

        private static string CleanseSql(string text, bool makeLikable = false)
        {
            // whitespace passes through, but nulls have be converted.
            if (string.IsNullOrEmpty(text)) { return "NULL"; }

            if (text.StartsWith("'") && text.EndsWith("'")) { return text; }

            text = text.Replace("'", "''");

            if (makeLikable)
            {
                // If the consumer didn't do this for us, wrap the text in %.
                if (!(text.StartsWith("%") || text.EndsWith("%")))
                {
                    text = $"%{text}%";
                }
            }

            return $"'{text}'";
        }

        private static string MakeDateSqlReady(DateTime date, bool makeLikable = false)
        {
            return CleanseSql(date.ToString(DateFormat), makeLikable);
        }

        private static string MakeSqlReady<T>(T item, bool makeLikable = false)
        {
            if (item == null) { return "NULL"; }

            Type? type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            return (type == typeof(Guid) || type == typeof(string))
                ? CleanseSql(item.ToString() ?? string.Empty, makeLikable)
                : (type == typeof(DateTime))
                    ? MakeDateSqlReady(Convert.ToDateTime(item), makeLikable)
                    : item.ToString() ?? string.Empty;
        }

        private static IEnumerable<string> MakeSqlReady<T>(IEnumerable<T> items, bool makeLikable = false)
        {
            if (items == null) { return new List<string>(); }

            var results = new List<string>();

            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            items.ToList().ForEach(i =>
            {
                if (i != null)
                {
                    if (type == typeof(Guid) || type == typeof(string))
                    {
                        results.Add(CleanseSql(i.ToString() ?? string.Empty, makeLikable));
                    }
                    else if (type == typeof(DateTime))
                    {
                        results.Add(MakeDateSqlReady(Convert.ToDateTime(i), makeLikable));
                    }
                    else
                    {
                        results.Add(i.ToString() ?? string.Empty);
                    }
                }
            });

            return results;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Raw;
        }
    }
}

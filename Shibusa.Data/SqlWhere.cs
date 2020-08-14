using Shibusa.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shibusa.Data
{
    /// <summary>
    /// Represents a complete T-SQL WHERE clause. This class implements the Specification pattern to provide
    /// AND, OR, ANDNOT, and ORNOT combinations. Clauses can be joined together in a variety of ways, even
    /// providing complex nested conditions.
    /// <seealso cref="https://en.wikipedia.org/wiki/Specification_pattern"/>
    /// </summary>
    public sealed class SqlWhere : IEquatable<SqlWhere>
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss.fffffff";

        /// <summary>
        /// The raw T-SQL to attach to a "WHERE" statement. This value does not
        /// begin with the 'WHERE' statement.
        /// </summary>
        public string Raw { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SqlWhere()
        {
            Raw = string.Empty;
        }

        /// <summary>
        /// Constructor that takes in an applicable string representing the WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause with which to prime the <see cref="Raw"/> property.</param>
        public SqlWhere(string where)
        {
            Raw = string.IsNullOrWhiteSpace(where)
                ? throw new ArgumentNullException(nameof(where))
                : where;
        }

        /// <summary>
        /// Constructor that takes in an existing <see cref="SqlWhere"/> object.
        /// </summary>
        /// <param name="where">The <see cref="SqlWhere"/> object to clone.</param>
        public SqlWhere(SqlWhere where)
        {
            Raw = where == null ? throw new ArgumentNullException(nameof(where))
                : where.Raw;
        }

        /// <summary>
        /// Creates a clone of the provided <see cref="SqlWhere"/> object.
        /// </summary>
        /// <param name="where">The object to clone.</param>
        /// <returns>A new <see cref="SqlWhere"/>.</returns>
        public SqlWhere Where(SqlWhere where) => new SqlWhere(where);

        /// <summary>
        /// Creates a new <see cref="SqlWhere"/> object that takes in an applicable string representing the WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause with which to prime the <see cref="Raw"/> property.</param>
        /// <returns>A new <see cref="SqlWhere"/>.</returns>
        public SqlWhere Where(string where) => new SqlWhere(where);

        /// <summary>
        /// Attaches the <see cref="Raw"/> of an <see cref="WhereClause"/> to the existing <see cref="Raw"/> with an AND conjunction.
        /// </summary>
        /// <param name="where">The <see cref="WhereClause"/> implementation to attach to the existing WHERE clause.</param>
        /// <returns>A new <see cref="SqlWhere"/>.</returns>
        public SqlWhere And(SqlWhere where) => new SqlWhere($"({Raw} AND {where.Raw})");

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="WhereClause"/> to the existing <see cref="Raw"/> with an OR conjunction.
        /// </summary>
        /// <param name="where">The <see cref="WhereClause"/> implementation to attach to the existing WHERE clause.</param>
        /// <returns>An <see cref="WhereClause"/> implementation with the combined WHERE clause.</returns>
        public SqlWhere Or(SqlWhere where) => new SqlWhere($"({Raw} OR {where.Raw})");

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="WhereClause"/> to the existing <see cref="Raw"/> with an AND NOT conjunction.
        /// </summary>
        /// <param name="where">The <see cref="WhereClause"/> implementation to attach to the existing WHERE clause.</param>
        /// <returns>An <see cref="WhereClause"/> implementation with the combined WHERE clause.</returns>
        public SqlWhere AndNot(SqlWhere where) => new SqlWhere($"({Raw} AND NOT {where.Raw})");

        /// <summary>
        /// Attaches the <see cref="Raw"/> of a <see cref="WhereClause"/> to the existing <see cref="Raw"/> with an OR NOT conjunction.
        /// </summary>
        /// <param name="where">The <see cref="WhereClause"/> implementation to attach to the existing WHERE clause.</param>
        /// <returns>An <see cref="WhereClause"/> implementation with the combined WHERE clause.</returns>
        public SqlWhere OrNot(SqlWhere where) => new SqlWhere($"({Raw} OR NOT {where.Raw})");

        /// <summary>
        /// Creates an <see cref="SqlWhere"/> implementation that compares the left to the
        /// right using the <see cref="Comparison"/> value provided.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., int, string, datetime).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="comparison">The type of comparison to be constructed.
        /// <seealso cref="Comparison"/></param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="right2">A second object to support the T-SQL BETWEEN keyword.</param>
        /// <param name="makeFriendly">If true, the values in right and right2
        /// will be wrapped in ticks or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// the comparison specified. <seealso cref="WhereClause.RawSql"/></returns>
        /// <remarks>This method cannot support IN or NOT IN. Use the methods that take in IEnumerable arguments.</remarks>
        public SqlWhere Where<T>(string left, Comparison comparison, IEnumerable<T> right, bool makeFriendly = true)
        {
            var whereClause = new StringBuilder();

            List<string> modifiedRight;
            if (makeFriendly)
            {
                modifiedRight = MakeSqlReady(right)?.ToList();
            }
            else
            {
                modifiedRight = new List<string>();
                right.ToList().ForEach(i => modifiedRight.Add(i.ToString()));
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
                case Comparison.NotLike:
                case Comparison.LessThan:
                case Comparison.LessThanOrEqual:
                case Comparison.GreaterThan:
                case Comparison.GreaterThanOrEqual:
                case Comparison.Between:
                case Comparison.NotBetween:
                    throw new Exception($"{comparison.ToString()} is not supported for multiple value comparisons.");
                default:
                    throw new Exception($"Unknown comparison type ('{comparison.ToString()}') in Where<T>(string left, Comparison comparison, T right, T right2 = default(T))");
            }

            return new SqlWhere(whereClause.ToString());
        }

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// an equality comparison between left and right.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// an equality check. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.Equal, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a lack of equality comparison between left and right.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a check for lack of equality. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereNotEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.NotEqual, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left '&gt;' right comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a '&gt;' comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereGreaterThan<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.GreaterThan, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left '&gt;=' right comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a '&gt;=' comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereGreaterThanOrEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.GreaterThanOrEqual, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left '&lt;' right comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a '&lt;' comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereLessThan<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.LessThan, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left '&lt;=' right comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a '&lt;=' comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereLessThanOrEqual<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.LessThanOrEqual, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a check using LIKE to compare left and right.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a LIKE comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.Like, right, default, makeFriendly);

        /// <summary>
        /// Creates a new <see cref="SqlWhere" /> instance where the left is compared to the right
        /// using a partial string match in a case insensitive match.
        /// </summary>
        /// <typeparam name="T">The type of instances in the right argument.</typeparam>
        /// <param name="left">The value of the left side of the equation.</param>
        /// <param name="right">The instance to which to compare.</param>
        /// <param name="makeFriendly">If true and if the type of item in the enumerable is an appopriate
        /// candidate for conversion, the values on the right will be modified to be friendly to the
        /// underlying system.</param>
        /// <returns>A new <see cref="SqlWhere" /> instance constructed from the arguments.</returns>
        public SqlWhere WhereCiLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.CiLike, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a check using NOT LIKE to compare left and right.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a NOT LIKE comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereNotLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.NotLike, right, default, makeFriendly);

        /// <summary>
        /// Creates a new <see cref="SqlWhere" /> instance where the left is compared to the right
        /// using a partial string match, excluding all that match.
        /// </summary>
        /// <typeparam name="T">The type of instances in the right argument.</typeparam>
        /// <param name="left">The value of the left side of the equation.</param>
        /// <param name="right">The instance to which to compare.</param>
        /// <param name="makeFriendly">If true and if the type of item in the enumerable is an appopriate
        /// candidate for conversion, the values on the right will be modified to be friendly to the
        /// underlying system.</param>
        /// <returns>A new <see cref="SqlWhere" /> instance constructed from the arguments.</returns>
        public SqlWhere WhereNotCiLike<T>(string left, T right, bool makeFriendly = true)
            => Where(left, Comparison.NotCiLike, right, default, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left BETWEEN right AND right2 comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a BETWEEN right AND right2 comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereBetween<T>(string left, T right, T right2, bool makeFriendly = true)
            => Where(left, Comparison.Between, right, right2, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents
        /// a left NOT BETWEEN right AND right2 comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">The object to compare to the left value.</param>
        /// <param name="makeFriendly">If true, the value of right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a NOT BETWEEN right AND right2 comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereNotBetween<T>(string left, T right, T right2, bool makeFriendly = true)
            => Where(left, Comparison.NotBetween, right, right2, makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents either
        /// an IN or NOT IN comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="comparison">The type of comparison to be constructed. Only <see cref="Comparison.In"/>
        /// and <see cref="Comparison.NotIn"/> are allowed.
        /// <param name="right">An <see cref="IEnumerable{T}"/> Fcollection.</param>
        /// <param name="makeFriendly">If true, the values in right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// either an IN or a NOT IN right comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere Where<T>(string left, Comparison comparison, T right, T right2 = default, bool makeFriendly = true)
        {
            if (string.IsNullOrWhiteSpace(left)) { throw new ArgumentNullException(nameof(left)); }

            bool isLikable = (comparison == Comparison.Like || comparison == Comparison.NotLike ||
                              comparison == Comparison.CiLike || comparison == Comparison.NotCiLike);
            string modifiedRight = (makeFriendly || isLikable)
                ? MakeSqlReady(right, isLikable)
                : right?.ToString();

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
                    string modifiedRight2 = makeFriendly ? MakeSqlReady(right2) : right.ToString();
                    whereClause.Append($" BETWEEN {modifiedRight} AND {modifiedRight2}");
                    break;
                case Comparison.NotBetween:
                    if (right2 == null) { throw new ArgumentNullException(nameof(right2), "Two arguments are required when using NOT BETWEEN."); }
                    modifiedRight2 = makeFriendly ? MakeSqlReady(right2) : right.ToString();
                    whereClause.Append($" NOT BETWEEN {modifiedRight} AND {modifiedRight2}");
                    break;
                case Comparison.In:
                case Comparison.NotIn:
                    throw new Exception($"{comparison.ToString()} is not supported for single value comparisons.");
                default:
                    throw new Exception($"Unknown comparison type ('{comparison.ToString()}') in Where<T>(string left, Comparison comparison, T right, T right2 = default(T))");
            }

            return new SqlWhere(whereClause.ToString());
        }

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents an IN comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">An <see cref="IEnumerable{T}"/> collection.</param>
        /// <param name="makeFriendly">If true, the values in right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// an IN right comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereIn<T>(string left, IEnumerable<T> right, bool makeFriendly = true)
            => Where(left: left, comparison: Comparison.In, right: right, makeFriendly: makeFriendly);

        /// <summary>
        /// Creates an object implementing <see cref="SqlWhere"/> that represents either
        /// a NOT IN comparison.
        /// </summary>
        /// <typeparam name="T">Virtually any type, but typically those commonly converted to data storage
        /// (e.g., <see cref="Int32"/>, <see cref="String"/>, or <see cref="DateTime"/>).</typeparam>
        /// <param name="left">The name of the column reference.</param>
        /// <param name="right">An <see cref="IEnumerable{T}"/> collection.</param>
        /// <param name="makeFriendly">If true, the values in right will be wrapped in ticks
        /// or converted to NULL if appropriate.</param>
        /// <returns>An object with an <see cref="SqlWhere"/> implementation that has raw SQL representing
        /// a NOT IN right comparison. <seealso cref="WhereClause.RawSql"/></returns>
        public SqlWhere WhereNotIn<T>(string left, IEnumerable<T> right, bool makeFriendly = true)
            => Where(left: left, comparison: Comparison.NotIn, right: right, makeFriendly: makeFriendly);

        private string CleanseSql(string text, bool makeLikable = false)
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

        private string MakeDateSqlReady(DateTime date, bool makeLikable = false)
        {
            return CleanseSql(date.ToString(DATE_FORMAT), makeLikable);
        }

        private string MakeSqlReady<T>(T item, bool makeLikable = false)
        {
            if (item == null) { return "NULL"; }

            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            string result;

            if (type == typeof(Guid)
                || type == typeof(string))
            {
                result = CleanseSql(item.ToString(), makeLikable);
            }
            else if (type == typeof(DateTime))
            {
                result = MakeDateSqlReady((item as DateTime?).Value, makeLikable);
            }
            else { result = item.ToString(); }

            return result;
        }

        private IEnumerable<string> MakeSqlReady<T>(IEnumerable<T> items, bool makeLikable = false)
        {
            if (items == null) { return new List<string>(); }

            var results = new List<string>();

            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            items.ToList().ForEach(i =>
            {
                if (type == typeof(Guid)
                    || type == typeof(string))
                {
                    results.Add(CleanseSql(i.ToString(), makeLikable));
                }
                else if (type == typeof(DateTime))
                {
                    results.Add(MakeDateSqlReady((i as DateTime?).Value, makeLikable));
                }
                else
                {
                    results.Add(i.ToString());
                }
            });

            return results;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// The value matches that of <see cref="Raw"/>.
        /// </summary>
        /// <returns>A string that represents the current object; in this case, <see cref="Raw"/>.</returns>
        public override string ToString()
        {
            return Raw;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SqlWhere);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(SqlWhere other)
        {
            return other != null &&
                   Raw == other.Raw;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return 2027124281 + EqualityComparer<string>.Default.GetHashCode(Raw);
        }
    }
}

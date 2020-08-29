using System.Collections.Generic;

namespace Shibusa.Data.Abstractions
{
    /// <summary>
    /// Represents the criteria used to formulate queries.
    /// </summary>
    public abstract class CriteriaBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CriteriaBase"/> class.
        /// </summary>
        public CriteriaBase()
        {
            OrderBy = new Dictionary<string, SortOrder>();
        }

        /// <summary>
        /// Gets or sets the logical operator to apply to combinations of values among the properties
        /// in this object.
        /// </summary>
        public virtual LogicalOperator LogicalOperator { get; set; } = CriteriaDefaults.DEFAULT_LOGICAL_OPERATOR;

        /// <summary>
        /// Gets a <see cref="IDictionary{TKey, TValue}"/> of strings (e.g., column or property names) and <see cref="SortOrder"/> values by which
        /// to order the results.
        /// </summary>
        public virtual IDictionary<string,SortOrder> OrderBy { get; }
    }
}

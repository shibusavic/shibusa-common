using System.Collections.Generic;

namespace Shibusa.Data.Abstractions
{
    /// <summary>
    /// Represents the criteria used to formulate queries.
    /// </summary>
    public abstract class CriteriaBase
    {
        protected Dictionary<string, SortOrder> orderBy = new Dictionary<string, SortOrder>();

        /// <summary>
        /// Gets or sets the logical operator to apply to combinations of values among the properties
        /// in this object.
        /// </summary>
        public virtual LogicalOperator LogicalOperator { get; set; } = CriteriaDefaults.DEFAULT_LOGICAL_OPERATOR;

        /// <summary>
        /// Gets or sets a <see cref="Dictionary{TKey, TValue}"/> of strings (e.g., column or property names) and <see cref="SortOrder"/> values by which
        /// to order the results.
        /// </summary>
        public virtual Dictionary<string, SortOrder> OrderBy
        {
            get => orderBy ?? new Dictionary<string, SortOrder>();
            set => orderBy = value ?? new Dictionary<string, SortOrder>();
        }
    }
}

namespace Shibusa.Data.Abstractions
{
    /// <summary>
    /// Represents the criteria used to formulate temporal queries.
    /// <seealso cref="https://docs.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-2017"/>
    /// </summary>
    public abstract class TemporalCriteriaBase : CriteriaBase
    {
        private DateTime? from;
        private DateTime? to;
        private DateTime? betweenStart;
        private DateTime? betweenEnd;
        private DateTime? containedInStart;
        private DateTime? containedInEnd;

        /// <summary>
        /// Gets or sets the AS OF value for temporal queries.
        /// </summary>
        public virtual DateTime? AsOf { get; set; }

        /// <summary>
        /// Gets or sets the FROM value in the FROM/TO syntax for temporal queries.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the value set is greater than the <see cref="To"/> value.</exception>
        public virtual DateTime? From
        {
            get => from;
            set
            {
                if (To.HasValue && value != null && value > To.Value)
                {
                    throw new ArgumentException("The From date cannot be after the To date on a temporal criteria object.");
                }
                from = value;
            }
        }

        /// <summary>
        /// Gets or sets the TO value in the FROM/TO syntax for temporal queries.
        /// </summary>
        public virtual DateTime? To
        {
            get => to;
            set
            {
                if (From.HasValue && value != null && value < From.Value)
                {
                    throw new ArgumentException("The From date cannot be after the To date on a temporal criteria object.");
                }
                to = value;
            }
        }

        /// <summary>
        /// Gets or sets the start value for the BETWEEN syntax for temporal queries.
        /// </summary>
        public virtual DateTime? BetweenStart
        {
            get => betweenStart;
            set
            {
                if (BetweenEnd.HasValue && value != null && value > BetweenEnd.Value)
                {
                    throw new ArgumentException("The Between Start date cannot be after the Between End date on a temporal criteria object.");
                }
                betweenStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the end value for the BETWEEN syntax for temporal queries.
        /// </summary>
        public virtual DateTime? BetweenEnd
        {
            get => betweenEnd;
            set
            {
                if (BetweenStart.HasValue && value != null && value < BetweenStart.Value)
                {
                    throw new ArgumentException("The Between Start date cannot be after the Between End date on a temporal criteria object.");
                }
                betweenEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets the start value for the CONTAINED IN syntax for temporal queries.
        /// </summary>
        public virtual DateTime? ContainedInStart
        {
            get => containedInStart;
            set
            {
                if (ContainedInEnd.HasValue && value != null && value > ContainedInEnd.Value)
                {
                    throw new ArgumentException("The Contained In start date cannot be after the Contained In end date on a temporal criteria object.");
                }
                containedInStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the end value for the CONTAINED IN syntax for temporal queries.
        /// </summary>
        public virtual DateTime? ContainedInEnd
        {
            get => containedInEnd;
            set
            {
                if (ContainedInStart.HasValue && value != null && value < ContainedInStart.Value)
                {
                    throw new ArgumentException("The Contained In start date cannot be after the Contained In end date on a temporal criteria object.");
                }
                containedInEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets an indicator for the ALL value in temporal queries.
        /// </summary>
        public virtual bool? All { get; set; }
    }
}

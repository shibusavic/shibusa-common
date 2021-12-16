namespace Shibusa.Data.Abstractions
{
    /// <summary>
    /// Represents the criteria used to formulate queries with paged results.
    /// </summary>
    public abstract class PagedCriteriaBase : CriteriaBase
    {
        protected int numberPerPage = CriteriaDefaults.DefaultNumberPerPage;
        protected int pageNumber = 1;

        /// <summary>
        /// Gets or sets the maximum number of records to be returned per page.
        /// </summary>
        /// <remarks>If this is set to any number less than 1, it will be
        /// set to <see cref="CriteriaDefaults.DefaultNumberPerPage"/></remarks>
        public virtual int NumberPerPage
        {
            get => numberPerPage;
            set => numberPerPage = value < 1
                    ? CriteriaDefaults.DefaultNumberPerPage
                    : value > CriteriaDefaults.DefaultMaxPerPage
                        ? CriteriaDefaults.DefaultMaxPerPage
                        : value;
        }

        /// <summary>
        /// Gets or sets the page number on which to begin the results.
        /// </summary>
        /// <remarks><see cref="PageNumber"/> will never be less than 1.</remarks>
        public virtual int PageNumber
        {
            get => pageNumber;
            set => pageNumber = Math.Max(1, value);
        }

        /// <summary>
        /// Gets the zero-based index corresponding to the first record of the current page.
        /// </summary>
        /// <remarks><see cref="OffsetValue"/> will never be less than 0.</remarks>
        public virtual int OffsetValue => NumberPerPage * (PageNumber - 1);
    }
}

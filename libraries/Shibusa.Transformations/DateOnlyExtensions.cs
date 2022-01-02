namespace Shibusa.Transformations
{
    /// <summary>
    /// Extends <see cref="DateOnly"/>.
    /// </summary>
    public static class DateOnlyExtensions
    {
        /// <summary>
        /// Converts a <see cref="DateOnly"/> to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The <see cref="DateOnly"/> to convert.</param>
        /// <returns>A <see cref="DateTime"/> instance with a <see cref="DateTimeKind"/> of <see cref="DateTimeKind.Unspecified"/>.</returns>
        public static DateTime ToDateTime(this DateOnly date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Unspecified);

        /// <summary>
        /// Converts a <see cref="DateOnly"/> to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The <see cref="DateOnly"/> to convert.</param>
        /// <param name="dateTimeKind">The <see cref="DateTimeKind"/> of the resulting <see cref="DateTime"/> instance.</param>
        /// <returns>A <see cref="DateTime"/> instance with the specified <see cref="DateTimeKind"/>.</returns>
        public static DateTime ToDateTime(this DateOnly date, DateTimeKind dateTimeKind) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, dateTimeKind);

        /// <summary>
        /// Converts a <see cref="DateOnly"/> to a UTC <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The <see cref="DateOnly"/> to convert.</param>
        /// <returns>A UTC <see cref="DateTime"/>.</returns>
        public static DateTime ToUtcDateTime(this DateOnly date) => date.ToDateTime(DateTimeKind.Utc);

        /// <summary>
        /// Converts a <see cref="DateOnly"/> to a Local <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The <see cref="DateOnly"/> to convert.</param>
        /// <returns>A local <see cref="DateTime"/>.</returns>
        public static DateTime ToLocalDateTime(this DateOnly date) => date.ToDateTime(DateTimeKind.Local);
    }
}

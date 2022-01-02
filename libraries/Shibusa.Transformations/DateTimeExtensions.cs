namespace Shibusa.Transformations
{
    /// <summary>
    /// Extends <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to the start of its day, preserving the <see cref="DateTime.Kind"/>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime date) =>
            new(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);

        /// <summary>
        /// Converts a <see cref="DateTime"/> to the end of its day, preserving the <see cref="DateTime.Kind"/>.
        /// </summary>
        /// <param name="date">The date to transform.</param>
        /// <returns>Ending time of the date passed in; the <see cref="DateTime.Kind"/> of date is preserved.</returns>
        public static DateTime EndOfDay(this DateTime date) =>
            new(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);

        /// <summary>
        /// Add or subtract weekdays from the specified date.
        /// </summary>
        /// <param name="dateTime">The date to convert.</param>
        /// <param name="numberToIncrement">The number of weekdays to increment or decrement.
        /// Use negative integers to decrement.</param>
        /// <returns>A date guaranteed to be a weekday.</returns>
        public static DateTime AddWeekdays(this DateTime dateTime, int numberToIncrement)
        {
            DateTime date = dateTime;
            if (numberToIncrement == 0) { return date; }

            int numberIncremented = 0;
            int increment = numberToIncrement > 0 ? 1 : -1;

            while (numberIncremented < Math.Abs(numberToIncrement))
            {
                date = date.AddDays(increment);
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) { continue; }
                numberIncremented++;
            }

            return date;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> to convert.</param>
        /// <returns>A <see cref="DateOnly"/> instance.</returns>
        public static DateOnly ToDateOnly(this DateTime date) => new DateOnly(date.Year, date.Month, date.Day);
    }
}

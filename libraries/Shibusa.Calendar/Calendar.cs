namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents a generic calendar.
    /// </summary>
    public class Calendar
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects containing an entry for each date
        /// in the range, inclusive of both the <paramref name="start"/> and <paramref name="finish"/> dates.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of inclusive <see cref="DateOnly"/> objects between the 
        /// <paramref name="start"/> and <paramref name="finish"/> dates.</returns>
        public static IEnumerable<DateOnly> GetInclusiveDays(DateOnly start, DateOnly finish)
        {
            var (first, last) = OrderDates(start, finish);

            while (first <= last)
            {
                yield return first;
                first = first.AddDays(1);
            }
        }

        /// <summary>
        /// Counts the days between the two dates, inclusively.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>An inclusive count of days between two dates.</returns>
        public static int CountInclusiveDays(DateOnly start, DateOnly finish) =>
            GetInclusiveDays(start, finish).Count();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects containing
        /// an entry for each weekday in the range, inclusively.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateOnly"/> between the start and end dates, inclusively,
        /// where the day of the week is a weekday.</returns>
        public static IEnumerable<DateOnly> GetInclusiveWeekDays(DateOnly start, DateOnly finish) =>
            GetInclusiveDays(start, finish).Where(d => d.DayOfWeek != DayOfWeek.Saturday
                 && d.DayOfWeek != DayOfWeek.Sunday);

        /// <summary>
        /// Counts the weekdays between two dates, inclusively.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>An inclusive count of week days between two dates.</returns>
        public static int CountInclusiveWeekDays(DateOnly start, DateOnly finish) =>
            GetInclusiveWeekDays(start, finish).Count();

        /// <summary>
        /// Returns a date for a holiday falling on a weekend day to the day
        /// on which it will be celebrated on the U.S. calendar.
        /// </summary>
        /// <param name="holiday">The holiday to adjust.</param>
        /// <returns>The day on which the holiday will be celebrated.</returns>
        public static DateOnly AdjustWeekendHolidayToCelebratedDay(DateOnly holiday) =>
            holiday.DayOfWeek == DayOfWeek.Saturday // move to Friday
                ? holiday.AddDays(-1)
                : holiday.DayOfWeek == DayOfWeek.Sunday // move to Monday
                    ? holiday.AddDays(1)
                    : holiday; // keep as-is
        
        /// <summary>
        /// Given two dates, return them in chronological order.
        /// </summary>
        /// <param name="date1">First date.</param>
        /// <param name="date2">Second date.</param>
        /// <returns>A tuple of first and last dates.</returns>
        public static (DateOnly First, DateOnly Last) OrderDates(DateOnly date1, DateOnly date2)
        {
            DateOnly first = date1 < date2 ? date1 : date2;
            DateOnly last = first < date2 ? date2 : date1;

            return (first, last);
        }
    }
}

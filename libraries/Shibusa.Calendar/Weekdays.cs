#if NETSTANDARD2_0
namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents days of the week other than Saturday and Sunday.
    /// </summary>
    public static class Weekdays
    {
        /// <summary>
        /// Count the number of weekdays between two dates.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="finish">The end date.</param>
        /// <param name="inclusive">An indicator of whether to count the <paramref name="finish"/> day; defaults to true.</param>
        /// <returns>A count of weekdays. The <paramref name="start"/> is always counted; if
        /// <paramref name="inclusive"/> is true, the <paramref name="finish"/> date is also counted.</returns>
        public static int Count(DateTime start, DateTime finish, bool inclusive = true)
        {
            var (first, last) = Calendar.OrderDates(start, finish);

            int count = 0;
            DateTime date = first;
            while (date < last)
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
                date = date.AddDays(1);
            }

            if (inclusive && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            { count++; }

            return count;
        }
    }
}
#else
namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents days of the week other than Saturday and Sunday.
    /// </summary>
    public static class Weekdays
    {
        /// <summary>
        /// Count the number of weekdays between two dates.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="finish">The end date.</param>
        /// <param name="inclusive">An indicator of whether to count the <paramref name="finish"/> day; defaults to true.</param>
        /// <returns>A count of weekdays. The <paramref name="start"/> is always counted; if
        /// <paramref name="inclusive"/> is true, the <paramref name="finish"/> date is also counted.</returns>
        public static int Count(DateOnly start, DateOnly finish, bool inclusive = true)
        {
            var (first, last) = Calendar.OrderDates(start, finish);

            int count = 0;
            DateOnly date = first;
            while (date < last)
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
                date = date.AddDays(1);
            }

            if (inclusive && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            { count++; }

            return count;
        }
    }
}
#endif
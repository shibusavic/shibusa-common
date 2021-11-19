using System;

namespace Shibusa.Calendar
{
    public static class Weekdays
    {
        /// <summary>
        /// Count the number of weekdays between two dates.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <param name="inclusive">An indicator of whether to count the last day.</param>
        /// <returns>A count of days.</returns>
        /// <remarks>If the <paramref name="end"/> date is before the <paramref name="start"/> date,
        /// the count will be negative.</remarks>
        public static int Count(DateTime start, DateTime end, bool inclusive = true)
        {
            int count = 0;
            DateTime date = start;
            if (end >= date)
            {
                while (date < end)
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        count++;
                    }
                    date = date.AddDays(1);
                }
                if (inclusive) { count++; }
            }
            else
            {
                while (date > end)
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        count--;
                    }
                    date = date.AddDays(-1);
                }
                if (inclusive) { count--; }
            }

            return count;
        }
    }
}

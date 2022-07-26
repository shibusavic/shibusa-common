namespace Shibusa.Calendar
{
    public static class Weekdays
    {
        /// <summary>
        /// Count the number of weekdays between two dates.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="finish">The end date.</param>
        /// <param name="inclusive">An indicator of whether to count the last day.</param>
        /// <returns>A count of weekdays.</returns>
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

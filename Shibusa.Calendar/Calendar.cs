using Shibusa.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents a generic calendar.
    /// </summary>
    public class Calendar
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateTime"/> objects containing an entry for each date
        /// in the range, inclusive of both the <paramref name="start"/> and <paramref name="finish"/> dates.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of inclusive <see cref="DateTime"/> objects between the 
        /// <paramref name="start"/> and <paramref name="finish"/> dates.</returns>
        public static IEnumerable<DateTime> GetInclusiveDays(DateTime start, DateTime finish)
        {
            DateTime incrementer = new(start.StartOfDay().Ticks);
            DateTime limit = new(finish.EndOfDay().Ticks);

            while (incrementer <= limit)
            {
                yield return incrementer;
                incrementer = incrementer.AddDays(1);
            }
        }

        /// <summary>
        /// Counts the days between the two dates, inclusively.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>An inclusive count of days between two dates.</returns>
        public static int CountInclusiveDays(DateTime start, DateTime finish) =>
            GetInclusiveDays(start, finish).Count();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateTime"/> objects containing
        /// an entry for each weekday in the range, inclusively.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateTime"/> between the start and end dates, inclusively,
        /// where the day of the week is a weekday.</returns>
        public static IEnumerable<DateTime> GetInclusiveWeekDays(DateTime start, DateTime finish) =>
            GetInclusiveDays(start, finish).Where(d => d.DayOfWeek != DayOfWeek.Saturday
                 && d.DayOfWeek != DayOfWeek.Sunday);

        /// <summary>
        /// Counts the weekdays between two dates, inclusively.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>An inclusive count of week days between two dates.</returns>
        public static int CountInclusiveWeekDays(DateTime start, DateTime finish) =>
            GetInclusiveWeekDays(start, finish).Count();

        /// <summary>
        /// Returns a date for a holiday falling on a weekend day to the day
        /// on which it will be celebrated on the U.S. calendar.
        /// </summary>
        /// <param name="holiday">The holiday to adjust.</param>
        /// <returns>The day on which the holiday will be celebrated.</returns>
        public static DateTime AdjustWeekendHolidayToCelebratedDay(DateTime holiday) =>
            holiday.DayOfWeek == DayOfWeek.Saturday // move to Friday
                ? holiday.AddDays(-1)
                : holiday.DayOfWeek == DayOfWeek.Sunday // move to Monday
                    ? holiday.AddDays(1)
                    : holiday; // keep as-is
    }
}

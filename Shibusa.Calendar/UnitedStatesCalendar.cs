using System;
using System.Collections.Generic;
using System.Linq;

namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents the calendar of U.S. holidays
    /// </summary>
    public sealed class UnitedStatesCalendar : Calendar
    {
        private static Dictionary<int, Dictionary<string, DateTime>> cachedYears = null;

        private static void CacheYears(int[] years)
        {
            cachedYears ??= new Dictionary<int, Dictionary<string, DateTime>>();

            foreach (int year in years)
            {
                if (!cachedYears.ContainsKey(year))
                {
                    cachedYears.Add(year, new Dictionary<string, DateTime>
                    {
                        { "New Year's Day", NewYearsDay(year)},
                        { "St. Valentine's Day", SaintValentinesDay(year)},
                        { "Easter Sunday", EasterSunday(year)},
                        { "Memorial Day", MemorialDay(year)},
                        { "Independence Day", IndependenceDay(year)},
                        { "Labor Day", LaborDay(year)},
                        { "Veterans Day", VeteransDay(year)},
                        { "Thanksgiving Day", ThanksgivingDay(year)},
                        { "Christmas Eve Day", ChristmasEveDay(year)},
                        { "Christmas Day", ChristmasDay(year)},
                        { "New Year's Eve Day", NewYearsEveDay(year)}
                    });
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateTime"/> objects containing
        /// an entry for each week day in the range, inclusively, and exluding any U.S. holiday
        /// celebrated on a week day.
        /// </summary>
        /// <param name="startDate">The inclusive start date.</param>
        /// <param name="endDate">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateTime"/> between the start and end dates, inclusively,
        /// where the day of the week is a weekday and not a celebrated holiday.</returns>
        public static IEnumerable<DateTime> GetWeekDaysExcludingHolidays(DateTime startDate, DateTime endDate) =>
            GetInclusiveWeekDays(startDate, endDate).Except(
                GetHolidayDates(startDate, endDate).Where(h =>
                    AdjustWeekendHolidayToCelebratedDay(h).DayOfWeek != DayOfWeek.Saturday
                    && AdjustWeekendHolidayToCelebratedDay(h).DayOfWeek != DayOfWeek.Sunday));

        /// <summary>
        /// Counts the week days between two dates, inclusively, excluding celebrated holidays.
        /// </summary>
        /// <param name="startDate">The inclusive start date.</param>
        /// <param name="endDate">The inclusive end date.</param>
        /// <returns>An inclusive count of week days between two dates, excluding celebrated holidays.</returns>
        public static int CountWeekDaysExcludingHolidays(DateTime startDate, DateTime endDate) =>
            GetWeekDaysExcludingHolidays(startDate, endDate).Count();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateTime"/> objects containing
        /// an entry for each recongized holiday.
        /// </summary>
        /// <param name="startDate">The inclusive start date.</param>
        /// <param name="endDate">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateTime"/> between the start and end dates, inclusively,
        /// where the day is a U.S. holiday.
        public static IEnumerable<DateTime> GetHolidayDates(DateTime startDate, DateTime endDate)
        {
            IEnumerable<int> years = Enumerable.Range(startDate.Year, (endDate.Year - startDate.Year) + 1);

            foreach (int year in years)
            {
                foreach (var holiday in Holidays(year).Values)
                {
                    if (holiday >= Transformations.TransformDateTime.StartOfDay(startDate)
                        && holiday <= Transformations.TransformDateTime.EndOfDay(endDate))
                    {
                        yield return holiday;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve a list of U.S. Holidays for a given year.
        /// </summary>
        /// <param name="year">The year for which to calculate the holidays.</param>
        /// <returns>A dictionary with each key naming a holiday and each value being the corresponding date for that holiday in the specified year.</returns>
        public static IDictionary<string, DateTime> Holidays(int year)
        {
            CacheYears(new int[] { year });
            return cachedYears[year];
        }

        /// <summary>
        /// Returns the date for New Year's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate New Year's Day.</param>
        /// <returns>New Years Day for the specified year.</returns>
        public static DateTime NewYearsDay(int year) => new DateTime(year, 1, 1).Date;

        /// <summary>
        /// Returns the date for Saint Valentine's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Valentine's Day.</param>
        /// <returns>Valentine's Day for the specified year.</returns>
        public static DateTime SaintValentinesDay(int year) => new DateTime(year, 2, 14);

        /// <summary>
        /// Returns the date for Easter Sunday for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Easter Sunday.</param>
        /// <returns>Easter Sunday for the specified year.</returns>
        /// <seealso cref="http://www.codeproject.com/KB/datetime/christianholidays.aspx"/>
        public static DateTime EasterSunday(int year)
        {
            var g = year % 19;
            var c = year / 100;
            var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            var i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));
            var day = i - (year + year / 4 + i + 2 - c + c / 4) % 7 + 28;

            var month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Returns the date for Memorial Day for the year provided.
        /// </summary>
        /// <param name="year">Year for which to calculate Memorial Day.</param>
        /// <returns>Memorial Day for the specified year.</returns>
        public static DateTime MemorialDay(int year)
        {
            var memorialDay = new DateTime(year, 5, 31);
            while (memorialDay.DayOfWeek != DayOfWeek.Monday)
            {
                memorialDay = memorialDay.AddDays(-1);
            }
            return memorialDay.Date;
        }

        /// <summary>
        /// Returns the date for U.S. Independence Day for the year provided.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Independence day for the specified year.</returns>
        public static DateTime IndependenceDay(int year) => new DateTime(year, 7, 4);

        /// <summary>
        /// Returns the date for Labor Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Labor Day.</param>
        /// <returns>Labor Day for the specified year.</returns>
        public static DateTime LaborDay(int year)
        {
            var laborDay = new DateTime(year, 9, 1);
            while (laborDay.DayOfWeek != DayOfWeek.Monday)
            {
                laborDay = laborDay.AddDays(1);
            }
            return laborDay;
        }

        /// <summary>
        /// Returns the date for Veterans Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Veterans Day.</param>
        /// <returns>Veterans Day for the specified year.</returns>
        public static DateTime VeteransDay(int year) => new DateTime(year, 11, 11);

        /// <summary>
        /// Returns the date for Thanksgiving Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Thanksgiving.</param>
        /// <returns>Thanksgiving for the specified year.</returns>
        public static DateTime ThanksgivingDay(int year)
        {
            var thanksgiving = (from day in Enumerable.Range(1, 30)
                                where new DateTime(year, 11, day).DayOfWeek == DayOfWeek.Thursday
                                select day).ElementAt(3);
            return new DateTime(year, 11, thanksgiving);

        }

        /// <summary>
        /// Returns the date for Christmas Eve Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Christmas Eve.</param>
        /// <returns>Christmas Eve for the specified year.</returns>
        public static DateTime ChristmasEveDay(int year) => ChristmasDay(year).AddDays(-1);

        /// <summary>
        /// Returns the date for Christmas Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Christmas.</param>
        /// <returns>Christmas for the specified year.</returns>
        public static DateTime ChristmasDay(int year) => new DateTime(year, 12, 25);

        /// <summary>
        /// Returns the date for New Year's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate New Year's Day.</param>
        /// <returns>New Year's Day for the specified year.</returns>
        public static DateTime NewYearsEveDay(int year) => new DateTime(year, 12, 31).Date;
    }
}

#if NETSTANDARD2_0
namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents the calendar of U.S. holidays
    /// </summary>
    public sealed class UnitedStatesCalendar
    {
        private static readonly SortedDictionary<int, IDictionary<string, DateTime>> cachedYears = new();

        /// <summary>
        /// Represents the U.S. holiday names.
        /// </summary>
        public static class HolidayNames
        {
            public const string NewYearsEve = "New Year's Eve Day";
            public const string MartinLutherKingJr = "Martin Luther King Jr's Birthday";
            public const string Valentines = "St. Valentine's Day";
            public const string Easter = "Easter Sunday";
            public const string MemorialDay = "Memorial Day";
            public const string Juneteenth = "Juneteenth";
            public const string IndependenceDay = "Independence Day";
            public const string LaborDay = "Labor Day";
            public const string VeteransDay = "Veterans Day";
            public const string Thanksgiving = "Thanksgiving Day";
            public const string ChristmasEve = "Christmas Eve Day";
            public const string Christmas = "Christmas Day";
            public const string NewYears = "New Year's Day";
        }

        private static void CacheYears(int[] years)
        {
            foreach (int year in years.Where(y => !cachedYears.ContainsKey(y)))
            {
                cachedYears[year] = new Dictionary<string, DateTime>
                    {
                        { HolidayNames.NewYears, NewYearsDay(year) },
                        { HolidayNames.MartinLutherKingJr, MartinLutherKingJrBirthday(year) },
                        { HolidayNames.Valentines, SaintValentinesDay(year)},
                        { HolidayNames.Easter, EasterSunday(year) },
                        { HolidayNames.MemorialDay, MemorialDay(year) },
                        { HolidayNames.Juneteenth, Juneteenth(year) },
                        { HolidayNames.IndependenceDay, IndependenceDay(year) },
                        { HolidayNames.LaborDay, LaborDay(year) },
                        { HolidayNames.VeteransDay, VeteransDay(year) },
                        { HolidayNames.Thanksgiving, ThanksgivingDay(year) },
                        { HolidayNames.ChristmasEve, ChristmasEveDay(year) },
                        { HolidayNames.Christmas, ChristmasDay(year) },
                        { HolidayNames.NewYearsEve, NewYearsEveDay(year) }
                    };
            }
        }

        public static string? GetNameForHoliday(DateTime date)
        {
            var holidaysForYear = Holidays(date.Year);

            var item = holidaysForYear.FirstOrDefault(h => h.Value.Equals(date));

            return item.Equals(default) ? null : item.Key;
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateTime"/> objects containing
        /// an entry for each week day in the range, inclusively, and exluding any U.S. holiday
        /// celebrated on a week day.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateTime"/> between the start and end dates, inclusively,
        /// where the day of the week is a weekday and not a celebrated holiday.</returns>
        public static IEnumerable<DateTime> GetWeekDaysExcludingHolidays(DateTime start, DateTime finish) =>
            Calendar.GetInclusiveWeekDays(start, finish).Except(
                GetHolidayDates(start, finish).Where(h =>
                    Calendar.AdjustWeekendHolidayToCelebratedDay(h).DayOfWeek != DayOfWeek.Saturday
                    && Calendar.AdjustWeekendHolidayToCelebratedDay(h).DayOfWeek != DayOfWeek.Sunday));

        /// <summary>
        /// Counts the week days between two dates, inclusively, excluding celebrated holidays.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>An inclusive count of week days between two dates, excluding celebrated holidays.</returns>
        public static int CountWeekDaysExcludingHolidays(DateTime start, DateTime finish) =>
            GetWeekDaysExcludingHolidays(start, finish).Count();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateTime"/> objects containing
        /// an entry for each recongized holiday.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateTime"/> holidays between the start and end dates, inclusively.
        public static IEnumerable<DateTime> GetHolidayDates(DateTime start, DateTime finish)
        {
            var (first, last) = Calendar.OrderDates(start, finish);

            foreach (int year in Enumerable.Range(first.Year, (last.Year - first.Year) + 1))
            {
                foreach (var holiday in Holidays(year).Values)
                {
                    if (holiday >= start && holiday <= finish)
                    {
                        yield return holiday;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve a list of U.S. Holidays for a given year.
        /// </summary>
        /// <param name="year">The year for which to calculate the holiday.</param>
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
        public static DateTime NewYearsDay(int year) => new DateTime(year, 1, 1);

        /// <summary>
        /// Returns the date for Martin Luther King Jr's Birthday
        /// </summary>
        /// <param name="year">The year for which to calculate the holiday.</param>
        /// <returns>Martin Luther King Jr's birthday for the specified year.</returns>
        public static DateTime MartinLutherKingJrBirthday(int year) => new DateTime(year, 1, 17);

        /// <summary>
        /// Returns the date for Saint Valentine's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Valentine's Day.</param>
        /// <returns>Valentine's Day for the specified year.</returns>
        public static DateTime SaintValentinesDay(int year) => new(year, 2, 14);

        /// <summary>
        /// Returns the date for Easter Sunday for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Easter Sunday.</param>
        /// <returns>Easter Sunday for the specified year.</returns>
        /// <seealso cref="http://www.codeproject.com/KB/DateTime/christianholidays.aspx"/>
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
            return memorialDay;
        }

        /// <summary>
        /// Returns the date for Juneteenth in the specified year.
        /// </summary>
        /// <param name="year">The specified year.</param>
        /// <returns>The date of Juneteenth for the specified year.</returns>
        public static DateTime Juneteenth(int year) => new DateTime(year, 6, 19);

        /// <summary>
        /// Returns the date for U.S. Independence Day for the year provided.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Independence day for the specified year.</returns>
        public static DateTime IndependenceDay(int year) => new(year, 7, 4);

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
        public static DateTime VeteransDay(int year) => new(year, 11, 11);

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
        public static DateTime ChristmasDay(int year) => new(year, 12, 25);

        /// <summary>
        /// Returns the date for New Year's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate New Year's Day.</param>
        /// <returns>New Year's Day for the specified year.</returns>
        public static DateTime NewYearsEveDay(int year) => new DateTime(year, 12, 31);
    }
}
#else
namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents the calendar of U.S. holidays
    /// </summary>
    public sealed class UnitedStatesCalendar
    {
        private static readonly SortedDictionary<int, IDictionary<string, DateOnly>> cachedYears = new();

        /// <summary>
        /// Represents the U.S. holiday names.
        /// </summary>
        public static class HolidayNames
        {
            public const string NewYearsEve = "New Year's Eve Day";
            public const string MartinLutherKingJr = "Martin Luther King Jr's Birthday";
            public const string Valentines = "St. Valentine's Day";
            public const string Easter = "Easter Sunday";
            public const string MemorialDay = "Memorial Day";
            public const string Juneteenth = "Juneteenth";
            public const string IndependenceDay = "Independence Day";
            public const string LaborDay = "Labor Day";
            public const string VeteransDay = "Veterans Day";
            public const string Thanksgiving = "Thanksgiving Day";
            public const string ChristmasEve = "Christmas Eve Day";
            public const string Christmas = "Christmas Day";
            public const string NewYears = "New Year's Day";
        }

        private static void CacheYears(int[] years)
        {
            foreach (int year in years.Where(y => !cachedYears.ContainsKey(y)))
            {
                cachedYears[year] = new Dictionary<string, DateOnly>
                    {
                        { HolidayNames.NewYears, NewYearsDay(year) },
                        { HolidayNames.MartinLutherKingJr, MartinLutherKingJrBirthday(year) },
                        { HolidayNames.Valentines, SaintValentinesDay(year)},
                        { HolidayNames.Easter, EasterSunday(year) },
                        { HolidayNames.MemorialDay, MemorialDay(year) },
                        { HolidayNames.Juneteenth, Juneteenth(year) },
                        { HolidayNames.IndependenceDay, IndependenceDay(year) },
                        { HolidayNames.LaborDay, LaborDay(year) },
                        { HolidayNames.VeteransDay, VeteransDay(year) },
                        { HolidayNames.Thanksgiving, ThanksgivingDay(year) },
                        { HolidayNames.ChristmasEve, ChristmasEveDay(year) },
                        { HolidayNames.Christmas, ChristmasDay(year) },
                        { HolidayNames.NewYearsEve, NewYearsEveDay(year) }
                    };
            }
        }

        public static string? GetNameForHoliday(DateOnly date)
        {
            var holidaysForYear = Holidays(date.Year);

            var item = holidaysForYear.FirstOrDefault(h => h.Value.Equals(date));

            return item.Equals(default) ? null : item.Key;
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects containing
        /// an entry for each week day in the range, inclusively, and exluding any U.S. holiday
        /// celebrated on a week day.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateOnly"/> between the start and end dates, inclusively,
        /// where the day of the week is a weekday and not a celebrated holiday.</returns>
        public static IEnumerable<DateOnly> GetWeekDaysExcludingHolidays(DateOnly start, DateOnly finish) =>
            Calendar.GetInclusiveWeekDays(start, finish).Except(
                GetHolidayDates(start, finish).Where(h =>
                    Calendar.AdjustWeekendHolidayToCelebratedDay(h).DayOfWeek != DayOfWeek.Saturday
                    && Calendar.AdjustWeekendHolidayToCelebratedDay(h).DayOfWeek != DayOfWeek.Sunday));

        /// <summary>
        /// Counts the week days between two dates, inclusively, excluding celebrated holidays.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>An inclusive count of week days between two dates, excluding celebrated holidays.</returns>
        public static int CountWeekDaysExcludingHolidays(DateOnly start, DateOnly finish) =>
            GetWeekDaysExcludingHolidays(start, finish).Count();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects containing
        /// an entry for each recongized holiday.
        /// </summary>
        /// <param name="start">The inclusive start date.</param>
        /// <param name="finish">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateOnly"/> holidays between the start and end dates, inclusively.
        public static IEnumerable<DateOnly> GetHolidayDates(DateOnly start, DateOnly finish)
        {
            var (first, last) = Calendar.OrderDates(start, finish);

            foreach (int year in Enumerable.Range(first.Year, (last.Year - first.Year) + 1))
            {
                foreach (var holiday in Holidays(year).Values)
                {
                    if (holiday >= start && holiday <= finish)
                    {
                        yield return holiday;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve a list of U.S. Holidays for a given year.
        /// </summary>
        /// <param name="year">The year for which to calculate the holiday.</param>
        /// <returns>A dictionary with each key naming a holiday and each value being the corresponding date for that holiday in the specified year.</returns>
        public static IDictionary<string, DateOnly> Holidays(int year)
        {
            CacheYears(new int[] { year });
            return cachedYears[year];
        }

        /// <summary>
        /// Returns the date for New Year's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate New Year's Day.</param>
        /// <returns>New Years Day for the specified year.</returns>
        public static DateOnly NewYearsDay(int year) => new DateOnly(year, 1, 1);

        /// <summary>
        /// Returns the date for Martin Luther King Jr's Birthday
        /// </summary>
        /// <param name="year">The year for which to calculate the holiday.</param>
        /// <returns>Martin Luther King Jr's birthday for the specified year.</returns>
        public static DateOnly MartinLutherKingJrBirthday(int year) => new DateOnly(year, 1, 17);

        /// <summary>
        /// Returns the date for Saint Valentine's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Valentine's Day.</param>
        /// <returns>Valentine's Day for the specified year.</returns>
        public static DateOnly SaintValentinesDay(int year) => new(year, 2, 14);

        /// <summary>
        /// Returns the date for Easter Sunday for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Easter Sunday.</param>
        /// <returns>Easter Sunday for the specified year.</returns>
        /// <seealso cref="http://www.codeproject.com/KB/DateOnly/christianholidays.aspx"/>
        public static DateOnly EasterSunday(int year)
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

            return new DateOnly(year, month, day);
        }

        /// <summary>
        /// Returns the date for Memorial Day for the year provided.
        /// </summary>
        /// <param name="year">Year for which to calculate Memorial Day.</param>
        /// <returns>Memorial Day for the specified year.</returns>
        public static DateOnly MemorialDay(int year)
        {
            var memorialDay = new DateOnly(year, 5, 31);
            while (memorialDay.DayOfWeek != DayOfWeek.Monday)
            {
                memorialDay = memorialDay.AddDays(-1);
            }
            return memorialDay;
        }

        /// <summary>
        /// Returns the date for Juneteenth in the specified year.
        /// </summary>
        /// <param name="year">The specified year.</param>
        /// <returns>The date of Juneteenth for the specified year.</returns>
        public static DateOnly Juneteenth(int year) => new DateOnly(year, 6, 19);

        /// <summary>
        /// Returns the date for U.S. Independence Day for the year provided.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Independence day for the specified year.</returns>
        public static DateOnly IndependenceDay(int year) => new(year, 7, 4);

        /// <summary>
        /// Returns the date for Labor Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Labor Day.</param>
        /// <returns>Labor Day for the specified year.</returns>
        public static DateOnly LaborDay(int year)
        {
            var laborDay = new DateOnly(year, 9, 1);
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
        public static DateOnly VeteransDay(int year) => new(year, 11, 11);

        /// <summary>
        /// Returns the date for Thanksgiving Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Thanksgiving.</param>
        /// <returns>Thanksgiving for the specified year.</returns>
        public static DateOnly ThanksgivingDay(int year)
        {
            var thanksgiving = (from day in Enumerable.Range(1, 30)
                                where new DateOnly(year, 11, day).DayOfWeek == DayOfWeek.Thursday
                                select day).ElementAt(3);
            return new DateOnly(year, 11, thanksgiving);

        }

        /// <summary>
        /// Returns the date for Christmas Eve Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Christmas Eve.</param>
        /// <returns>Christmas Eve for the specified year.</returns>
        public static DateOnly ChristmasEveDay(int year) => ChristmasDay(year).AddDays(-1);

        /// <summary>
        /// Returns the date for Christmas Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate Christmas.</param>
        /// <returns>Christmas for the specified year.</returns>
        public static DateOnly ChristmasDay(int year) => new(year, 12, 25);

        /// <summary>
        /// Returns the date for New Year's Day for the year provided.
        /// </summary>
        /// <param name="year">The year for which to calculate New Year's Day.</param>
        /// <returns>New Year's Day for the specified year.</returns>
        public static DateOnly NewYearsEveDay(int year) => new DateOnly(year, 12, 31);
    }
}
#endif
namespace Shibusa.Calendar
{
    /// <summary>
    /// Represents the calendar of U.S. holidays
    /// </summary>
    public sealed class UnitedStatesCalendar : Calendar
    {
        private static readonly Dictionary<int, Dictionary<string, DateOnly>> cachedYears = new();

        private static void CacheYears(int[] years)
        {
            foreach (int year in years)
            {
                if (!cachedYears.ContainsKey(year))
                {
                    cachedYears.Add(year, new Dictionary<string, DateOnly>
                    {
                        { "New Year's Day", NewYearsDay(year) },
                        { "Martin Luther King Jr's Birthday", MartinLutherKingJrBirthday(year) },
                        { "St. Valentine's Day", SaintValentinesDay(year)},
                        { "Easter Sunday", EasterSunday(year) },
                        { "Memorial Day", MemorialDay(year) },
                        { "Juneteenth", Juneteenth(year) },
                        { "Independence Day", IndependenceDay(year) },
                        { "Labor Day", LaborDay(year) },
                        { "Veterans Day", VeteransDay(year) },
                        { "Thanksgiving Day", ThanksgivingDay(year) },
                        { "Christmas Eve Day", ChristmasEveDay(year) },
                        { "Christmas Day", ChristmasDay(year) },
                        { "New Year's Eve Day", NewYearsEveDay(year) }
                    });
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects containing
        /// an entry for each week day in the range, inclusively, and exluding any U.S. holiday
        /// celebrated on a week day.
        /// </summary>
        /// <param name="startDate">The inclusive start date.</param>
        /// <param name="endDate">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateOnly"/> between the start and end dates, inclusively,
        /// where the day of the week is a weekday and not a celebrated holiday.</returns>
        public static IEnumerable<DateOnly> GetWeekDaysExcludingHolidays(DateOnly startDate, DateOnly endDate) =>
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
        public static int CountWeekDaysExcludingHolidays(DateOnly startDate, DateOnly endDate) =>
            GetWeekDaysExcludingHolidays(startDate, endDate).Count();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects containing
        /// an entry for each recongized holiday.
        /// </summary>
        /// <param name="startDate">The inclusive start date.</param>
        /// <param name="endDate">The inclusive end date.</param>
        /// <returns>A collection of <see cref="DateOnly"/> between the start and end dates, inclusively,
        /// where the day is a U.S. holiday.
        public static IEnumerable<DateOnly> GetHolidayDates(DateOnly start, DateOnly finish)
        {
            var (first, last) = OrderDates(start, finish);

            IEnumerable<int> years = Enumerable.Range(first.Year, (last.Year - first.Year) + 1);

            foreach (int year in years)
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

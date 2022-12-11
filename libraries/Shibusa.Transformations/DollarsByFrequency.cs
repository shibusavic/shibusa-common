namespace Shibusa.Transformations
{
    /// <summary>
    /// Utility class for converting dollar amounts between frequencies.
    /// </summary>
    public static class DollarsByFrequency
    {
        /// <summary>
        /// Frequencies
        /// </summary>
        public static class Frequency
        {
            public const string Hourly = "Hourly";
            public const string Daily = "Daily";
            public const string Weekly = "Weekly";
            public const string EveryTwoWeeks = "Every 2 Week";
            public const string SemiMonthly = "Semi-Monthly";
            public const string Monthly = "Monthly";
            public const string Quarterly = "Quarterly";
            public const string SemiAnnually = "Semi-Annually";
            public const string Annually = "Annually";
        }

        /// <summary>
        /// Represents calendar-related factors.
        /// </summary>
        public static class CalendarFactors
        {
            /// <summary>
            /// Twelve (12) months in a year.
            /// </summary>
            public const double MonthsInYear = 12;

            /// <summary>
            /// Fifty-two (52). Another constant. 
            /// Could have been (WEEKS_IN_YEAR = DAYS_IN_YEAR / 7) = 52.17963224893914;
            /// Using a constant of 52 here helped us align with the ever popular, but not very precise, 4.33 for WEEKS_IN_MONTH.
            /// Using 52.17963224893914, WEEKS_IN_MONTH becomes 4.348302687411595.
            /// </summary>
            public const double WeeksInYear = 52;

            /// <summary>
            /// WEEKS_IN_YEAR / MONTHS_IN_YEAR.
            /// </summary>
            public const double WeeksInMonth = 4.333333333;

            /// <summary>
            /// The days in a year, taken from an average over a 101 year period from 1900 through 2000, inclusively.
            /// </summary>
            public const double DaysInYear = 365.24752475247500;

            /// <summary>
            /// DAYS_IN_WEEK = DAYS_IN_YEAR / WEEKS_IN_YEAR
            /// </summary>
            public const double DaysInWeek = 7.024181264280269;

            /// <summary>
            /// DAYS_IN_MONTH = DAYS_IN_YEAR / MONTHS_IN_YEAR
            /// </summary>
            public const double DaysInMonth = 30.43811881188117;

            /// <summary>
            /// DAYS_IN_WORK_MONTH = DAYS_IN_MONTH*(5/7)
            /// </summary>
            public const double DaysInWorkMonth = 21.74151343705798;

            /// <summary>
            /// DAYS_IN_WORK_WEEK = DAYS_IN_WEEK*(5/7)
            /// </summary>
            public const double DaysInWorkWeek = 5.017272331628764;
        }

        /// <summary>
        /// Convert a wage dollar amount from one frequency to another.
        /// </summary>
        /// <param name="sourceFrequency">The source frequency for the converstion.</param>
        /// <param name="targetFrequency">The target frequency for the converstion.</param>
        /// <param name="amount">The dollar amount to convert.</param>
        /// <returns>The calculated dollar amount in the target frequency.</returns>
        /// <exception cref="Exception">Thrown if either the source or target frequency is not recognized.</exception>
        public static decimal Convert(string sourceFrequency, string targetFrequency, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(sourceFrequency) || string.IsNullOrWhiteSpace(targetFrequency)) { return 0; }
            if (amount == 0 || sourceFrequency.Equals(targetFrequency)) { return amount; }

            double workHoursInDay = 8;
            double workHoursInWeek = 40;

            decimal factor = sourceFrequency switch
            {
                Frequency.Hourly => targetFrequency switch
                {
                    Frequency.Hourly => 1M,
                    Frequency.Daily => (decimal)workHoursInDay,
                    Frequency.Weekly => (decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek,
                    Frequency.EveryTwoWeeks => 2M * (decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek,
                    Frequency.SemiMonthly => ((decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek * (decimal)CalendarFactors.WeeksInYear) / ((decimal)CalendarFactors.MonthsInYear * 2M),
                    Frequency.Monthly => ((decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek * (decimal)CalendarFactors.WeeksInYear) / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Quarterly => 3M * ((decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek * (decimal)CalendarFactors.WeeksInYear) / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.SemiAnnually => 6M * ((decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek * (decimal)CalendarFactors.WeeksInYear) / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Annually => ((decimal)workHoursInDay * (decimal)CalendarFactors.DaysInWorkWeek * (decimal)CalendarFactors.WeeksInYear),
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.Daily => targetFrequency switch
                {
                    Frequency.Hourly => 1M / (decimal)workHoursInDay,
                    Frequency.Daily => 1M,
                    Frequency.Weekly => (decimal)CalendarFactors.DaysInWorkWeek,
                    Frequency.EveryTwoWeeks => (decimal)CalendarFactors.DaysInWorkWeek * 2,
                    Frequency.SemiMonthly => (decimal)CalendarFactors.DaysInWorkMonth / 2,
                    Frequency.Monthly => (decimal)CalendarFactors.DaysInWorkMonth,
                    Frequency.Quarterly => (decimal)CalendarFactors.DaysInWorkMonth * 3,
                    Frequency.SemiAnnually => (decimal)CalendarFactors.DaysInWorkMonth * 6,
                    Frequency.Annually => (decimal)CalendarFactors.DaysInWorkWeek * (decimal)CalendarFactors.WeeksInYear,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.Weekly => targetFrequency switch
                {
                    Frequency.Hourly => 1M / (decimal)workHoursInWeek,
                    Frequency.Daily => 1M / (decimal)CalendarFactors.DaysInWorkWeek,
                    Frequency.Weekly => 1M,
                    Frequency.EveryTwoWeeks => 2M,
                    Frequency.SemiMonthly => (decimal)CalendarFactors.WeeksInYear / ((decimal)CalendarFactors.MonthsInYear * 2M),
                    Frequency.Monthly => (decimal)CalendarFactors.WeeksInYear / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Quarterly => (decimal)CalendarFactors.WeeksInYear / 4M,
                    Frequency.SemiAnnually => (decimal)CalendarFactors.WeeksInYear / 2M,
                    Frequency.Annually => (decimal)CalendarFactors.WeeksInYear,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.EveryTwoWeeks => targetFrequency switch
                {
                    Frequency.Hourly => 1M / ((decimal)workHoursInWeek * 2M),
                    Frequency.Daily => 1M / ((decimal)CalendarFactors.DaysInWorkWeek * 2M),
                    Frequency.Weekly => .5M,
                    Frequency.EveryTwoWeeks => 1M,
                    Frequency.SemiMonthly => ((decimal)CalendarFactors.WeeksInYear / 2M) / ((decimal)CalendarFactors.MonthsInYear * 2M),
                    Frequency.Monthly => ((decimal)CalendarFactors.WeeksInYear / 2M) / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Quarterly => ((decimal)CalendarFactors.WeeksInYear / 2M) / 4M,
                    Frequency.SemiAnnually => ((decimal)CalendarFactors.WeeksInYear / 2M) / 2M,
                    Frequency.Annually => ((decimal)CalendarFactors.WeeksInYear / 2M),
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.SemiMonthly => targetFrequency switch
                {
                    Frequency.Hourly => (2M * (decimal)CalendarFactors.MonthsInYear) / (decimal)CalendarFactors.WeeksInYear / (decimal)workHoursInWeek,
                    Frequency.Daily => (2M * (decimal)CalendarFactors.MonthsInYear) / (decimal)CalendarFactors.WeeksInYear / (decimal)CalendarFactors.DaysInWorkWeek,
                    Frequency.Weekly => (2M * (decimal)CalendarFactors.MonthsInYear) / (decimal)CalendarFactors.WeeksInYear,
                    Frequency.EveryTwoWeeks => (2M * (decimal)CalendarFactors.MonthsInYear) / ((decimal)CalendarFactors.WeeksInYear / 2M),
                    Frequency.SemiMonthly => 1M,
                    Frequency.Monthly => 2M,
                    Frequency.Quarterly => 2M * 3M,
                    Frequency.SemiAnnually => (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Annually => (decimal)CalendarFactors.MonthsInYear * 2,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.Monthly => targetFrequency switch
                {
                    Frequency.Hourly => (decimal)CalendarFactors.MonthsInYear / (decimal)CalendarFactors.WeeksInYear / (decimal)workHoursInWeek,
                    Frequency.Daily => 1M / (decimal)CalendarFactors.DaysInWorkMonth,
                    Frequency.Weekly => (decimal)CalendarFactors.MonthsInYear / (decimal)CalendarFactors.WeeksInYear,
                    Frequency.EveryTwoWeeks => (decimal)CalendarFactors.MonthsInYear / ((decimal)CalendarFactors.WeeksInYear / 2M),
                    Frequency.SemiMonthly => 0.5M,
                    Frequency.Monthly => 1M,
                    Frequency.Quarterly => 3M,
                    Frequency.SemiAnnually => 6M,
                    Frequency.Annually => (decimal)CalendarFactors.MonthsInYear,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.Quarterly => targetFrequency switch
                {
                    Frequency.Hourly => 4M / (decimal)CalendarFactors.WeeksInYear / (decimal)workHoursInWeek,
                    Frequency.Daily => 4M / (decimal)CalendarFactors.WeeksInYear / (decimal)CalendarFactors.DaysInWorkWeek,
                    Frequency.Weekly => 4M / (decimal)CalendarFactors.WeeksInYear,
                    Frequency.EveryTwoWeeks => 4M / ((decimal)CalendarFactors.WeeksInYear / 2M),
                    Frequency.SemiMonthly => 4M / ((decimal)CalendarFactors.MonthsInYear * 2M),
                    Frequency.Monthly => 1M / 3M,
                    Frequency.Quarterly => 1M,
                    Frequency.SemiAnnually => 2M,
                    Frequency.Annually => 4M,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.SemiAnnually => targetFrequency switch
                {
                    Frequency.Hourly => 2M / (decimal)CalendarFactors.WeeksInYear / (decimal)workHoursInWeek,
                    Frequency.Daily => 2M / (decimal)CalendarFactors.MonthsInYear / (decimal)CalendarFactors.DaysInWorkMonth,
                    Frequency.Weekly => 2M / (decimal)CalendarFactors.WeeksInYear,
                    Frequency.EveryTwoWeeks => 2M / ((decimal)CalendarFactors.WeeksInYear / 2M),
                    Frequency.SemiMonthly => 2M / ((decimal)CalendarFactors.MonthsInYear * 2M),
                    Frequency.Monthly => 2M / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Quarterly => 0.5M,
                    Frequency.SemiAnnually => 1M,
                    Frequency.Annually => 2M,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.Annually => targetFrequency switch
                {
                    Frequency.Hourly => 1M / (decimal)CalendarFactors.WeeksInYear / (decimal)workHoursInWeek,
                    Frequency.Daily => 1M / (decimal)CalendarFactors.MonthsInYear / (decimal)CalendarFactors.DaysInWorkMonth,
                    Frequency.Weekly => 1M / (decimal)CalendarFactors.WeeksInYear,
                    Frequency.EveryTwoWeeks => 1M / ((decimal)CalendarFactors.WeeksInYear / 2M),
                    Frequency.SemiMonthly => 1M / ((decimal)CalendarFactors.MonthsInYear * 2M),
                    Frequency.Monthly => 1M / (decimal)CalendarFactors.MonthsInYear,
                    Frequency.Quarterly => 1M / 4M,
                    Frequency.SemiAnnually => 1M / 2M,
                    Frequency.Annually => 1M,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                _ => throw new Exception($"Unknown source frequency: {sourceFrequency}")
            };

            return amount * factor;
        }
    }
}

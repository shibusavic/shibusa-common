using System;

namespace Shibusa.Transformations
{
    /// <summary>
    /// Utility class for converting dollar amounts between frequencies.
    /// </summary>
    public static class TransformDollarsByFrequency
    {
        /// <summary>
        /// Frequencies
        /// </summary>
        public static class Frequency
        {
            public const string HOURLY = "Hourly";
            public const string DAILY = "Daily";
            public const string WEEKLY = "Weekly";
            public const string EVERY_TWO_WEEKS = "Every 2 Week";
            public const string SEMI_MONTHLY = "Semi-Monthly";
            public const string MONTHLY = "Monthly";
            public const string QUARTERLY = "Quarterly";
            public const string SEMI_ANNUALLY = "Semi-Annually";
            public const string ANNUALLY = "Annually";
        }

        /// <summary>
        /// Represents calendar-related factors.
        /// </summary>
        public static class CalendarFactors
        {
            /// <summary>
            /// Twelve (12) months in a year.
            /// </summary>
            public const double MONTHS_IN_YEAR = 12;

            /// <summary>
            /// Fifty-two (52). Another constant. 
            /// Could have been (WEEKS_IN_YEAR = DAYS_IN_YEAR / 7) = 52.17963224893914;
            /// Using a constant of 52 here helped us align with the ever popular, but not very precise, 4.33 for WEEKS_IN_MONTH.
            /// Using 52.17963224893914, WEEKS_IN_MONTH becomes 4.348302687411595.
            /// </summary>
            public const double WEEKS_IN_YEAR = 52;

            /// <summary>
            /// WEEKS_IN_YEAR / MONTHS_IN_YEAR.
            /// </summary>
            public const double WEEKS_IN_MONTH = 4.333333333;

            /// <summary>
            /// The days in a year, taken from an average over a 101 year period from 1900 through 2000, inclusively.
            /// </summary>
            public const double DAYS_IN_YEAR = 365.24752475247500;

            /// <summary>
            /// DAYS_IN_WEEK = DAYS_IN_YEAR / WEEKS_IN_YEAR
            /// </summary>
            public const double DAYS_IN_WEEK = 7.024181264280269;

            /// <summary>
            /// DAYS_IN_MONTH = DAYS_IN_YEAR / MONTHS_IN_YEAR
            /// </summary>
            public const double DAYS_IN_MONTH = 30.43811881188117;

            /// <summary>
            /// DAYS_IN_WORK_MONTH = DAYS_IN_MONTH*(5/7)
            /// </summary>
            public const double DAYS_IN_WORK_MONTH = 21.74151343705798;

            /// <summary>
            /// DAYS_IN_WORK_WEEK = DAYS_IN_WEEK*(5/7)
            /// </summary>
            public const double DAYS_IN_WORK_WEEK = 5.017272331628764;
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
                Frequency.HOURLY => targetFrequency switch
                {
                    Frequency.HOURLY => 1M,
                    Frequency.DAILY => (decimal)workHoursInDay,
                    Frequency.WEEKLY => (decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK,
                    Frequency.EVERY_TWO_WEEKS => 2M * (decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK,
                    Frequency.SEMI_MONTHLY => ((decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * (decimal)CalendarFactors.WEEKS_IN_YEAR) / ((decimal)CalendarFactors.MONTHS_IN_YEAR * 2M),
                    Frequency.MONTHLY => ((decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * (decimal)CalendarFactors.WEEKS_IN_YEAR) / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.QUARTERLY => 3M * ((decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * (decimal)CalendarFactors.WEEKS_IN_YEAR) / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.SEMI_ANNUALLY => 6M * ((decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * (decimal)CalendarFactors.WEEKS_IN_YEAR) / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.ANNUALLY => ((decimal)workHoursInDay * (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * (decimal)CalendarFactors.WEEKS_IN_YEAR),
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.DAILY => targetFrequency switch
                {
                    Frequency.HOURLY => 1M / (decimal)workHoursInDay,
                    Frequency.DAILY => 1M,
                    Frequency.WEEKLY => (decimal)CalendarFactors.DAYS_IN_WORK_WEEK,
                    Frequency.EVERY_TWO_WEEKS => (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * 2,
                    Frequency.SEMI_MONTHLY => (decimal)CalendarFactors.DAYS_IN_WORK_MONTH / 2,
                    Frequency.MONTHLY => (decimal)CalendarFactors.DAYS_IN_WORK_MONTH,
                    Frequency.QUARTERLY => (decimal)CalendarFactors.DAYS_IN_WORK_MONTH * 3,
                    Frequency.SEMI_ANNUALLY => (decimal)CalendarFactors.DAYS_IN_WORK_MONTH * 6,
                    Frequency.ANNUALLY => (decimal)CalendarFactors.DAYS_IN_WORK_WEEK * (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.WEEKLY => targetFrequency switch
                {
                    Frequency.HOURLY => 1M / (decimal)workHoursInWeek,
                    Frequency.DAILY => 1M / (decimal)CalendarFactors.DAYS_IN_WORK_WEEK,
                    Frequency.WEEKLY => 1M,
                    Frequency.EVERY_TWO_WEEKS => 2M,
                    Frequency.SEMI_MONTHLY => (decimal)CalendarFactors.WEEKS_IN_YEAR / ((decimal)CalendarFactors.MONTHS_IN_YEAR * 2M),
                    Frequency.MONTHLY => (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.QUARTERLY => (decimal)CalendarFactors.WEEKS_IN_YEAR / 4M,
                    Frequency.SEMI_ANNUALLY => (decimal)CalendarFactors.WEEKS_IN_YEAR / 2M,
                    Frequency.ANNUALLY => (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.EVERY_TWO_WEEKS => targetFrequency switch
                {
                    Frequency.HOURLY => 1M / ((decimal)workHoursInWeek * 2M),
                    Frequency.DAILY => 1M / ((decimal)CalendarFactors.DAYS_IN_WORK_WEEK * 2M),
                    Frequency.WEEKLY => .5M,
                    Frequency.EVERY_TWO_WEEKS => 1M,
                    Frequency.SEMI_MONTHLY => ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M) / ((decimal)CalendarFactors.MONTHS_IN_YEAR * 2M),
                    Frequency.MONTHLY => ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M) / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.QUARTERLY => ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M) / 4M,
                    Frequency.SEMI_ANNUALLY => ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M) / 2M,
                    Frequency.ANNUALLY => ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M),
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.SEMI_MONTHLY => targetFrequency switch
                {
                    Frequency.HOURLY => (2M * (decimal)CalendarFactors.MONTHS_IN_YEAR) / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)workHoursInWeek,
                    Frequency.DAILY => (2M * (decimal)CalendarFactors.MONTHS_IN_YEAR) / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)CalendarFactors.DAYS_IN_WORK_WEEK,
                    Frequency.WEEKLY => (2M * (decimal)CalendarFactors.MONTHS_IN_YEAR) / (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    Frequency.EVERY_TWO_WEEKS => (2M * (decimal)CalendarFactors.MONTHS_IN_YEAR) / ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M),
                    Frequency.SEMI_MONTHLY => 1M,
                    Frequency.MONTHLY => 2M,
                    Frequency.QUARTERLY => 2M * 3M,
                    Frequency.SEMI_ANNUALLY => (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.ANNUALLY => (decimal)CalendarFactors.MONTHS_IN_YEAR * 2,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.MONTHLY => targetFrequency switch
                {
                    Frequency.HOURLY => (decimal)CalendarFactors.MONTHS_IN_YEAR / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)workHoursInWeek,
                    Frequency.DAILY => 1M / (decimal)CalendarFactors.DAYS_IN_WORK_MONTH,
                    Frequency.WEEKLY => (decimal)CalendarFactors.MONTHS_IN_YEAR / (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    Frequency.EVERY_TWO_WEEKS => (decimal)CalendarFactors.MONTHS_IN_YEAR / ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M),
                    Frequency.SEMI_MONTHLY => 0.5M,
                    Frequency.MONTHLY => 1M,
                    Frequency.QUARTERLY => 3M,
                    Frequency.SEMI_ANNUALLY => 6M,
                    Frequency.ANNUALLY => (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.QUARTERLY => targetFrequency switch
                {
                    Frequency.HOURLY => 4M / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)workHoursInWeek,
                    Frequency.DAILY => 4M / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)CalendarFactors.DAYS_IN_WORK_WEEK,
                    Frequency.WEEKLY => 4M / (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    Frequency.EVERY_TWO_WEEKS => 4M / ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M),
                    Frequency.SEMI_MONTHLY => 4M / ((decimal)CalendarFactors.MONTHS_IN_YEAR * 2M),
                    Frequency.MONTHLY => 1M / 3M,
                    Frequency.QUARTERLY => 1M,
                    Frequency.SEMI_ANNUALLY => 2M,
                    Frequency.ANNUALLY => 4M,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.SEMI_ANNUALLY => targetFrequency switch
                {
                    Frequency.HOURLY => 2M / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)workHoursInWeek,
                    Frequency.DAILY => 2M / (decimal)CalendarFactors.MONTHS_IN_YEAR / (decimal)CalendarFactors.DAYS_IN_WORK_MONTH,
                    Frequency.WEEKLY => 2M / (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    Frequency.EVERY_TWO_WEEKS => 2M / ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M),
                    Frequency.SEMI_MONTHLY => 2M / ((decimal)CalendarFactors.MONTHS_IN_YEAR * 2M),
                    Frequency.MONTHLY => 2M / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.QUARTERLY => 0.5M,
                    Frequency.SEMI_ANNUALLY => 1M,
                    Frequency.ANNUALLY => 2M,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                Frequency.ANNUALLY => targetFrequency switch
                {
                    Frequency.HOURLY => 1M / (decimal)CalendarFactors.WEEKS_IN_YEAR / (decimal)workHoursInWeek,
                    Frequency.DAILY => 1M / (decimal)CalendarFactors.MONTHS_IN_YEAR / (decimal)CalendarFactors.DAYS_IN_WORK_MONTH,
                    Frequency.WEEKLY => 1M / (decimal)CalendarFactors.WEEKS_IN_YEAR,
                    Frequency.EVERY_TWO_WEEKS => 1M / ((decimal)CalendarFactors.WEEKS_IN_YEAR / 2M),
                    Frequency.SEMI_MONTHLY => 1M / ((decimal)CalendarFactors.MONTHS_IN_YEAR * 2M),
                    Frequency.MONTHLY => 1M / (decimal)CalendarFactors.MONTHS_IN_YEAR,
                    Frequency.QUARTERLY => 1M / 4M,
                    Frequency.SEMI_ANNUALLY => 1M / 2M,
                    Frequency.ANNUALLY => 1M,
                    _ => throw new Exception($"Unknown target frequency: {targetFrequency}")
                },
                _ => throw new Exception($"Unknown source frequency: {sourceFrequency}")
            };

            return amount * factor;
        }
    }
}

using System;

namespace Shibusa.Transformations
{
    /// <summary>
    /// Extends <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to the start of its day, preserving the <see cref="DateTime.Kind"/>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime date) =>
            new(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);

        /// <summary>
        /// Converts a <see cref="DateTime"/> to the end of its day, preserving the <see cref="DateTime.Kind"/>.
        /// </summary>
        /// <param name="date">The date to transform.</param>
        /// <returns>Ending time of the date passed in; the <see cref="DateTime.Kind"/> of date is preserved.</returns>
        public static DateTime EndOfDay(this DateTime date) =>
            new(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);
    }
}

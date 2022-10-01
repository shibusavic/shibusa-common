using System.Text.RegularExpressions;

namespace Shibusa.Transformations
{
    public static class TransformWordsToTime
    {
        private static Regex timeRefRegex = new Regex(@"(\d+)\s+([^ ]+)\s+?(ago)?", RegexOptions.Singleline);

        public static DateTime ParseWords(string timeRef, DateTime? relativePosition = null)
        {
            DateTime result = relativePosition ?? DateTime.Now;

            var matches = timeRefRegex.Matches(timeRef);

            if (matches?.Any() ?? false)
            {
                string numberText = matches.First().Groups[1].Value.Trim();
                string periodText = matches.First().Groups[2].Value.ToLower().Trim();

                if (int.TryParse(numberText, out int number))
                {
                    number = Math.Abs(number) * -1;
                    result = periodText switch
                    {
                        var p when p is "week" or "weeks" => result.AddDays(7 * number),
                        var p when p is "day" or "days" => result.AddDays(number),
                        var p when p is "month" or "months" => result.AddMonths(number),
                        var p when p is "year" or "years" => result.AddYears(number),
                        _ => result
                    };
                }
            }

            return result;
        }
    }
}

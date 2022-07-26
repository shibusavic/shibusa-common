using Xunit;
using Xunit.Abstractions;

namespace Shibusa.Calendar.UnitTests
{
    public class CalendarTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CalendarTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void DateDays_ReversedDates_ResultIsInclusive()
        {
            var days = Calendar.GetInclusiveDays(new DateOnly(2020, 1, 5), new DateOnly(2020, 1, 1));
            Assert.Equal(5, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd") + $"\t{d.DayOfWeek}"));

            days = Calendar.GetInclusiveDays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 1));
            Assert.Single(days);
        }

        [Fact]
        public void GetDays_ResultIsInclusive()
        {
            var days = Calendar.GetInclusiveDays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Equal(5, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd") + $"\t{d.DayOfWeek}"));

            days = Calendar.GetInclusiveDays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 1));
            Assert.Single(days);
        }

        [Fact]
        public void CountDays_ResultIsInclusive()
        {
            var count = Calendar.CountInclusiveDays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Equal(5, count);
        }

        [Fact]
        public void GetWeekDays_ResultIsInclusive()
        {
            var days = Calendar.GetInclusiveWeekDays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Equal(3, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd") + $"\t{d.DayOfWeek}"));
        }

        [Fact]
        public void CountWeekDays_ResultIsInclusive()
        {
            var count = Calendar.CountInclusiveWeekDays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Equal(3, count);
        }
    }
}

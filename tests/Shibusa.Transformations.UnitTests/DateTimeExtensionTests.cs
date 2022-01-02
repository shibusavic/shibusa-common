using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class DateTimeExtensionTests
    {
        [Fact]
        public void StartOfDay()
        {
            DateTime now = DateTime.Now;

            Assert.Equal(DateTimeKind.Local, now.Kind);
            DateTime sod = now.StartOfDay();
            Assert.NotEqual(now, sod);
            Assert.Equal(now.Kind, sod.Kind);
            Assert.Equal(0, sod.Hour);
            Assert.Equal(0, sod.Minute);
            Assert.Equal(0, sod.Second);
            Assert.Equal(0, sod.Millisecond);

            now = DateTime.UtcNow;
            Assert.Equal(DateTimeKind.Utc, now.Kind);
            sod = now.StartOfDay();
            Assert.NotEqual(now, sod);
            Assert.Equal(now.Kind, sod.Kind);
            Assert.Equal(0, sod.Hour);
            Assert.Equal(0, sod.Minute);
            Assert.Equal(0, sod.Second);
            Assert.Equal(0, sod.Millisecond);
        }

        [Fact]
        public void EndOfDay()
        {
            DateTime now = DateTime.Now;
            Assert.Equal(DateTimeKind.Local, now.Kind);
            DateTime eod = now.EndOfDay();
            Assert.NotEqual(now, eod);
            Assert.Equal(now.Kind, eod.Kind);
            Assert.Equal(23, eod.Hour);
            Assert.Equal(59, eod.Minute);
            Assert.Equal(59, eod.Second);
            Assert.Equal(999, eod.Millisecond);

            now = DateTime.UtcNow;
            Assert.Equal(DateTimeKind.Utc, now.Kind);
            eod = now.EndOfDay();
            Assert.NotEqual(now, eod);
            Assert.Equal(now.Kind, eod.Kind);
            Assert.Equal(23, eod.Hour);
            Assert.Equal(59, eod.Minute);
            Assert.Equal(59, eod.Second);
            Assert.Equal(999, eod.Millisecond);
        }

        [Fact]
        public void GetWeekday_Forward_1()
        {
            DateTime saturday = new(2021, 10, 2);
            DateTime nextWeekday = saturday.AddWeekdays(1);
            DateTime expected = new(2021, 10, 4);
            Assert.Equal(expected, nextWeekday);
        }

        [Fact]
        public void GetWeekday_Forward_10()
        {
            DateTime saturday = new(2021, 10, 2);
            DateTime nextWeekday = saturday.AddWeekdays(10);
            DateTime expected = new(2021, 10, 15);
            Assert.True(nextWeekday.DayOfWeek == DayOfWeek.Friday);
            Assert.Equal(expected, nextWeekday);
        }

        [Fact]
        public void GetWeekday_Reverse_1()
        {
            DateTime saturday = new(2021, 10, 2);
            DateTime previousWeekday = saturday.AddWeekdays(-1);
            DateTime expected = new(2021, 10, 1);
            Assert.True(previousWeekday.DayOfWeek == DayOfWeek.Friday);
            Assert.Equal(expected, previousWeekday);
        }

        [Fact]
        public void GetWeekday_Reverse_10()
        {
            DateTime saturday = new(2021, 10, 2);
            DateTime previousWeekday = saturday.AddWeekdays(-10);
            Assert.True(previousWeekday.DayOfWeek == DayOfWeek.Monday);
            DateTime expected = new(2021, 9, 20);
            Assert.Equal(expected, previousWeekday);
        }

        [Fact]
        public void ToDateOnly_Local()
        {
            var now = DateTime.Now;
            var dateOnly = now.ToDateOnly();
            Assert.Equal(dateOnly.Year, now.Year);
            Assert.Equal(dateOnly.Month, now.Month);
            Assert.Equal(dateOnly.Day, now.Day);
        }

        [Fact]
        public void ToDateOnly_Utc()
        {
            var now = DateTime.UtcNow;
            var dateOnly = now.ToDateOnly();
            Assert.Equal(dateOnly.Year, now.Year);
            Assert.Equal(dateOnly.Month, now.Month);
            Assert.Equal(dateOnly.Day, now.Day);
        }
    }
}

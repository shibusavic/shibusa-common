using Xunit;

namespace Shibusa.Calendar.UnitTests
{
    public class WeekdaysTests
    {
        [Fact]
        public void Count_SameDate_NotInclusive_Zero()
        {
            DateTime date = new(2020, 1, 1); // Wednesday
            int count = Weekdays.Count(date, date, false);
            Assert.Equal(0, count);
        }

        [Fact]
        public void Count_SameDate_Inclusive_One()
        {
            DateTime date = new(2020, 1, 1); // Wednesday
            int count = Weekdays.Count(date, date, true);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Count_StartBeforeEnd_NotInclusive_Counts()
        {
            DateTime date1 = new(2020, 1, 1);
            DateTime date2 = new(2020, 1, 2);
            int count = Weekdays.Count(date1, date2, false);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Count_StartBeforeEnd_Inclusive_Counts()
        {
            DateTime date1 = new(2020, 1, 1);
            DateTime date2 = new(2020, 1, 2);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_EndBeforeStart_NotInclusive_NegativeCount()
        {
            DateTime date1 = new(2020, 1, 2);
            DateTime date2 = new(2020, 1, 1);
            int count = Weekdays.Count(date1, date2, false);
            Assert.Equal(-1, count);
        }

        [Fact]
        public void Count_EndBeforeStart_Inclusive_NegativeCount()
        {
            DateTime date1 = new(2020, 1, 2);
            DateTime date2 = new(2020, 1, 1);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(-2, count);
        }

        [Fact]
        public void Count_OverWeekend_CountsWeekdays()
        {
            DateTime date1 = new(2020, 1, 3);
            DateTime date2 = new(2020, 1, 6);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_OverWeekend_CountsWeekdays_Negative()
        {
            DateTime date1 = new(2020, 1, 6);
            DateTime date2 = new(2020, 1, 3);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(-2, count);
        }
    }
}

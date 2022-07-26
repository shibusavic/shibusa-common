using Xunit;

namespace Shibusa.Calendar.UnitTests
{
    public class WeekdaysTests
    {
        [Fact]
        public void Count_SameDate_NotInclusive_Zero()
        {
            DateOnly date = new(2020, 1, 1); // Wednesday
            int count = Weekdays.Count(date, date, false);
            Assert.Equal(0, count);
        }

        [Fact]
        public void Count_SameDate_Inclusive_One()
        {
            DateOnly date = new(2020, 1, 1); // Wednesday
            int count = Weekdays.Count(date, date, true);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Count_StartBeforeEnd_NotInclusive_Counts()
        {
            DateOnly date1 = new(2020, 1, 1);
            DateOnly date2 = new(2020, 1, 2);
            int count = Weekdays.Count(date1, date2, false);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Count_StartBeforeEnd_Inclusive_Counts()
        {
            DateOnly date1 = new(2020, 1, 1);
            DateOnly date2 = new(2020, 1, 2);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_EndBeforeStart_NotInclusive_Reversed_PositiveCount()
        {
            DateOnly date1 = new(2020, 1, 2);
            DateOnly date2 = new(2020, 1, 1);
            int count = Weekdays.Count(date1, date2, false);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Count_EndBeforeStart_Inclusive_Reversed_PositiveCount()
        {
            DateOnly date1 = new(2020, 1, 2);
            DateOnly date2 = new(2020, 1, 1);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_OverWeekend_CountsWeekdays()
        {
            DateOnly date1 = new(2020, 1, 3);
            DateOnly date2 = new(2020, 1, 6);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_OverWeekend_CountsWeekdays_Reversed_Positive()
        {
            DateOnly date1 = new(2020, 1, 6);
            DateOnly date2 = new(2020, 1, 3);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Count_IntoWeekend_CountsWeekdays()
        {
            DateOnly date1 = new(2020, 1, 3);
            DateOnly date2 = new(2020, 1, 5);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Count_IntoWeekend_CountsWeekdays_Reversed_Positive()
        {
            DateOnly date1 = new(2020, 1, 5);
            DateOnly date2 = new(2020, 1, 3);
            int count = Weekdays.Count(date1, date2, true);
            Assert.Equal(1, count);
        }
    }
}

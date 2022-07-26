using Xunit;
using Xunit.Abstractions;

namespace Shibusa.Calendar.UnitTests
{
    public class UnitedStatesCalendarTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public UnitedStatesCalendarTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void NewYears()
        {
            Assert.Equal(new DateOnly(2000, 1, 1), UnitedStatesCalendar.NewYearsDay(2000));
        }

        [Fact]
        public void Valentines()
        {
            Assert.Equal(new DateOnly(2000, 2, 14), UnitedStatesCalendar.SaintValentinesDay(2000));
        }

        [Fact]
        public void Easter()
        {
            Assert.Equal(new DateOnly(2001, 4, 15), UnitedStatesCalendar.EasterSunday(2001));
            Assert.Equal(new DateOnly(2005, 3, 27), UnitedStatesCalendar.EasterSunday(2005));
        }

        [Fact]
        public void MartinLutherKingJrBirthday()
        {
            Assert.Equal(new DateOnly(2010, 1, 17), UnitedStatesCalendar.MartinLutherKingJrBirthday(2010));
        }

        [Fact]
        public void MemorialDay()
        {
            Assert.Equal(new DateOnly(1999, 5, 31), UnitedStatesCalendar.MemorialDay(1999));
            Assert.Equal(new DateOnly(2022, 5, 30), UnitedStatesCalendar.MemorialDay(2022));
        }

        [Fact]
        public void Juneteenth()
        {
            Assert.Equal(new DateOnly(2020, 6, 19), UnitedStatesCalendar.Juneteenth(2020));
        }

        [Fact]
        public void IndependenceDay()
        {
            Assert.Equal(new DateOnly(2000, 7, 4), UnitedStatesCalendar.IndependenceDay(2000));
        }

        [Fact]
        public void LaborDay()
        {
            Assert.Equal(new DateOnly(2004, 9, 6), UnitedStatesCalendar.LaborDay(2004));
        }

        [Fact]
        public void Veterans()
        {
            Assert.Equal(new DateOnly(2004, 11, 11), UnitedStatesCalendar.VeteransDay(2004));
            Assert.Equal(new DateOnly(2014, 11, 11), UnitedStatesCalendar.VeteransDay(2014));
        }

        [Fact]
        public void Thanksgiving()
        {
            Assert.Equal(new DateOnly(2017, 11, 23), UnitedStatesCalendar.ThanksgivingDay(2017));
        }

        [Fact]
        public void ChristmasEve()
        {
            Assert.Equal(new DateOnly(2000, 12, 24), UnitedStatesCalendar.ChristmasEveDay(2000));
        }

        [Fact]
        public void Christmas()
        {
            Assert.Equal(new DateOnly(2000, 12, 25), UnitedStatesCalendar.ChristmasDay(2000));
        }

        [Fact]
        public void NewYearsEve()
        {
            Assert.Equal(new DateOnly(2000, 12, 31), UnitedStatesCalendar.NewYearsEveDay(2000));
        }

        [Fact]
        public void GetHolidayDates_ResultIsInclusive()
        {
            var days = UnitedStatesCalendar.GetHolidayDates(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Single(days);
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd") + $"\t{d.DayOfWeek}"));
        }

        [Fact]
        public void GetWeekDaysExcludingHolidays_ResultIsInclusive()
        {
            var days = UnitedStatesCalendar.GetWeekDaysExcludingHolidays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Equal(2, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd") + $"\t{d.DayOfWeek}"));
        }

        [Fact]
        public void CountWeekDaysExcludingHolidays_ResultIsInclusive()
        {
            var count = UnitedStatesCalendar.CountWeekDaysExcludingHolidays(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 5));
            Assert.Equal(2, count);
        }
    }
}

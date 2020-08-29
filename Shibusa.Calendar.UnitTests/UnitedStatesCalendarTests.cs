using System;
using System.Linq;
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
            Assert.Equal(new System.DateTime(2000, 1, 1), (System.DateTime)UnitedStatesCalendar.NewYearsDay(2000));
        }

        [Fact]
        public void Valentines()
        {
            Assert.Equal(new System.DateTime(2000, 2, 14), (System.DateTime)UnitedStatesCalendar.SaintValentinesDay(2000));
        }

        [Fact]
        public void Easter()
        {
            Assert.Equal(new System.DateTime(2001, 4, 15), (System.DateTime)UnitedStatesCalendar.EasterSunday(2001));
            Assert.Equal(new System.DateTime(2005, 3, 27), (System.DateTime)UnitedStatesCalendar.EasterSunday(2005));
        }

        [Fact]
        public void MemorialDay()
        {
            Assert.Equal(new System.DateTime(1999, 5, 31), (System.DateTime)UnitedStatesCalendar.MemorialDay(1999));
            Assert.Equal(new System.DateTime(2022, 5, 30), (System.DateTime)UnitedStatesCalendar.MemorialDay(2022));
        }

        [Fact]
        public void IndependenceDay()
        {
            Assert.Equal(new System.DateTime(2000, 7, 4), (System.DateTime)UnitedStatesCalendar.IndependenceDay(2000));
        }

        [Fact]
        public void LaborDay()
        {
            Assert.Equal(new System.DateTime(2004, 9, 6), (System.DateTime)UnitedStatesCalendar.LaborDay(2004));
        }

        [Fact]
        public void Veterans()
        {
            Assert.Equal(new System.DateTime(2004, 11, 11), (System.DateTime)UnitedStatesCalendar.VeteransDay(2004));
            Assert.Equal(new System.DateTime(2014, 11, 11), (System.DateTime)UnitedStatesCalendar.VeteransDay(2014));
        }

        [Fact]
        public void Thanksgiving()
        {
            Assert.Equal(new System.DateTime(2017, 11, 23), (System.DateTime)UnitedStatesCalendar.ThanksgivingDay(2017));
        }

        [Fact]
        public void ChristmasEve()
        {
            Assert.Equal(new System.DateTime(2000, 12, 24), (System.DateTime)UnitedStatesCalendar.ChristmasEveDay(2000));
        }

        [Fact]
        public void Christmas()
        {
            Assert.Equal(new System.DateTime(2000, 12, 25), (System.DateTime)UnitedStatesCalendar.ChristmasDay(2000));
        }

        [Fact]
        public void NewYearsEve()
        {
            Assert.Equal(new System.DateTime(2000, 12, 31), (System.DateTime)UnitedStatesCalendar.NewYearsEveDay(2000));
        }

        [Fact]
        public void DateDays_InvalidDate_EmptyResult()
        {
            var days = UnitedStatesCalendar.GetInclusiveDays(new DateTime(2020, 1, 2), new DateTime(2020, 1, 1));
            Assert.Empty(days);
        }

        [Fact]
        public void GetDays_ResultIsInclusive()
        {
            var days = UnitedStatesCalendar.GetInclusiveDays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Equal(5, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd HH:mm:ss:ffff") + $"\t{d.DayOfWeek}"));

            days = UnitedStatesCalendar.GetInclusiveDays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));
            Assert.Single(days);
        }

        [Fact]
        public void CountDays_ResultIsInclusive()
        {
            var count = UnitedStatesCalendar.CountInclusiveDays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Equal(5, count);
        }

        [Fact]
        public void GetWeekDays_ResultIsInclusive()
        {
            var days = UnitedStatesCalendar.GetInclusiveWeekDays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Equal(3, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd HH:mm:ss:ffff") + $"\t{d.DayOfWeek}"));
        }

        [Fact]
        public void CountWeekDays_ResultIsInclusive()
        {
            var count = UnitedStatesCalendar.CountInclusiveWeekDays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Equal(3, count);
        }

        [Fact]
        public void GetHolidayDates_ResultIsInclusive()
        {
            var days = UnitedStatesCalendar.GetHolidayDates(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Single(days);
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd HH:mm:ss:ffff") + $"\t{d.DayOfWeek}"));
        }

        [Fact]
        public void GetWeekDaysExcludingHolidays_ResultIsInclusive()
        {
            var days = UnitedStatesCalendar.GetWeekDaysExcludingHolidays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Equal(2, days.Count());
            days.ToList().ForEach(d => testOutputHelper.WriteLine(d.ToString("yyyy-MM-dd HH:mm:ss:ffff") + $"\t{d.DayOfWeek}"));
        }

        [Fact]
        public void CountWeekDaysExcludingHolidays_ResultIsInclusive()
        {
            var count = UnitedStatesCalendar.CountWeekDaysExcludingHolidays(new DateTime(2020, 1, 1), new DateTime(2020, 1, 5));
            Assert.Equal(2, count);
        }
    }
}

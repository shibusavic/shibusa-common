using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class DollarsByFrequencyTests
    {
        [Fact]
        public void WageDollarTransformations_MatchingFrequencies()
        {
            decimal dailyAmount = 1000M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: dailyAmount);

            Assert.Equal(dailyAmount, answer);
        }

        private static void AssertInRange(decimal expectedAnswer, decimal answer, decimal errorRange = .01M)
        {
            decimal high = expectedAnswer * (1 + errorRange);
            decimal low = expectedAnswer * (1 - errorRange);

            Assert.True(answer >= low && answer <= high, $"{answer} expected to be between {low} and {high}.");
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromHourly()
        {
            decimal hourlyAmount = 10M;
            decimal expectedDailyAmount = hourlyAmount * 8M;
            decimal expectedWeeklyAmount = hourlyAmount * 40M;
            decimal expectedTwoWeeks = expectedWeeklyAmount * 2;
            decimal expectedMonthly = (expectedWeeklyAmount * 52) / 12;
            decimal expectedSemiMonthly = expectedMonthly / 2;
            decimal expectedQuarterly = expectedMonthly * 3;
            decimal expectedSemiAnnual = expectedMonthly * 6;
            decimal expectedAnnual = expectedMonthly * 12;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: hourlyAmount);

            AssertInRange(hourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: hourlyAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: hourlyAmount);

            AssertInRange(expectedWeeklyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: hourlyAmount);

            AssertInRange(expectedTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: hourlyAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: hourlyAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: hourlyAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: hourlyAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Hourly,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: hourlyAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromDaily()
        {
            decimal dailyAmount = 400M;
            decimal expectedHourlyAmount = dailyAmount / 8M;
            decimal expectedWeeklyAmount = dailyAmount * 5M;
            decimal expectedTwoWeeks = expectedWeeklyAmount * 2;
            decimal expectedMonthly = (expectedWeeklyAmount * 52) / 12;
            decimal expectedSemiMonthly = expectedMonthly / 2;
            decimal expectedQuarterly = expectedMonthly * 3;
            decimal expectedSemiAnnual = expectedMonthly * 6;
            decimal expectedAnnual = expectedMonthly * 12;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: dailyAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: dailyAmount);

            AssertInRange(dailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: dailyAmount);

            AssertInRange(expectedWeeklyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: dailyAmount);

            AssertInRange(expectedTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: dailyAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: dailyAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: dailyAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: dailyAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Daily,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: dailyAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromWeekly()
        {
            decimal weeklyAmount = 400M;
            decimal expectedDailyAmount = weeklyAmount / 5M;
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedTwoWeeks = weeklyAmount * 2M;
            decimal expectedMonthly = (weeklyAmount * 52M) / 12M;
            decimal expectedSemiMonthly = expectedMonthly / 2M;
            decimal expectedQuarterly = expectedMonthly * 3M;
            decimal expectedSemiAnnual = expectedMonthly * 6M;
            decimal expectedAnnual = expectedMonthly * 12M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: weeklyAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: weeklyAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: weeklyAmount);

            AssertInRange(weeklyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: weeklyAmount);

            AssertInRange(expectedTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: weeklyAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: weeklyAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: weeklyAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: weeklyAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Weekly,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: weeklyAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromEveryTwoWeeks()
        {
            decimal baseAmount = 1000M;
            decimal expectedDailyAmount = baseAmount / 10M;
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedWeekly = baseAmount / 2M;
            decimal expectedMonthly = (baseAmount * 26M) / 12M;
            decimal expectedSemiMonthly = expectedMonthly / 2M;
            decimal expectedQuarterly = expectedMonthly * 3M;
            decimal expectedSemiAnnual = expectedMonthly * 6M;
            decimal expectedAnnual = expectedMonthly * 12M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: baseAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromSemiMonthly()
        {
            decimal baseAmount = 1000M;
            decimal expectedDailyAmount = (baseAmount * 24M) / (52M * 5M);
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedWeekly = expectedDailyAmount * 5M;
            decimal expectedMonthly = baseAmount * 2M;
            decimal expectedEveryTwoWeeks = (baseAmount * 24M) / 26M;
            decimal expectedQuarterly = expectedMonthly * 3M;
            decimal expectedSemiAnnual = expectedMonthly * 6M;
            decimal expectedAnnual = expectedMonthly * 12M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: baseAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromMonthly()
        {
            decimal baseAmount = 2000M;
            decimal expectedDailyAmount = (baseAmount * 12M) / (52M * 5M);
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedWeekly = expectedDailyAmount * 5M;
            decimal expectedSemiMonthly = (baseAmount * 12M) / 24M;
            decimal expectedEveryTwoWeeks = (baseAmount * 12M) / 26M;
            decimal expectedQuarterly = baseAmount * 3M;
            decimal expectedSemiAnnual = baseAmount * 6M;
            decimal expectedAnnual = baseAmount * 12M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Monthly,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: baseAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromQuarterly()
        {
            decimal baseAmount = 10000M;
            decimal expectedDailyAmount = (baseAmount * 4M) / (52M * 5M);
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedWeekly = expectedDailyAmount * 5M;
            decimal expectedSemiMonthly = (baseAmount * 4) / 24M;
            decimal expectedEveryTwoWeeks = (baseAmount * 4) / 26M;
            decimal expectedMonthly = baseAmount * 4M / 12M;
            decimal expectedSemiAnnual = expectedMonthly * 6M;
            decimal expectedAnnual = expectedMonthly * 12M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Quarterly,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: baseAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromSemiAnnually()
        {
            decimal baseAmount = 25000M;
            decimal expectedDailyAmount = (baseAmount * 2M) / (52M * 5M);
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedWeekly = expectedDailyAmount * 5M;
            decimal expectedSemiMonthly = (baseAmount * 2M) / 24M;
            decimal expectedEveryTwoWeeks = (baseAmount * 2M) / 26M;
            decimal expectedMonthly = baseAmount * 2M / 12M;
            decimal expectedQuarterly = expectedMonthly * 3M;
            decimal expectedAnnual = expectedMonthly * 12M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: baseAmount);

            AssertInRange(expectedAnnual, answer);
        }

        [Fact]
        public void WageDollarTransformations_TransformationFromAnnually()
        {
            decimal baseAmount = 50000M;
            decimal expectedDailyAmount = baseAmount / (52M * 5M);
            decimal expectedHourlyAmount = expectedDailyAmount / 8M;
            decimal expectedWeekly = expectedDailyAmount * 5M;
            decimal expectedSemiMonthly = baseAmount / 24M;
            decimal expectedEveryTwoWeeks = baseAmount / 26M;
            decimal expectedMonthly = baseAmount / 12M;
            decimal expectedQuarterly = expectedMonthly * 3M;
            decimal expectedSemiAnnually = baseAmount / 2M;

            decimal answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.Hourly,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.Daily,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.Weekly,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.EveryTwoWeeks,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.SemiMonthly,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.Monthly,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.Quarterly,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.SemiAnnually,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnually, answer);

            answer = DollarsByFrequency.Convert(sourceFrequency: DollarsByFrequency.Frequency.Annually,
                targetFrequency: DollarsByFrequency.Frequency.Annually,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);
        }
    }
}

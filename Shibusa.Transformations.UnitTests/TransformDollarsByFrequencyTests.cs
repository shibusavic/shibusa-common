using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class TransformDollarsByFrequencyTests
    {
        [Fact]
        public void WageDollarTransformations_MatchingFrequencies()
        {
            decimal dailyAmount = 1000M;

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: dailyAmount);

            Assert.Equal(dailyAmount, answer);
        }

        private static void AssertInRange(decimal expectedAnswer, decimal answer, decimal errorRange = .01M)
        {
            var high = expectedAnswer * (1 + errorRange);
            var low = expectedAnswer * (1 - errorRange);

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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: hourlyAmount);

            AssertInRange(hourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: hourlyAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: hourlyAmount);

            AssertInRange(expectedWeeklyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: hourlyAmount);

            AssertInRange(expectedTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: hourlyAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: hourlyAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: hourlyAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: hourlyAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: dailyAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: dailyAmount);

            AssertInRange(dailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: dailyAmount);

            AssertInRange(expectedWeeklyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: dailyAmount);

            AssertInRange(expectedTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: dailyAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: dailyAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: dailyAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: dailyAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: weeklyAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: weeklyAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: weeklyAmount);

            AssertInRange(weeklyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: weeklyAmount);

            AssertInRange(expectedTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: weeklyAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: weeklyAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: weeklyAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: weeklyAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnual, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
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

            decimal answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.HOURLY,
                amount: baseAmount);

            AssertInRange(expectedHourlyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.DAILY,
                amount: baseAmount);

            AssertInRange(expectedDailyAmount, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.WEEKLY,
                amount: baseAmount);

            AssertInRange(expectedWeekly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.EVERY_TWO_WEEKS,
                amount: baseAmount);

            AssertInRange(expectedEveryTwoWeeks, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedSemiMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.MONTHLY,
                amount: baseAmount);

            AssertInRange(expectedMonthly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.QUARTERLY,
                amount: baseAmount);

            AssertInRange(expectedQuarterly, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.SEMI_ANNUALLY,
                amount: baseAmount);

            AssertInRange(expectedSemiAnnually, answer);

            answer = TransformDollarsByFrequency.Convert(sourceFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                targetFrequency: TransformDollarsByFrequency.Frequency.ANNUALLY,
                amount: baseAmount);

            AssertInRange(baseAmount, answer);
        }
    }
}

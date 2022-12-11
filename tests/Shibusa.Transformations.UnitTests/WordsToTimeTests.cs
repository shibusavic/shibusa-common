using Shibusa.Extensions;
using Xunit;

namespace Shibusa.Transformations.UnitTests;

public class WordsToTimeTests
{
    [Fact]
    public void ParseWords_TransformWordsToTime_2DaysAgo()
    {
        DateTime expected = DateTime.Now.AddDays(-2).StartOfDay();
        DateTime twoWeeksAgo = WordsToTime.ParseWords("2 days ago").StartOfDay();

        Assert.Equal(expected, twoWeeksAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_1DayAgo()
    {
        DateTime expected = DateTime.Now.AddDays(-1).StartOfDay();
        DateTime oneWeekAgo = WordsToTime.ParseWords("1 day ago").StartOfDay();

        Assert.Equal(expected, oneWeekAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_2WeeksAgo()
    {
        DateTime expected = DateTime.Now.AddDays(-14).StartOfDay();
        DateTime twoWeeksAgo = WordsToTime.ParseWords("2 weeks ago").StartOfDay();

        Assert.Equal(expected, twoWeeksAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_1WeekAgo()
    {
        DateTime expected = DateTime.Now.AddDays(-7).StartOfDay();
        DateTime oneWeekAgo = WordsToTime.ParseWords("1 week ago").StartOfDay();

        Assert.Equal(expected, oneWeekAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_2MonthsAgo()
    {
        DateTime expected = DateTime.Now.AddMonths(-2).StartOfDay();
        DateTime twoWeeksAgo = WordsToTime.ParseWords("2 months ago").StartOfDay();

        Assert.Equal(expected, twoWeeksAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_1MonthAgo()
    {
        DateTime expected = DateTime.Now.AddMonths(-1).StartOfDay();
        DateTime oneWeekAgo = WordsToTime.ParseWords("1 month ago").StartOfDay();

        Assert.Equal(expected, oneWeekAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_2YearsAgo()
    {
        DateTime expected = DateTime.Now.AddYears(-2).StartOfDay();
        DateTime twoWeeksAgo = WordsToTime.ParseWords("2 years ago").StartOfDay();

        Assert.Equal(expected, twoWeeksAgo);
    }

    [Fact]
    public void ParseWords_TransformWordsToTime_1YearAgo()
    {
        DateTime expected = DateTime.Now.AddYears(-1).StartOfDay();
        DateTime oneWeekAgo = WordsToTime.ParseWords("1 year ago").StartOfDay();

        Assert.Equal(expected, oneWeekAgo);
    }
}

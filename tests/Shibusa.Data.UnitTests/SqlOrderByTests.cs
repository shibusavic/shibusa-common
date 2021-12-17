using Shibusa.Data.Abstractions;
using Xunit;

namespace Shibusa.Data.UnitTests
{
    public class SqlOrderByTests
    {
        [Fact]
        public void Create_Empty_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, SqlOrderBy.Create(columns: new Dictionary<string, SortOrder>()));
        }

        [Fact]
        public void Create_Single_ReturnsOnePhrase()
        {
            var sortKvp = new KeyValuePair<string, SortOrder>("TestCol", SortOrder.Ascending);
            Assert.Equal($"{sortKvp.Key} ASC", SqlOrderBy.Create(sortKvp));
        }

        [Fact]
        public void Create_Multiple_ReturnsCombinedPhrase()
        {
            var cols = new Dictionary<string, SortOrder>
            {
                { "TestCol", SortOrder.Ascending },
                { "OtherCol", SortOrder.Descending }
            };

            Assert.Equal($"TestCol ASC, OtherCol DESC", SqlOrderBy.Create(cols));
        }
    }
}

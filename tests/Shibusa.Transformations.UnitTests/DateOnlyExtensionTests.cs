using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class DateOnlyExtensionTests
    {
        [Fact]
        public void ToDateTime_Unspecified()
        {
            DateOnly date = new DateOnly(2000, 1, 1);
            DateTime expectedDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            DateTime dateTime = date.ToDateTime();
            Assert.Equal(expectedDate, dateTime);
        }

        [Fact]
        public void ToDateTime_Local()
        {
            DateOnly date = new DateOnly(2000, 1, 1);
            DateTime expectedDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime dateTime1 = date.ToLocalDateTime();
            DateTime dateTime2 = date.ToDateTime(DateTimeKind.Local);
            Assert.Equal(expectedDate, dateTime1);
            Assert.Equal(expectedDate, dateTime2);
        }

        [Fact]
        public void ToDateTime_Utc()
        {
            DateOnly date = new DateOnly(2000, 1, 1);
            DateTime expectedDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime1 = date.ToUtcDateTime();
            DateTime dateTime2 = date.ToDateTime(DateTimeKind.Utc);
            Assert.Equal(expectedDate, dateTime1);
            Assert.Equal(expectedDate, dateTime2);
        }
    }
}

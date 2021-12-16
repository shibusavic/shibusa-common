using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class UnitedStatesPhoneFormatterTests
    {
        [Theory]
        [InlineData("1234", "1234")]
        [InlineData("5671234", "567-1234")]
        [InlineData("567.1234", "567-1234")]
        [InlineData("567-1234", "567-1234")]
        [InlineData("5.6.7-1&2p3j4kk", "567-1234")]
        [InlineData("1235671234", "(123) 567-1234")]
        public void FormatPhoneNumber(string input, string expected)
        {
            Assert.Equal(expected, string.Format(new UnitedStatesPhoneFormatter(), "{0}", input));
        }

        [Theory]
        [InlineData("1234", "1234")]
        [InlineData("5671234", "5671234")]
        [InlineData("567.1234", "5671234")]
        [InlineData("567-1234", "5671234")]
        [InlineData("5.6.7-1&2p3j4kk", "5671234")]
        [InlineData("1235671234", "1235671234")]
        public void FormatPhoneNumber_NumbersOnly(string input, string expected)
        {
            Assert.Equal(expected, string.Format(new UnitedStatesPhoneFormatter(), "{0:N}", input));
        }

        [Theory]
        [InlineData("1234", "1234")]
        [InlineData("5671234", "567.1234")]
        [InlineData("567.1234", "567.1234")]
        [InlineData("567-1234", "567.1234")]
        [InlineData("5.6.7-1&2p3j4kk", "567.1234")]
        [InlineData("1235671234", "123.567.1234")]
        public void FormatPhoneNumberWithDots(string input, string expected)
        {
            Assert.Equal(expected, string.Format(new UnitedStatesPhoneFormatter(), "{0:dots}", input));
        }

        [Fact]
        public void FormatPhoneNumberWithCountryCode()
        {
            string expected = "+1 (123) 456-7890";
            string input = "1234567890"; // exactly 10
            Assert.Equal(expected, string.Format(new UnitedStatesPhoneFormatter(), "{0:I}", input));

            input = "123456789"; // less than 10
            Assert.Throws<FormatException>(() => string.Format(new UnitedStatesPhoneFormatter(), "{0:I}", input));
            input = "123456789111"; // more that 10
            Assert.Throws<FormatException>(() => string.Format(new UnitedStatesPhoneFormatter(), "{0:I}", input));
        }

        [Fact]
        public void FormatPhoneNumberWithCountryCodeAndDots()
        {
            string expected = "+1.123.456.7890";
            string input = "1234567890"; // exactly 10
            Assert.Equal(expected, string.Format(new UnitedStatesPhoneFormatter(), "{0:Idots}", input));

            input = "123456789"; // less than 10
            Assert.Throws<FormatException>(() => string.Format(new UnitedStatesPhoneFormatter(), "{0:Idots}", input));
            input = "123456789111"; // more that 10
            Assert.Throws<FormatException>(() => string.Format(new UnitedStatesPhoneFormatter(), "{0:Idots}", input));
        }
    }
}

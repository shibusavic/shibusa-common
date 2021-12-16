using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class SocialSecurityNumberFormatterTests
    {
        [Theory]
        [InlineData("123456789", "123-45-6789")]
        [InlineData("123-45-6789", "123-45-6789")]
        [InlineData("123 45 6789", "123-45-6789")]
        [InlineData("123.45.6789", "123-45-6789")]
        public void FormatSsn(string input, string expected)
        {
            Assert.Equal(expected, string.Format(new SocialSecurityNumberFormatter(), "{0}", input));
        }

        [Theory]
        [InlineData("123456789", "123.45.6789")]
        [InlineData("123-45-6789", "123.45.6789")]
        [InlineData("123 45 6789", "123.45.6789")]
        [InlineData("123.45.6789", "123.45.6789")]
        public void FormatSsnWithDots(string input, string expected)
        {
            Assert.Equal(expected, string.Format(new SocialSecurityNumberFormatter(), "{0:dots}", input));
        }

        [Theory]
        [InlineData("123456789", "123456789")]
        [InlineData("123-45-6789", "123456789")]
        [InlineData("123 45 6789", "123456789")]
        [InlineData("123.45.6789", "123456789")]
        public void FormatSsnWithOnlyNumbers(string input, string expected)
        {
            Assert.Equal(expected, string.Format(new SocialSecurityNumberFormatter(), "{0:N}", input));
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("1234567890")]
        [InlineData("123-45-678")]
        [InlineData("123-456.7890")]
        public void InvalidFormat(string input)
        {
            Assert.Throws<FormatException>(() => string.Format(new SocialSecurityNumberFormatter(), "{0}", input));
        }
    }
}

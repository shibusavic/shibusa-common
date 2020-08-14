using Xunit;

namespace Shibusa.Validators.UnitTests
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("123", false)]
        [InlineData("1234", true)]
        [InlineData("12344", false)]
        [InlineData("1231234", true)]
        [InlineData("123-1234", true)]
        [InlineData("123.1234", true)]
        [InlineData("1231231234", true)]
        [InlineData("1231231234000", false)]
        [InlineData("(123) 123-1234", true)]
        [InlineData("123.123.1234", true)]
        [InlineData("+1-123-123-1234", true)]
        [InlineData("+1 (123) 123-1234", true)]
        public void ValidatePhoneNumber(string input, bool isValid)
        {
            if (isValid)
            {
                Assert.True(UnitedStatesPhoneNumber.IsValidStructure(input, 4, 7, 10, 11));
            }
            else
            {
                Assert.False(UnitedStatesPhoneNumber.IsValidStructure(input, 4, 7, 10, 11));
            }
        }
    }
}

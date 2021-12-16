using Xunit;

namespace Shibusa.Validators.UnitTests
{
    /// <summary>
    /// <seealso cref="http://codefool.tumblr.com/post/15288874550/list-of-valid-and-invalid-email-addresses"/>
    /// <seealso cref="https://en.wikipedia.org/wiki/Email_address"/>
    /// <seealso cref="https://www.w3.org/Protocols/rfc822/3_Lexical.html"/>
    /// <seealso cref="https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/"/>
    /// </summary>
    public class EmailAddressTest
    {
        [Theory]
        [InlineData("email@example.com")]
        [InlineData("firstname.lastname@example.com")]
        [InlineData("email@subdomain.example.com")]
        [InlineData("firstname+lastname@example.com")]
        [InlineData("email@123.123.123.123")]
        [InlineData("email@[123.123.123.123]")]
        [InlineData("“email”@example-specquote.com")]
        [InlineData("\"email\"@example-quote.com")]
        [InlineData("1234567890@example.com")]
        [InlineData("email@example-no-quote.com")]
        [InlineData("_______@example.com")]
        [InlineData("email@example.name")]
        [InlineData("email@example.museum")]
        [InlineData("email@example.co.jp")]
        [InlineData("firstname-lastname@example.com")]
        [InlineData("myemail@cover.me")]
        public void ValidEmail_Valid(string email)
        {
            Assert.True(EmailAddress.IsValid(email));
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("#@%^%#$@#$@#.com")]
        [InlineData("@example.com")]
        [InlineData("Joe Smith <email@example.com>")]
        [InlineData("email.example.com")]
        [InlineData("email@example@example.com")]
        [InlineData(".email@example.com")]
        [InlineData("email.@example.com")]
        [InlineData("email..email@example.com")]
        [InlineData("email@example.com (Joe Smith)")]
        [InlineData("email@example")]
        [InlineData("email@-example.com")]
        [InlineData("email@111.222.333.44444")]
        [InlineData("email@example..com")]
        [InlineData("Abc..123@example.com")]
        public void InvalidEmail_NotValid(string email)
        {
            Assert.False(EmailAddress.IsValid(email));
        }
    }
}

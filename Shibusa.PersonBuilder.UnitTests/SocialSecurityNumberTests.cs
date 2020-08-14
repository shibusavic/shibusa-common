using Xunit;

namespace Shibusa.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void GenerateFakeSsn()
        {
            var person = new PersonBuilder()
                .WithFakeSsn()
                .Build();

            Assert.True(Validators.SocialSecurityNumber.IsValidStructure(person.SocialSecurityNumber));
            Assert.False(Validators.SocialSecurityNumber.IsValid(person.SocialSecurityNumber));
        }

        [Fact]
        public void GeerateRealisticSsn()
        {
            var person = new PersonBuilder()
                .WithRealisticSsn()
                .Build();

            Assert.True(Validators.SocialSecurityNumber.IsValidStructure(person.SocialSecurityNumber));
            Assert.True(Validators.SocialSecurityNumber.IsValid(person.SocialSecurityNumber));
        }
    }
}

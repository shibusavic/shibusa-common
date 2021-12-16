using Xunit;

namespace Shibusa.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void PassPhoneNumber()
        {
            Person? person = new PersonBuilder()
                    .WithPhone("1234567890")
                    .Build();

            Assert.True(Validators.UnitedStatesPhoneNumber.IsValidStructure(person.PhoneNumber));
        }

        [Fact]
        public void GeneratePhoneNumber()
        {
            Person? person = new PersonBuilder()
                .WithPhone()
                .Build();

            Assert.True(Validators.UnitedStatesPhoneNumber.IsValidStructure(person.PhoneNumber));
        }
    }
}

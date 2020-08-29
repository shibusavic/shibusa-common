using Xunit;

namespace Shibusa.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void WithName_Explicit()
        {
            Person person = new PersonBuilder()
                .WithName(new PersonName(firstName: "Mike", lastName: "Michaels"))
                .Build();

            Assert.Equal("Mike", person.Name.FirstName);
            Assert.Equal("Michaels", person.Name.LastName);
            Assert.Null(person.Name.MiddleName);
            Assert.Null(person.Name.MiddleInitial);
            Assert.Null(person.Name.Suffix);
        }

        [Fact]
        public void WithName_Gender()
        {
            Person person = new PersonBuilder()
                .WithName(Gender.Male)
                .Build();

            Assert.NotNull(person.Name.FirstName);
            Assert.NotNull(person.Name.LastName);
            Assert.Null(person.Name.MiddleName);
            Assert.Null(person.Name.MiddleInitial);
            Assert.Null(person.Name.Suffix);
        }

        [Fact]
        public void WithName_GenderOther()
        {
            Person person = new PersonBuilder()
                .WithName(Gender.Other)
                .Build();

            Assert.NotNull(person.Name.FirstName);
            Assert.NotNull(person.Name.LastName);
            Assert.Null(person.Name.MiddleName);
            Assert.Null(person.Name.MiddleInitial);
            Assert.Null(person.Name.Suffix);
        }

        [Fact]
        public void WithName_ExcludeMiddle()
        {
            Person person = new PersonBuilder()
                .WithName(Gender.Female, false)
                .Build();

            Assert.NotNull(person.Name.FirstName);
            Assert.NotNull(person.Name.LastName);
            Assert.Null(person.Name.MiddleName);
            Assert.Null(person.Name.MiddleInitial);
            Assert.Null(person.Name.Suffix);
        }

        [Fact]
        public void WithName_Random()
        {
            Person person = new PersonBuilder()
                .WithName()
                .Build();

            Assert.NotNull(person.Name.FirstName);
            Assert.NotNull(person.Name.LastName);
            Assert.Null(person.Name.MiddleName);
            Assert.Null(person.Name.MiddleInitial);
            Assert.Null(person.Name.Suffix);
        }
    }
}

using System.Linq;
using Xunit;

namespace Shibusa.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void BuildRandomFemale()
        {
            Person person = new PersonBuilder()
                .WithAge(24)
                .WithGender(Gender.Female)
                .WithName(includeMiddleName: true)
                .WithRaces(minimum: 1, maximum: 2)
                .WithEthnicity(PersonBuilder.Constants.Ethnicity.NOT_HISPANIC)
                .Build();

            Assert.Equal<int>(24, person.Age);
            Assert.Equal(Gender.Female, person.Gender);
            Assert.NotNull(person.Name.FirstName);
            Assert.NotNull(person.Name.LastName);
            Assert.NotNull(person.Name.MiddleInitial);
            Assert.NotNull(person.Name.MiddleName);
            Assert.Equal(PersonBuilder.Constants.Ethnicity.NOT_HISPANIC, person.Ethnicity);
            Assert.True(condition: person.Races.Any() && person.Races.Count() <= 2);
        }
    }
}

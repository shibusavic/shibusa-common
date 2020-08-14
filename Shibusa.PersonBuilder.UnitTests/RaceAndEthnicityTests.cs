using System.Linq;
using Xunit;

namespace Shibusa.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void GetEthnicities()
        {
            var ethnicities = PersonBuilder.Constants.Ethnicity.GetEthnicities();
            Assert.Contains(PersonBuilder.Constants.Ethnicity.HISPANIC, ethnicities);
        }

        [Fact]
        public void WithEthnicity()
        {
            var person = new PersonBuilder()
                .WithEthnicity(PersonBuilder.Constants.Ethnicity.HISPANIC)
                .Build();

            Assert.Equal(PersonBuilder.Constants.Ethnicity.HISPANIC, person.Ethnicity);
        }

        [Fact]
        public void GetRaces()
        {
            var races = PersonBuilder.Constants.Race.GetRaces();
            Assert.Contains(PersonBuilder.Constants.Race.WHITE, races);
        }

        [Fact]
        public void WithRaces_Single()
        {
            var person = new PersonBuilder()
                .WithRace(PersonBuilder.Constants.Race.HAWAIIAN)
                .Build();

            Assert.Single(person.Races);
            Assert.Contains(PersonBuilder.Constants.Race.HAWAIIAN, person.Races);
        }

        [Fact]
        public void WithRaces_Multiple()
        {
            var person = new PersonBuilder()
                .WithRaces(minimum: 2, maximum: 4)
                .Build();


            Assert.True(person.Races.Count() >= 2 && person.Races.Count() <= 4);
        }

        [Fact]
        public void WithRaces_NoDupes()
        {
            int numberOfRaces = PersonBuilder.Constants.Race.GetRaces().Count();

            for (int i = 0; i < 100; i++)
            {
                var person = new PersonBuilder()
                    .WithRaces(minimum: numberOfRaces, maximum: numberOfRaces)
                    .Build();

                Assert.Equal(numberOfRaces, person.Races.Count());
                var distinctRaces = person.Races.Distinct();
                Assert.Equal(numberOfRaces, distinctRaces.Count());
            }
        }
    }
}

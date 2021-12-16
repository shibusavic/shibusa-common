using Xunit;

namespace Shibusa.PersonBuilder.UnitTests
{
    public partial class PersonBuilderTests
    {
        [Fact]
        public void GetEthnicities()
        {
            IEnumerable<string?>? ethnicities = PersonBuilder.Constants.Ethnicity.GetEthnicities();
            Assert.Contains(PersonBuilder.Constants.Ethnicity.HISPANIC, ethnicities);
        }

        [Fact]
        public void WithEthnicity()
        {
            Person? person = new PersonBuilder()
                .WithEthnicity(PersonBuilder.Constants.Ethnicity.HISPANIC)
                .Build();

            Assert.Equal(PersonBuilder.Constants.Ethnicity.HISPANIC, person.Ethnicity);
        }

        [Fact]
        public void GetRaces()
        {
            IEnumerable<string?>? races = PersonBuilder.Constants.Race.GetRaces();
            Assert.Contains(PersonBuilder.Constants.Race.White, races);
        }

        [Fact]
        public void WithRaces_Single()
        {
            Person? person = new PersonBuilder()
                .WithRace(PersonBuilder.Constants.Race.Hawaiian)
                .Build();

            Assert.Single(person.Races);
            Assert.Contains(PersonBuilder.Constants.Race.Hawaiian, person.Races);
        }

        [Fact]
        public void WithRaces_Multiple()
        {
            Person? person = new PersonBuilder()
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
                Person? person = new PersonBuilder()
                    .WithRaces(minimum: numberOfRaces, maximum: numberOfRaces)
                    .Build();

                Assert.Equal(numberOfRaces, person.Races.Count());
                IEnumerable<string>? distinctRaces = person.Races.Distinct();
                Assert.Equal(numberOfRaces, distinctRaces.Count());
            }
        }
    }
}

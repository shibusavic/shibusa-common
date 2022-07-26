namespace Shibusa.PersonBuilder
{
    public partial class PersonBuilder
    {
        /// <summary>
        /// Sets the race for this person.
        /// </summary>
        /// <param name="race"></param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithRace(string race)
        {
            if (!Constants.Race.GetRaces().Contains(race)) { throw new ArgumentException($"Race '{race}' is not valid."); }
            person.Races = new List<string>() { race };
            return this;
        }

        /// <summary>
        /// Sets multiple races for this person.
        /// </summary>
        /// <param name="races"></param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithRaces(IEnumerable<string> races)
        {
            races.ToList().ForEach(r =>
            {
                if (!Constants.Race.GetRaces().Contains(r)) { throw new ArgumentException($"Race '{r}' is not valid."); }
            });

            person.Races = new List<string>(races);

            return this;
        }

        /// <summary>
        /// Generates a collection of random races for this person.
        /// </summary>
        /// <param name="minimum">The minimum number of races to generate.</param>
        /// <param name="maximum">The maximum number of races to generate.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithRaces(int minimum, int maximum)
        {
            if (maximum < minimum) { throw new ArgumentNullException($"{minimum} must be less than {maximum}"); }

            if (minimum > 0)
            {
                int numberOfRacesAdded = 0;
                List<string?> listOfRaces = Constants.Race.GetRaces().ToList();
                List<string> racesAdded = new();
                while (numberOfRacesAdded < minimum && numberOfRacesAdded <= maximum && listOfRaces.Any())
                {
                    int indexOfSelection = random.Next(0, listOfRaces.Count);
                    if (listOfRaces[indexOfSelection] != null)
                    {
                        racesAdded.Add(listOfRaces[indexOfSelection] ?? throw new Exception("Cannot add null to list of races."));
                    }
                    listOfRaces.RemoveAt(indexOfSelection);
                    numberOfRacesAdded++;
                }
                person.Races = new List<string>(racesAdded);
            }

            return this;
        }

        /// <summary>
        /// Sets the ethnicity for this person.
        /// </summary>
        /// <param name="ethnicity">The ethnicity to set.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithEthnicity(string ethnicity)
        {
            if (!Constants.Ethnicity.GetEthnicities().Contains(ethnicity)) { throw new ArgumentException($"Ethnicity '{ethnicity}' is not valid."); }
            person.Ethnicity = ethnicity;
            return this;
        }

        /// <summary>
        /// Gives this person the Hispanic ethnicity.
        /// </summary>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder AsHispanic()
        {
            person.Ethnicity = Constants.Ethnicity.HISPANIC;
            return this;
        }

        /// <summary>
        /// Sets a random ethnicity to this person (including none at all as an option).
        /// </summary>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithRandomEthnicity()
        {
            person.Ethnicity = random.Next(0, 3) switch
            {
                0 => Constants.Ethnicity.HISPANIC,
                1 => Constants.Ethnicity.NOT_HISPANIC,
                _ => default
            };
            return this;
        }

        /// <summary>
        /// Race and ethnicity constants.
        /// </summary>
        public static partial class Constants
        {
            /// <summary>
            /// Race constants.
            /// </summary>
            public static class Race
            {
                public const string White = "White";
                public const string Black = "Black";
                public const string AmericanIndian = "American Indian / Alaska Native";
                public const string Hawaiian = "Hawaiian Native / Pacific Islander";
                public const string Other = "Other";

                public static IEnumerable<string?> GetRaces()
                {
                    return typeof(Race).GetFields().Select(f => f.GetRawConstantValue()?.ToString());
                }
            }

            /// <summary>
            /// Ethnicity constants.
            /// </summary>
            public static class Ethnicity
            {
                public const string HISPANIC = "Hispanic/Latino";
                public const string NOT_HISPANIC = "Not Hispanic/Latino";

                public static IEnumerable<string?> GetEthnicities()
                {
                    return typeof(Ethnicity).GetFields().Select(f => f.GetRawConstantValue()?.ToString());
                }
            }
        }
    }
}

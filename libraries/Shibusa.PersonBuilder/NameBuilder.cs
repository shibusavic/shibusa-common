namespace Shibusa.PersonBuilder
{
    public partial class PersonBuilder
    {
        /// <summary>
        /// Add a name to this person.
        /// </summary>
        /// <param name="personName">The <see cref="PersonName"/> object to add.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithName(PersonName personName)
        {
            person.Name = personName;
            return this;
        }

        /// <summary>
        /// Add a name to this person.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <param name="middleName">The person's middle name.</param>
        /// <param name="suffix">The suffix of the person's name.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithName(string firstName,
            string lastName,
            string? middleName = null,
            string? suffix = null)
        {
            return WithName(new PersonName(firstName: firstName,
                lastName: lastName,
                middleName: middleName,
                suffix: suffix));
        }

        /// <summary>
        /// Generates a random person name for this person.
        /// </summary>
        /// <param name="gender">The gender of the person.</param>
        /// <param name="includeMiddleName">An indicator of whether a middle name should be generated.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithName(Gender gender = Gender.Other, bool includeMiddleName = false)
        {
            if (gender == Gender.Other)
            {
                gender = (Gender)random.Next(0, 2);
            }

            if (person.Gender != Gender.Other)
            {
                gender = person.Gender;
            }

            string surname = surnames.ElementAt(random.Next(0, surnameCount));
            string firstName = gender switch
            {
                Gender.Male => maleNames.ElementAt(random.Next(0, maleNameCount)),
                _ => femaleNames.ElementAt(random.Next(0, femaleNameCount))
            };
            string? middleName = includeMiddleName ? gender switch
            {
                Gender.Male => maleNames.ElementAt(random.Next(0, maleNameCount)),
                _ => femaleNames.ElementAt(random.Next(0, femaleNameCount))
            } : null;

            return WithName(firstName: firstName, lastName: surname, middleName: middleName);
        }
    }
}

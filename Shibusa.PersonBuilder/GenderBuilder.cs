namespace Shibusa.PersonBuilder
{
    public partial class PersonBuilder
    {
        /// <summary>
        /// Set this person's gender.
        /// </summary>
        /// <param name="gender">The gender of the person.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithGender(Gender gender)
        {
            person.Gender = gender;
            return this;
        }

        /// <summary>
        /// Generates a random gender for this person.
        /// </summary>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithGender()
        {
            person.Gender = (Gender)random.Next(0, 3);
            return this;
        }
    }
}

namespace Shibusa.PersonBuilder
{
    public partial class PersonBuilder
    {
        /// <summary>
        /// Set the person's age.
        /// </summary>
        /// <param name="age">The age to set for the person.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithAge(int age)
        {
            person.Age = age;
            return this;
        }

        /// <summary>
        /// Sets the perons's date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            person.DateOfBirth = dateOfBirth;
            return this;
        }
    }
}

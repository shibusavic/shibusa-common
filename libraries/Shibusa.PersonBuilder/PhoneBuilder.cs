using Shibusa.Transformations;

namespace Shibusa.PersonBuilder
{
    public partial class PersonBuilder
    {
        /// <summary>
        /// Generates a random phone number.
        /// </summary>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithPhone()
        {
            int areaCode = random.Next(200, 1000);
            int prefix = random.Next(100, 1000);
            int lineNumber = random.Next(1, 10000);

            string phoneNumber = $"1{areaCode}{prefix}{lineNumber}";

            return WithPhone(phoneNumber);
        }

        /// <summary>
        /// Sets the person's phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="validCounts">An array of counts that are valid (e.g., 4, 7, 10).</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithPhone(string phoneNumber, params int[] validCounts)
        {
            if (Validators.UnitedStatesPhoneNumber.IsValidStructure(phoneNumber, validCounts))
            {
                person.PhoneNumber = phoneNumber.ToString(new UnitedStatesPhoneFormatter());
                return this;
            }
            else
            {
                throw new ArgumentNullException($"Invalid phone number: {phoneNumber}");
            }
        }
    }
}

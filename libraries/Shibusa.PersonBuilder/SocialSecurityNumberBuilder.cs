namespace Shibusa.PersonBuilder
{
    public partial class PersonBuilder
    {
        /// <summary>
        /// Add a Social Security Number to this person.
        /// </summary>
        /// <param name="socialSecurityNumber">The Social Security Number.</param>
        /// <param name="requireValidNumber">An indicator of whether the generated number needs to
        /// have a  valid area.</param>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithSsn(string socialSecurityNumber, bool requireValidNumber = false)
        {
            if (Validators.SocialSecurityNumber.IsValidStructure(socialSecurityNumber))
            {
                if (requireValidNumber)
                {
                    if (Validators.SocialSecurityNumber.IsValid(socialSecurityNumber))
                    {
                        person.SocialSecurityNumber = socialSecurityNumber;
                        return this;
                    }
                    else
                    {
                        throw new ArgumentException($"{socialSecurityNumber} is not a valid Social Security Number.");
                    }
                }
                else
                {
                    person.SocialSecurityNumber = socialSecurityNumber;
                    return this;
                }
            }
            else
            {
                throw new ArgumentException($"{socialSecurityNumber} is not a properly structured Social Security Number.");
            }
        }

        /// <summary>
        /// Add a generated Social Security Number with an unused area.
        /// </summary>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithFakeSsn()
        {
            int areaIndex = random.Next(0, Validators.SocialSecurityNumber.UnusedAreas.Count());
            string area = Validators.SocialSecurityNumber.UnusedAreas.ElementAt(areaIndex).ToString().PadLeft(3, '0');
            string group = random.Next(1, 100).ToString().PadLeft(2, '0');
            string series = random.Next(1, 10000).ToString().PadLeft(4, '0');

            person.SocialSecurityNumber = $"{area}{group}{series}"
                .ToString(new Transformations.SocialSecurityNumberFormatter());

            return this;
        }

        /// <summary>
        /// Add a generated Social Security Number with an proper area.
        /// </summary>
        /// <returns>A reference to this <see cref="PersonBuilder"/> instance.</returns>
        public PersonBuilder WithRealisticSsn()
        {
            int areaIndex = random.Next(0, Validators.SocialSecurityNumber.UsedAreas.Count());
            string area = Validators.SocialSecurityNumber.UsedAreas.ElementAt(areaIndex).ToString().PadLeft(3, '0');
            string group = random.Next(1, 100).ToString().PadLeft(2, '0');
            string series = random.Next(1, 10000).ToString().PadLeft(4, '0');

            person.SocialSecurityNumber = $"{area}{group}{series}"
                .ToString(new Transformations.SocialSecurityNumberFormatter());

            return this;
        }
    }
}

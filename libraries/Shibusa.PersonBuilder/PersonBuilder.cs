using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Shibusa.PersonBuilder
{
    /// <summary>
    /// Represents a random person generator.
    /// </summary>
    public partial class PersonBuilder
    {
        protected readonly static HashSet<string> maleNames = new();
        protected readonly static HashSet<string> femaleNames = new();
        protected readonly static HashSet<string> surnames = new();
        protected readonly static HashSet<string> races = new();
        protected readonly static HashSet<string> ethnicities = new();

        protected readonly static int maleNameCount = 0;
        protected readonly static int femaleNameCount = 0;
        protected readonly static int surnameCount = 0;

        protected readonly static Random random = new(DateTime.Now.Millisecond);

        protected Person person;

        /// <summary>
        /// Creates a new instance of the <see cref="PersonBuilder"/> class.
        /// </summary>
        public PersonBuilder()
        {
            person = new Person();
        }

        /// <summary>
        /// Static constructor to load up embedded resources.
        /// </summary>
        static PersonBuilder()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream? maleNamesFile = assembly.GetManifestResourceStream("Shibusa.PersonBuilder.Resources.male-names.txt");
            Stream? femaleNamesFile = assembly.GetManifestResourceStream("Shibusa.PersonBuilder.Resources.female-names.txt");
            Stream? surnamesFile = assembly.GetManifestResourceStream("Shibusa.PersonBuilder.Resources.surnames.txt");

            byte[] buffer;
            string fileContent;
            string[] names;

            if (maleNamesFile != null)
            {
                buffer = new byte[maleNamesFile.Length];
                maleNamesFile.ReadAsync(buffer, 0, buffer.Length);
                fileContent = Encoding.UTF8.GetString(buffer).Replace(Environment.NewLine, "|");

                names = fileContent.Split('|');

                foreach (string name in names)
                {
                    maleNames.Add(name);
                }
            }

            if (femaleNamesFile != null)
            {
                buffer = new byte[femaleNamesFile.Length];
                femaleNamesFile.ReadAsync(buffer, 0, buffer.Length);
                fileContent = Encoding.UTF8.GetString(buffer).Replace(Environment.NewLine, "|");

                names = fileContent.Split('|');

                foreach (string name in names)
                {
                    femaleNames.Add(name);
                }
            }

            if (surnamesFile != null)
            {
                buffer = new byte[surnamesFile.Length];
                surnamesFile.ReadAsync(buffer, 0, buffer.Length);
                fileContent = Encoding.UTF8.GetString(buffer).Replace(Environment.NewLine, "|");

                names = fileContent.Split('|');

                foreach (string name in names)
                {
                    surnames.Add(name);
                }
            }

            maleNameCount = maleNames.Count;
            femaleNameCount = femaleNames.Count;
            surnameCount = surnames.Count;

            races = new HashSet<string>() {
                Constants.Race.White,
                Constants.Race.Black,
                Constants.Race.AmericanIndian,
                Constants.Race.Hawaiian,
                Constants.Race.Other
            };

            ethnicities = new HashSet<string>() {
                Constants.Ethnicity.HISPANIC,
                Constants.Ethnicity.NOT_HISPANIC
            };
        }

        /// <summary>
        /// Construct the person object given identified criteria.
        /// </summary>
        /// <returns>An instance of <see cref="Person"/>.</returns>
        public Person Build()
        {
            return person;
        }
    }
}

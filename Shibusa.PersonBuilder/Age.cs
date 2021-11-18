using System;

namespace Shibusa.PersonBuilder
{
    /// <summary>
    /// Represents a person's age.
    /// </summary>
    public readonly struct Age : IEquatable<Age>
    {
        private readonly int age;
        private readonly static Random random = new(DateTime.Now.Millisecond);
        private readonly int? minimum;
        private readonly int? maximum;
        private const int MinValue = 0;
        private const int MaxValue = 120;

        /// <summary>
        /// Creates a new instance of the <see cref="Age"/> struct and explicitely sets age.
        /// </summary>
        /// <param name="age"></param>
        public Age(int age)
        {
            minimum = age;
            maximum = age;
            this.age = age;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Age"/> class and randomnly creates an age.
        /// </summary>
        /// <param name="min">The inclusive minimum age.</param>
        /// <param name="max">The inclusive maximum age.</param>
        private Age(int min = 0, int max = 120)
        {
            minimum = min;
            maximum = max;
            age = random.Next(min, max + 1);
        }

        /// <summary>
        /// Implicitely converts an int to <see cref="Age"/>.
        /// </summary>
        /// <param name="age"></param>
        public static implicit operator Age(int age) => new(age);

        /// <summary>
        /// Implicitely converts an <see cref="Age"/> to an int.
        /// </summary>
        /// <param name="age"></param>
        public static implicit operator int(Age age) => age.age;

        /// <summary>
        /// Creates an instance of <see cref="Age"/> with a value no less than
        /// <paramref name="min"/>.
        /// </summary>
        /// <param name="min">The minimum inclusive age to generate</param>
        /// <returns>An age no less than the value of <paramref name="min"/>.</returns>
        public Age AtLeast(int min)
        {
            return Between(min, maximum ?? MaxValue);
        }

        /// <summary>
        /// Creates an instance of <see cref="Age"/> with a value no greater than
        /// <paramref name="max"/>.
        /// </summary>
        /// <param name="max">The maximum inclusive age to generate</param>
        /// <returns>An age no greater than the value of <paramref name="max"/>.</returns>
        public Age AtMost(int max)
        {
            return Between(minimum ?? MinValue, max);
        }

        /// <summary>
        /// Creates an instance of <see cref="Age"/> with a value no less than <paramref name="min"/>
        /// and no greater than <paramref name="max"/>.
        /// </summary>
        /// <param name="min">The minimum inclusive age to generate</param>
        /// <param name="max">The maximum inclusive age to generate</param>
        /// <returns>An age no less than <paramref name="max"/> and no greater 
        /// than the value of <paramref name="max"/>.</returns>
        public static Age Between(int min, int max)
        {
            return new Age(min: min, max: max);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return age.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Age age && Equals(age);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Age other)
        {
            return age == other.age;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return -1062103556 + age.GetHashCode();
        }

        /// <summary>
        /// Determines the equality of two <see cref="Age"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Age"/>.</param>
        /// <param name="right">The right <see cref="Age"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
        public static bool operator ==(Age left, Age right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines the inequality of two <see cref="Age"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref=""/>.</param>
        /// <param name="right">The right <see cref=""/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
        public static bool operator !=(Age left, Age right)
        {
            return !(left == right);
        }
    }
}


namespace Shibusa.Maths
{
    /// <summary>
    /// Represents a point on a Cartesian graph.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Point"/> struct using integer values.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Point"/> struct using decimal values.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        public Point(decimal x, decimal y)
        {
            X = (double)x;
            Y = (double)y;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Point"/> struct using double values.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// The X position.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y position.
        /// </summary>
        public double Y;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{X:#,##0.00},{Y:#,##0.00}";

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Point point && Equals(point);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Point other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            int hashCode = -180594284;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;        
#else
            return HashCode.Combine(X, Y);
#endif
        }

        /// <summary>
        /// Determines the equality of two <see cref="Point"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Point"/>.</param>
        /// <param name="right">The right <see cref="Point"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines the inequality of two <see cref="Point"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Point"/>.</param>
        /// <param name="right">The right <see cref="Point"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }
    }
}

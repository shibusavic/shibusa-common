namespace Shibusa.Maths
{
    /// <summary>
    /// Represents a line as defined by two <see cref="Point"/> instances.
    /// </summary>
    public struct Line : IEquatable<Line>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Line"/> struct.
        /// </summary>
        /// <param name="point1">The first <see cref="Point"/>.</param>
        /// <param name="point2">The second <see cref="Point"/>.</param>
        public Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        /// <summary>
        /// The first <see cref="Point"/>.
        /// </summary>
        public Point Point1;

        /// <summary>
        /// The second <see cref="Point"/>.
        /// </summary>
        public Point Point2;

        /// <summary>
        /// Gets a collection of the two <see cref="Point"/> instances that constitute the line.
        /// </summary>
        public Point[] Points => new[] { Point1, Point2 };

        /// <summary>
        /// Gets the slope of the line.
        /// </summary>
        public double Slope =>
            (Point2.X - Point1.X) == 0 ? 0 :
            (Point2.Y - Point1.Y) / (Point2.X - Point1.X);

        /// <summary>
        /// Gets the length of the line.
        /// </summary>
        public double Length
        {
            get
            {
                if (Point1.X == Point2.X) { return FindLength(Point2.Y, Point1.Y); }
                if (Point1.Y == Point2.Y) { return FindLength(Point2.X, Point1.X); }

                Point point3 = new(Point1.X, Point2.Y);

                double lengthA = FindLength(point3.X, Point2.X);
                double lengthB = FindLength(point3.Y, Point1.Y);

                return Math.Sqrt(Math.Pow((double)lengthA, 2) + Math.Pow((double)lengthB, 2));
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{Point1} to {Point2}";

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Line line && Equals(line);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Line other)
        {
            return other.Points.Contains(Point1)
                && other.Points.Contains(Point2);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Point1, Point2);
        }

        /// <summary>
        /// Determines the equality of two <see cref="Line"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Line"/>.</param>
        /// <param name="right">The right <see cref="Line"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
        public static bool operator ==(Line left, Line right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines the inequality of two <see cref="Line"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Line"/>.</param>
        /// <param name="right">The right <see cref="Line"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
        public static bool operator !=(Line left, Line right)
        {
            return !(left == right);
        }

        private static double FindLength(double a, double b) =>
            ((a <= 0 && b <= 0) || (a >= 0 && b >= 0))
            ? Math.Abs(b - a)
            : Math.Abs(b) + Math.Abs(a);
    }
}

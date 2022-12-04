namespace Shibusa.Maths
{
    /// <summary>
    /// Represents a triangle.
    /// <seealso href="https://en.wikipedia.org/wiki/Law_of_cosines"/>
    /// </summary>
    /// <remarks>
    /// <see cref="AngleA"/> is the angle (in degrees) at <see cref="Point"/> A.
    /// <see cref="AngleB"/> is the angle (in degrees) at <see cref="Point"/> B.
    /// <see cref="AngleC"/> is the angle (in degrees) at <see cref="Point"/> C.
    /// <see cref="LineA"/> is the <see cref="Line"/> opposite <see cref="AngleA"/>.
    /// <see cref="LineB"/> is the <see cref="Line"/> opposite <see cref="AngleB"/>.
    /// <see cref="LineC"/> is the <see cref="Line"/> opposite <see cref="AngleC"/>.
    /// </remarks>
    public struct Triangle : IEquatable<Triangle>
    {
        /// <summary>
        /// Gets the <see cref="Point"/> A.
        /// </summary>
        public Point PointA;

        /// <summary>
        /// Gets the <see cref="Point"/> B.
        /// </summary>
        public Point PointB;

        /// <summary>
        /// Gets the <see cref="Point"/> C.
        /// </summary>
        public Point PointC;

        /// <summary>
        /// Creates a new instance of the <see cref="Triangle"/> struct.
        /// </summary>
        /// <param name="pointA"><see cref="Point"/> A.</param>
        /// <param name="pointB"><see cref="Point"/> B.</param>
        /// <param name="pointC"><see cref="Point"/> C.</param>
        public Triangle(Point pointA, Point pointB, Point pointC)
        {
            PointA = pointA;
            PointB = pointB;
            PointC = pointC;
        }

        /// <summary>
        /// Gets the line between points B and C (the line opposite angle A).
        /// </summary>
        public Line LineA => new(PointB, PointC);

        /// <summary>
        /// Gets the line between points A and C (the line opposite angle B).
        /// </summary>
        public Line LineB => new(PointA, PointC);

        /// <summary>
        /// Gets the line between points A and B (the line opposite angle C).
        /// </summary>
        public Line LineC => new(PointA, PointB);

        /// <summary>
        /// Gets the angle (in degrees) of angle A.
        /// </summary>
        public double AngleA => FindAngle(LineB.Length, LineC.Length, LineA.Length);

        /// <summary>
        /// Gets the angle (in degrees) of angle B.
        /// </summary>
        public double AngleB => FindAngle(LineC.Length, LineA.Length, LineB.Length);

        /// <summary>
        /// Gets the angle (in degrees) of angle C.
        /// </summary>
        public double AngleC => FindAngle(LineA.Length, LineB.Length, LineC.Length);

        /// <summary>
        /// Gets the three points of the triangle - points A, B, and C in that order.
        /// </summary>
        public IEnumerable<Point> Points => new[] { PointA, PointB, PointC };

        /// <summary>
        /// Gets the three lines of the triangle - lines A, B, and C in that order.
        /// </summary>
        public IEnumerable<Line> Lines => new[] { LineA, LineB, LineC };

        /// <summary>
        /// Gets the angles (in degrees) of the triangle - angles A, B, and C in that order.
        /// </summary>
        public IEnumerable<double> Angles => new[] { AngleA, AngleB, AngleC };

        /// <summary>
        /// Gets the <see cref="Line"/> that is the hypotenuse of the triangle.
        /// </summary>
#if NETSTANDARD2_0
        public Line Hypotenuse => Lines.OrderByDescending(l => l.Length).FirstOrDefault();
#else
        public Line Hypotenuse => Lines.MaxBy(l => l.Length);
#endif

        /// <summary>
        /// Gets an indicator of whether the triangle is a right triangle.
        /// </summary>
        public bool IsRight => Angles.Contains(90D);

        /// <summary>
        /// Gets an indicator of whether the triangle is a scalene triangle.
        /// </summary>
        public bool IsScalene => LineA.Length != LineB.Length &&
            LineB.Length != LineC.Length && 
            LineA.Length != LineC.Length;

        /// <summary>
        /// Gets an indicator of whether the triangle is an isosceles triangle.
        /// </summary>
        public bool IsIsosceles => (LineA.Length == LineB.Length && LineB.Length != LineC.Length)
            || (LineA.Length == LineC.Length && LineC.Length != LineB.Length)
            || (LineB.Length == LineC.Length && LineC.Length != LineB.Length);

        /// <summary>
        /// Gets an indicator of whether the triangle is an equilateral triangle.
        /// </summary>
        public bool IsEquilateral => LineA.Length == LineB.Length && LineB.Length == LineC.Length;

        /// <summary>
        /// Gets an indicator of whether the triangle is an acute triangle.
        /// </summary>
        public bool IsAcute => AngleA < 90D && AngleB < 90D && AngleC < 90D;

        /// <summary>
        /// Gets an indicator of whether the triangle is an obtuse triangle.
        /// </summary>
        public bool IsObtuse => AngleA > 90D || AngleB > 90D || AngleC > 90D;

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Triangle triangle && Equals(triangle);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Triangle other)
        {
            return other.Points.Contains(PointA)
                && other.Points.Contains(PointB)
                && other.Points.Contains(PointC);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            int hashCode = -180594284;
            hashCode = hashCode * -1521134295 + PointA.GetHashCode();
            hashCode = hashCode * -1521134295 + PointB.GetHashCode();
            hashCode = hashCode * -1521134295 + PointC.GetHashCode();
            return hashCode;        
#else
            return HashCode.Combine(PointA, PointB, PointC);
#endif
        }

        /// <summary>
        /// Determines the equality of two <see cref="Triangle"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Triangle"/>.</param>
        /// <param name="right">The right <see cref="Triangle"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
        public static bool operator ==(Triangle left, Triangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines the inequality of two <see cref="Triangle"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="Triangle"/>.</param>
        /// <param name="right">The right <see cref="Triangle"/>.</param>
        /// <returns>An indicator of  equality; true if <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
        public static bool operator !=(Triangle left, Triangle right)
        {
            return !(left == right);
        }

        private static double FindAngle(double a, double b, double c)
        {
            var denominator = 2D * a * b;
            var numerator = Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2);
            double degrees = Math.Acos(numerator / denominator) * 180 / Math.PI;
            return Math.Round(degrees, 2);
        }
    }

}

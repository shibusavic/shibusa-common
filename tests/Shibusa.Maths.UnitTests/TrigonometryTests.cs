using Xunit;

namespace Shibusa.Maths.UnitTests
{
    public class TrigonometryTests
    {
        [Fact]
        public void Point_Equality()
        {
            var p1 = new Point() { X = 1D, Y = 2D };
            var p2 = new Point(1M, 2M);
            var p3 = new Point(2M, 3M);

            Assert.Equal(p1, p2);
            Assert.NotEqual(p1, p3);
            Assert.True(p1 == p2);
        }

        [Fact]
        public void Line_Equality()
        {
            var p1 = new Point(1M, 2M);
            var p2 = new Point(15M, 100M);
            var p3 = new Point(5M, 13M);

            var line1 = new Line() { Point1 = p1, Point2 = p2 };
            var line2 = new Line(p1, p2);
            var line3 = new Line(p1, p3);
            var line4 = new Line(p2, p1);

            Assert.Equal(line1, line2);
            Assert.Equal(line2, line4);
            Assert.NotEqual(line1, line3);
            Assert.True(line1 == line2);
        }

        [Fact]
        public void Line_Length_Positive()
        {
            var p1 = new Point(1M, 1M);
            var p2 = new Point(5M, 1M);
            var line = new Line(p1, p2);
            Assert.Equal(4D, line.Length);
        }

        [Fact]
        public void Line_Length_Negative()
        {
            var p1 = new Point(-1M, 1M);
            var p2 = new Point(-5M, 1M);
            var line = new Line(p1, p2);
            Assert.Equal(4D, line.Length);
        }

        [Fact]
        public void Line_Length_PositiveAndNegative()
        {
            var p1 = new Point(1M, 1M);
            var p2 = new Point(-5M, 1M);
            var line = new Line(p1, p2);
            Assert.Equal(6D, line.Length);
        }

        [Fact]
        public void Triangle_Equality()
        {
            var p1 = new Point(1M, 2M);
            var p2 = new Point(15M, 100M);
            var p3 = new Point(5M, 13M);
            var p4 = new Point(100M, 14M);

            var triangle1 = new Triangle(p1, p2, p3);
            var triangle2 = new Triangle() { PointA = p1, PointB = p2, PointC = p3 };
            var triangle3 = new Triangle(p1, p2, p4);
            var triangle4 = new Triangle(p3, p2, p1);

            Assert.Equal(triangle1, triangle2);
            Assert.Equal(triangle1, triangle4);
            Assert.True(triangle1 == triangle2);
            Assert.NotEqual(triangle1, triangle3);
        }

        [Fact]
        public void Triangle_FindRightTriangle()
        {
            var ptA = new Point(1D, 1D);
            var ptB = new Point(5D, 1D);
            var ptC = new Point(5D, 5D);

            var triangle = new Triangle(ptA, ptB, ptC);

            Assert.Equal(90D, triangle.AngleB);
            Assert.True(triangle.IsRight);
        }

        [Fact]
        public void Triangle_Defaults()
        {
            var triangle = new Triangle(default, default, default);

            Assert.True(triangle == default);
        }
    }
}

using System;
using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class TransformDateTimeTests
    {
        [Fact]
        public void StartOfDay()
        {
            DateTime now = DateTime.Now;

            Assert.Equal(DateTimeKind.Local, now.Kind);
            var sod = TransformDateTime.StartOfDay(now);
            Assert.NotEqual(now, sod);
            Assert.Equal(now.Kind, sod.Kind);
            Assert.Equal(0, sod.Hour);
            Assert.Equal(0, sod.Minute);
            Assert.Equal(0, sod.Second);
            Assert.Equal(0, sod.Millisecond);

            now = DateTime.UtcNow;
            Assert.Equal(DateTimeKind.Utc, now.Kind);
            sod = TransformDateTime.StartOfDay(now);
            Assert.NotEqual(now, sod);
            Assert.Equal(now.Kind, sod.Kind);
            Assert.Equal(0, sod.Hour);
            Assert.Equal(0, sod.Minute);
            Assert.Equal(0, sod.Second);
            Assert.Equal(0, sod.Millisecond);
        }

        [Fact]
        public void EndOfDay()
        {
            var now = DateTime.Now;
            Assert.Equal(DateTimeKind.Local, now.Kind);
            var eod = TransformDateTime.EndOfDay(now);
            Assert.NotEqual(now, eod);
            Assert.Equal(now.Kind, eod.Kind);
            Assert.Equal(23, eod.Hour);
            Assert.Equal(59, eod.Minute);
            Assert.Equal(59, eod.Second);
            Assert.Equal(999, eod.Millisecond);

            now = DateTime.UtcNow;
            Assert.Equal(DateTimeKind.Utc, now.Kind);
            eod = TransformDateTime.EndOfDay(now);
            Assert.NotEqual(now, eod);
            Assert.Equal(now.Kind, eod.Kind);
            Assert.Equal(23, eod.Hour);
            Assert.Equal(59, eod.Minute);
            Assert.Equal(59, eod.Second);
            Assert.Equal(999, eod.Millisecond);
        }
    }
}

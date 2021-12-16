using System.ComponentModel;
using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class EnumExtensionTests
    {
        private enum NoDescription
        {
            None = 0,
            Some,
            All
        }

        private enum WithDescription
        {
            [Description("Nothing")]
            None = 0,
            [Description("Some of it")]
            Some,
            [Description("All of it")]
            All
        }

        [Fact]
        public void GetDescription_WithDescription_GetsDescriptionValue()
        {
            Assert.Equal("Nothing", WithDescription.None.GetDescription());
            Assert.Equal("Some of it", WithDescription.Some.GetDescription());
            Assert.Equal("All of it", WithDescription.All.GetDescription());
        }

        [Fact]
        public void GetDescription_NoDescription_GetStringValue()
        {
            Assert.Equal("None", NoDescription.None.GetDescription());
            Assert.Equal("Some", NoDescription.Some.GetDescription());
            Assert.Equal("All", NoDescription.All.GetDescription());
        }

        [Fact]
        public void GetDescriptions_WithDescription_GetsDescriptionValues()
        {
            List<string> descriptions = new() { "Nothing", "Some of it", "All of it" };
            Assert.True(descriptions.SequenceEqual(EnumExtensions.GetDescriptions<WithDescription>()));
        }

        [Fact]
        public void GetDescriptions_NoDescription_GetsStringValues()
        {
            List<string> descriptions = new() { "None", "Some", "All" };
            Assert.True(descriptions.SequenceEqual(EnumExtensions.GetDescriptions<NoDescription>()));
        }
    }
}
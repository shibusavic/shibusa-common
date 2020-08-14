using System.Linq;
using Xunit;

namespace Shibusa.Validators.UnitTests
{
    public class SocialSecurityNumberTests
    {
        public SocialSecurityNumberTests()
        {
        }

        [Theory]
        [InlineData("123-12-1233")]
        [InlineData("123121233")]
        public void Ssn_ValidStructure(string ssn)
        {
            Assert.True(SocialSecurityNumber.IsValidStructure(ssn));
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("11")]
        [InlineData("111")]
        [InlineData("111-1")]
        [InlineData("111-11")]
        [InlineData("111-11-1")]
        [InlineData("111-11-11")]
        [InlineData("111-11-111")]
        [InlineData("111-11-11111")]
        [InlineData("111--1")]
        [InlineData("111--11")]
        [InlineData("111--11-1")]
        [InlineData("111--11-11")]
        [InlineData("111--11-111")]
        [InlineData("111--11-11111")]
        [InlineData("111-1-")]
        [InlineData("111-11--")]
        [InlineData("111-11--1")]
        [InlineData("111-11--11")]
        [InlineData("111-11--111")]
        [InlineData("111-11--11111")]
        [InlineData("111-11--1111-")]
        [InlineData("-111-11--1111-")]
        public void Ssn_InvalidStructure(string ssn)
        {
            Assert.False(SocialSecurityNumber.IsValidStructure(ssn));
        }

        [Theory]
        [InlineData("001-01-0001")]
        [InlineData("236-01-0001")]
        [InlineData("247-01-0001")]
        [InlineData("586-01-0001")]
        [InlineData("700-01-0001")]
        [InlineData("749-01-0001")]
        public void Ssn_Valid(string ssn)
        {
            Assert.True(SocialSecurityNumber.IsValid(ssn));
        }

        [Theory]
        [InlineData("000-00-0000")]
        [InlineData("000-11-1111")]
        [InlineData("111-00-1111")]
        [InlineData("111-11-0000")]
        [InlineData("237-11-1111")]
        [InlineData("246-11-1111")]
        [InlineData("587-11-1111")]
        [InlineData("679-11-1111")]
        [InlineData("681-11-1111")]
        [InlineData("690-11-1111")]
        [InlineData("699-11-1111")]
        [InlineData("750-11-1111")]
        [InlineData("999-11-1111")]
        public void Ssn_Invalid(string ssn)
        {
            Assert.False(SocialSecurityNumber.IsValid(ssn));
        }

        [Fact]
        public void Used_UnusedAreas()
        {
            var unusedAreas = SocialSecurityNumber.UnusedAreas;
            var usedAreas = SocialSecurityNumber.UsedAreas;

            Assert.Empty(usedAreas.Intersect(unusedAreas));

            var allAreas = unusedAreas.Union(usedAreas);

            foreach (int area in Enumerable.Range(1, 999)) 
            {
                Assert.Contains(area, allAreas);
            }
        }
    }
}

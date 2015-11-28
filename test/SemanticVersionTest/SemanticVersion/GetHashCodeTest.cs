using System;
namespace SemanticVersionTest
{
    using SemanticVersion;

    using Xunit;

    public class GetHashCodeTests
    {
        [Fact]
        public void GetHashCodeEqual()
        {
            Version left = new Version(1, 0, 0, "foo", "bar");
            Version right = new Version(1, 0, 0, "foo", "bar");
            
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }

        [Fact]
        public void GetHashCodeNotEqual()
        {
            Version left = new Version(1, 0, 0, "foo", "bar");
            Version right = new Version(2, 0, 0, "foo", "bar");

            Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
        }

        [Fact]
        public void GetHashCodeNotEqualNoBuild()
        {
            Version left = new Version(1, 0, 0, "foo");
            Version right = new Version(2, 0, 0, "foo");

            Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
        }

        [Fact]
        public void GetHashCodeNotEqualNoBuildNoPrerelease()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(2, 0, 0);

            Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
        }
    }
}

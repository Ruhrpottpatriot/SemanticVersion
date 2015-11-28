using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticVersionTest
{
    using SemanticVersion;

    using Xunit;

    public class EqualityTests
    {
        [Fact]
        public void Equals()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 0);

            Assert.True(left.Equals(right));
        }

        [Fact]
        public void EqualsObject()
        {
            Version left = new Version(1, 0, 0);
            object right = new Version(1, 0, 0);

            Assert.True(left.Equals(right));
        }

        [Fact]
        public void EqualsNull()
        {
            Version left = new Version(1, 0, 0);

            Assert.False(left.Equals((object)null));
        }

        [Fact]

        public void NotEqualsNull()
        {
            Version left = new Version(1, 0, 0);

            Assert.False(left.Equals(null));
        }

        [Fact]

        public void NotEqualsMajor()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(2, 0, 0);

            Assert.False(left.Equals(right));

        }

        [Fact]

        public void NotEqualsMinor()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 1, 0);

            Assert.False(left.Equals(right));
        }

        [Fact]

        public void NotEqualsPatch()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 1);

            Assert.False(left.Equals(right));
        }

        [Fact]
        public void NotEqualsPrerelease()
        {
            Version left = new Version(1, 0, 0, "foo");
            Version right = new Version(1, 0, 0, "bar");

            Assert.False(left.Equals(right));
        }

        [Fact]

        public void NotEqualsBuild()
        {
            Version left = new Version(1, 0, 0, "foo", "bar");
            Version right = new Version(1, 0, 0, "foo", "baz");

            Assert.False(left.Equals(right));
        }
    }
}

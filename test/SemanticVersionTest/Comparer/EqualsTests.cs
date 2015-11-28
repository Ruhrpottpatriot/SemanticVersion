namespace SemanticVersionTest.Comparer
{
    using System;

    using SemanticVersion;

    using Xunit;

    using Version = SemanticVersion.Version;

    public class EqualsTests
    {
        [Fact]
        public void ReferenceEquals()
        {
            Version version = new Version(1, 0, 0);
            
            SemanticVersionComparer comparer = new SemanticVersionComparer();

            Assert.True(comparer.Equals(version, version));
        }

        [Fact]
        public void ReferenceEqualsLeftNull()
        {
            Version version = new Version(1, 0, 0);

            SemanticVersionComparer comparer = new SemanticVersionComparer();

            Assert.False(comparer.Equals(null, version));
        }

        [Fact]
        public void ReferenceEqualsRightNull()
        {
            Version version = new Version(1, 0, 0);

            SemanticVersionComparer comparer = new SemanticVersionComparer();

            Assert.False(comparer.Equals(version, null));
        }
    }
}

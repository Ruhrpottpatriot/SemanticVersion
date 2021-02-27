namespace SemanticVersionTest
{
    using SemVersion;

    using Xunit;

    public class CompareToTests
    {
        [Fact]
        public void CompareToInvaildObject()
        {
            SemanticVersion version = new SemanticVersion(1, 0, 0);

            Assert.Equal(1, version.CompareTo(new object()));
        }

        [Fact]
        public void CompareToValidObject()
        {
            SemanticVersion version = new SemanticVersion(1, 0, 0);
            SemanticVersion other = new SemanticVersion(1, 0, 0);
            Assert.Equal(0, version.CompareTo((object)other));
        }

        [Fact]
        public void CompareTo()
        {
            SemanticVersion version = new SemanticVersion(1, 0, 0);

            Assert.Equal(0, version.CompareTo(version));
        }

        [Fact]
        public void CompareToNull()
        {
            SemanticVersion version = new SemanticVersion(1, 0, 0);

            Assert.Equal(1, version.CompareTo(null));
        }

        [Fact]
        public void CompareToMinor()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0);
            SemanticVersion right = new SemanticVersion(1, 1, 0);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToPatch()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0);
            SemanticVersion right = new SemanticVersion(1, 1, 1);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToBuildRightEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0, build: "abc");
            SemanticVersion right = new SemanticVersion(1, 1, 0);

            Assert.Equal(0, left.CompareTo(right));
        }

        [Fact]
        public void CompareToBuildLeftEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0);
            SemanticVersion right = new SemanticVersion(1, 1, 0, build: "abc");

            Assert.Equal(0, left.CompareTo(right));
        }

        [Fact]
        public void CompareToPatchIsWildcard()
        {
            SemanticVersion left = new SemanticVersion(1, 1, null);
            SemanticVersion right = new SemanticVersion(1, 1, 1);

            Assert.Equal(0, left.CompareTo(right));
        }

        [Fact]
        public void CompareToMinorAndPatchIsWildcard()
        {
            SemanticVersion left = new SemanticVersion(1, 0, null);
            SemanticVersion right = new SemanticVersion(1, 1, 0);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToPrereleaseIsWildcardAgainstEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0, prerelease: "*");
            SemanticVersion right = new SemanticVersion(1, 1, 0);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToPrereleaseIsWildcard()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0, prerelease: "*");
            SemanticVersion right = new SemanticVersion(1, 1, 0, prerelease: "alpha");

            Assert.Equal(0, left.CompareTo(right));
        }
    }
}

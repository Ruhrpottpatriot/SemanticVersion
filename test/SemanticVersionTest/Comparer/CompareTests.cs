namespace SemanticVersionTest.Comparer
{
    using SemanticVersion;

    using Xunit;

    public class CompareTests
    {
        [Fact]
        public void Compare()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 0);

            VersionComparer comparer = new VersionComparer();

            Assert.Equal(0,comparer.Compare(left, right));
        }

        [Fact]
        public void CompareLeftNull()
        {
            Version right = new Version(1, 0, 0);

            VersionComparer comparer = new VersionComparer();

            Assert.Equal(-1, comparer.Compare(null, right));
        }

        [Fact]
        public void CompareRightNull()
        {
            Version left = new Version(1, 0, 0);

            VersionComparer comparer = new VersionComparer();

            Assert.Equal(1, comparer.Compare(left, null));
        }

        [Fact]
        public void CompareBothNull()
        {
            VersionComparer comparer = new VersionComparer();

            Assert.Equal(0, comparer.Compare(null, null));
        }
    }
}
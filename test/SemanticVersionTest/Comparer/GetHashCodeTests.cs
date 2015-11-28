namespace SemanticVersionTest.Comparer
{
    using SemanticVersion;

    using Xunit;

    public class GetHashCodeTests
    {
        [Fact]
        public void GetHashCodeSame()
        {
            Version left = new Version(1, 0, 0, "foo", "bar");
            Version right = new Version(1, 0, 0, "foo", "bar");

            VersionComparer comparer = new VersionComparer();

            Assert.Equal(comparer.GetHashCode(left), comparer.GetHashCode(right));
        }

        [Fact]
        public void GetHashCodeDifferent()
        {
            Version left = new Version(1, 0, 0, "foo", "bar");
            Version right = new Version(1, 0, 0, "foo", "baz");

            VersionComparer comparer = new VersionComparer();

            Assert.NotEqual(comparer.GetHashCode(left), comparer.GetHashCode(right));
        }
    }
}
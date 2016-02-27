namespace SemanticVersionTest
{
    using SemVersion;

    using Xunit;

    public class TryParseTests
    {
        [Fact]
        public void TryParseReturnsVersion()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.1.1", out version);

            Assert.True(result);
            Assert.Equal(new SemanticVersion(1, 1, 1), version);
        }

        [Fact]
        public void TryParseNullReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse(null, out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseEmptyStringReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse(string.Empty, out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseInvalidStringReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("invalid-version", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseNonStandardReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1", out version);

            Assert.False(result);
            Assert.Null(version);
        }
    }
}
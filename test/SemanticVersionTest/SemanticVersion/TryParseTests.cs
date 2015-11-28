namespace SemanticVersionTest
{
    using Semver;

    using Xunit;

    public class TryParseTests
    {
        [Fact]
        public void Parse()
        {
            SemanticVersion version;
            Assert.True(SemanticVersion.TryParse("1.1.1", out version));
        }

        [Fact]
        public void ParseFail()
        {
            SemanticVersion version;
            Assert.False(SemanticVersion.TryParse("1", out version));
        }
    }
}
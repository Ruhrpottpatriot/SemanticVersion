namespace SemanticVersionTest
{
    using SemanticVersion;

    using Xunit;

    public class TryParseTests
    {
        [Fact]
        public void Parse()
        {
            Version version;
            Assert.True(Version.TryParse("1.1.1", out version));
        }

        [Fact]
        public void ParseFail()
        {
            Version version;
            Assert.False(Version.TryParse("1", out version));
        }
    }
}
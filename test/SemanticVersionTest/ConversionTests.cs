namespace SemanticVersionTest
{
    using System;

    using Semver;

    using Xunit;

    public class ConversionTests
    {
        [Fact]
        public void Conversion()
        {
            Version dotNetVersion = new Version(1, 1, 1, 1);

            SemanticVersion semanticVersion = (SemanticVersion)dotNetVersion;

            Assert.Equal(1, semanticVersion.Major);
            Assert.Equal(1, semanticVersion.Minor);
            Assert.Equal(1, semanticVersion.Patch);
            Assert.Equal(string.Empty, semanticVersion.Prerelease);
            Assert.Equal("1", semanticVersion.Build);
        }

        [Fact]
        public void ConversionNoBuildNoRevision()
        {
            Version dotNetVersion = new Version(1, 1);

            SemanticVersion semanticVersion = (SemanticVersion)dotNetVersion;

            Assert.Equal(1, semanticVersion.Major);
            Assert.Equal(1, semanticVersion.Minor);
            Assert.Equal(0, semanticVersion.Patch);
            Assert.Equal(string.Empty, semanticVersion.Prerelease);
            Assert.Equal(string.Empty, semanticVersion.Build);
        }

        [Fact]
        public void ConversionNull()
        {
            Version dotNetVersion = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                SemanticVersion version = (SemanticVersion)dotNetVersion;
            });
        }
    }
}

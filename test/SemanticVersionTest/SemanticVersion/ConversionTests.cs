namespace SemanticVersionTest
{
    using System;

    using Xunit;

    using Version = SemanticVersion.Version;

    public class ConversionTests
    {
        [Fact]
        public void Conversion()
        {
            System.Version dotNetVersion = new System.Version(1, 1, 1, 1);

            Version version = (Version)dotNetVersion;

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal(string.Empty, version.Prerelease);
            Assert.Equal("1", version.Build);
        }

        [Fact]
        public void ConversionNoBuildNoRevision()
        {
            System.Version dotNetVersion = new System.Version(1, 1);

            Version version = (Version)dotNetVersion;

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(0, version.Patch);
            Assert.Equal(string.Empty, version.Prerelease);
            Assert.Equal(string.Empty, version.Build);
        }

        [Fact]
        public void ConversionNull()
        {
            System.Version dotNetVersion = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                Version version = (Version)dotNetVersion;
            });
        }
    }
}

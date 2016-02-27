namespace SemanticVersionTest
{
    using System;
    using SemVersion;
    using Xunit;

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ParseTests
    {
        [Fact]
        public void Parse()
        {
            SemanticVersion version = SemanticVersion.Parse("1.1.1");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
        }

        [Fact]
        public void ParsePrerelease()
        {
            SemanticVersion version = SemanticVersion.Parse("1.1.1-alpha-12");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal("alpha-12", version.Prerelease);
        }

        [Fact]
        public void ParseBuild()
        {
            SemanticVersion version = SemanticVersion.Parse("1.1.1+nightly.23.43-foo");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal("nightly.23.43-foo", version.Build);
        }

        [Fact]
        public void ParseComplete()
        {
            SemanticVersion version = SemanticVersion.Parse("1.1.1-alpha-12+nightly.23.43-foo");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal("alpha-12", version.Prerelease);
            Assert.Equal("nightly.23.43-foo", version.Build);
        }

        [Fact]
        public void ParseMajorWildcard()
        {
            var version = SemanticVersion.Parse("1.*");
        }

        [Fact]
        public void ParseNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SemanticVersion.Parse(null));
        }

        [Fact]
        public void ParseEmptyStringThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(string.Empty));
        }

        [Fact]
        public void ParseInvalidStringThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("invalid-version"));
        }

        [Fact]
        public void ParseFail()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("foo-1.232+1"));
        }

        [Fact]
        public void ParseInvalidOperationNoMinorNoPatch()
        {
            Assert.Throws<InvalidOperationException>(() => SemanticVersion.Parse("1"));
        }

        [Fact]
        public void ParseInvalidOperationNoPatch()
        {
            Assert.Throws<InvalidOperationException>(() => SemanticVersion.Parse("1.3"));
        }

        [Fact]
        public void ParseInvalidOperationNoPatchWithPrerelease()
        {
            Assert.Throws<InvalidOperationException>(() => SemanticVersion.Parse("1.3-alpha"));
        }

        [Fact]
        public void ImplicitConversion()
        {
            SemanticVersion version = "1.1.1";

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
        }

        [Fact]
        public void ImplicitConversionFail()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                SemanticVersion version = "1";
            });
        }
    }
}

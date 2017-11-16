namespace SemanticVersionTest
{
    using System;
    using SemVersion;
    using Xunit;

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
        public void ParseValidZeroes()
        {
            SemanticVersion version = SemanticVersion.Parse("0.1.1");
            Assert.Equal(0, version.Major);

            version = SemanticVersion.Parse("1.0.1");
            Assert.Equal(0, version.Minor);

            version = SemanticVersion.Parse("1.1.0");
            Assert.Equal(0, version.Patch);
        }

        [Fact]
        public void ParseMaxInts()
        {
            SemanticVersion version = SemanticVersion.Parse(string.Format("{0}.{0}.{0}", int.MaxValue));

            Assert.Equal(int.MaxValue, version.Major);
            Assert.Equal(int.MaxValue, version.Minor);
            Assert.Equal(int.MaxValue, version.Patch);
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
        public void ParseWhiteSpaceThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(" "));
        }

        [Fact]
        public void ParseLeadingZeroesThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("01.1.1"));
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.01.1"));
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.1.01"));
        }

        [Fact]
        public void ParseNegativeNumbersThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("-1.1.1"));
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.-1.1"));
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.1.-1"));
        }

        [Fact]
        public void ParseInvalidStringThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("invalid-version"));
        }

        [Fact]
        public void ParseMissingMinorThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1"));
        }

        [Fact]
        public void ParseMissingPatchThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.2"));
        }

        [Fact]
        public void ParseMissingPatchWithPrereleaseThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.2-alpha"));
        }

        [Fact(Skip="Non-conformant: Parse allows leading zeroes.")]
        public void ParseLeadingZeroThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.01.1"));
        }

        [Fact]
        public void ParseMissingLeadingHyphenForPrereleaseThrows()
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.2.3.alpha"));
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
            Assert.Throws<ArgumentException>(() =>
            {
                SemanticVersion version = "1";
            });
        }
    }
}

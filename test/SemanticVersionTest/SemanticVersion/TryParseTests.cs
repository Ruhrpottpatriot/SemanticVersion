using System.Collections.Generic;

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
            var result = SemanticVersion.TryParse("1.2.3", out version);

            Assert.True(result);
            Assert.Equal(new SemanticVersion(1, 2, 3), version);
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
        public void TryParseMissingMinorReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseMissingPatchReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseMissingPatchValueReturnsFalse()
        {
            SemanticVersion version;
            // Trailing separator but no value
            var result = SemanticVersion.TryParse("1.2.", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseMissingPatchValueWithPrereleaseReturnsFalse()
        {
            SemanticVersion version;
            
            var result = SemanticVersion.TryParse("1.2-alpha", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseMissingPatchWithPrereleaseReturnsFalse()
        {
            SemanticVersion version;
            // Trailing separator but no value
            var result = SemanticVersion.TryParse("1.2-alpha.", out version);
            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseMajorWildcard()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("*", out version);

            Assert.True(result);
            Assert.Null(version.Major);
            Assert.Null(version.Minor);
            Assert.Null(version.Patch);
        }

        [Fact]
        public void TryParseMajorWildcardWithTrailingSeparator()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("*. ", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParseMinorWildcard()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.*", out version);

            Assert.True(result);
            Assert.Equal(1, version.Major);
            Assert.Null(version.Minor);
            Assert.Null(version.Patch);
        }


        [Fact]
        public void TryParseMinorWildcardWithTrailingSeparator()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.*.  ", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact]
        public void TryParsePatchWildcard()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.*", out version);

            Assert.True(result);
            Assert.Equal(1, version.Major);
            Assert.Equal(2, version.Minor);
            Assert.Null(version.Patch);
        }

        [Fact]
        public void TryParsePatchWildcardWithTrailingSeparator()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.*.  ", out version);

            Assert.False(result);
            Assert.Null(version);
        }


        [Fact]
        public void TryParseInvalidMajorMinorPatchValues()
        {
            SemanticVersion version;
            var invalidAtoms = new string[] {"-01", "-1", "00", "01", "0 2"};
            var validAtoms = new string[] {"0", "1", "10", "1234"};

            var list = new List<string>();
            list.AddRange(invalidAtoms);
            list.AddRange(validAtoms);


            var testValues = list.ToArray();
            bool result = false;
            string verStr;

            foreach (var major in invalidAtoms) {
                foreach (var minor in validAtoms) {
                    foreach (var patch in validAtoms) {

                        verStr = string.Format("{0}.{1}.{2}", major, minor, patch);
                        result = SemanticVersion.TryParse(verStr, out version);
                        Assert.False(result, verStr);
                        Assert.Null(version);

                        foreach (var prerelease in validAtoms) {

                            verStr = string.Format("{0}.{1}.{2}-{3}", major, minor, patch, prerelease);
                            result = SemanticVersion.TryParse(verStr, out version);
                            Assert.False(result, verStr);
                            Assert.Null(version);

                            foreach (var build in validAtoms) {

                                verStr = string.Format("{0}.{1}.{2}-{3}+{4}", major, minor, patch, prerelease, build);
                                result = SemanticVersion.TryParse(verStr, out version);
                                Assert.False(result);
                                Assert.Null(version);
                            }

                        }
                    }
                }
            }
        }


        [Fact] //(Skip = "Needs check with specification and regex refactoring")]
        public void TryParseWildcardInMajor()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("*.2", out version);

            Assert.True(result);
            Assert.Equal("*", version.ToString());
        }

        [Fact]
        public void TryParseWildcardInMinor()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.*.3", out version);

            Assert.True(result);
            Assert.Equal("1.*", version.ToString());
        }

        [Fact]
        public void TryParseWildcardInPatch()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.*", out version);

            Assert.True(result);
            Assert.Equal("1.2.*", version.ToString());
        }

        [Fact]
        public void TryParseWildcardInPrerelease()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.3-*", out version);

            Assert.True(result);
            Assert.Equal("1.2.3-*", version.ToString());
        }

        [Fact]
        public void TryParseWildcardInPrereleaseWithBuild()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.3-*+bld", out version);

            Assert.True(result);
            Assert.Equal("1.2.3-*+bld", version.ToString());
        }

        [Fact]
        public void TryParseWildcardInPrereleaseAndBuild()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.3-*+*", out version);

            Assert.True(result);
            Assert.Equal("1.2.3-*", version.ToString());
        }

        [Fact]
        public void TryParseWildcardInBuild()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.3+*", out version);

            Assert.True(result);

            // The test restult below is intended, as build shouldn't factor 
            // into version precedende (and therefore equality).
            Assert.Equal("1.2.3", version.ToString());
        }

        [Fact] //(Skip="Broken")]
        public void TryParseWildcardInBuildWithPrerelease()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.3-preR+*", out version);

            Assert.True(result);

            // The test restult below is intended, as build shouldn't factor 
            // into version precedende (and therefore equality).
            Assert.Equal("1.2.3-preR", version.ToString());
        }

    }
}
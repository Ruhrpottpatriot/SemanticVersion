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
            Assert.Equal(null, version.Major);
            Assert.Equal(null, version.Minor);
            Assert.Equal(null, version.Patch);
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
            Assert.Equal(null, version.Minor);
            Assert.Equal(null, version.Patch);
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
            Assert.Equal(null, version.Patch);
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
        public void TryParseInvalidValues()
        {
            SemanticVersion version;
            var invalidAtoms = new string[] {"-01", "-1"};   // 00, 01 work, but violate "leading zero" restriction?
            var validAtoms = new string[] {"0", "1", "10", "1000"};

            var list = new List<string>();
            list.AddRange(invalidAtoms);
            list.AddRange(validAtoms);


            var testValues = list.ToArray();
            bool result = false;
            string verStr;

            foreach (var major in testValues) {
                foreach (var minor in testValues) {
                    foreach (var patch in testValues) {

                        verStr = string.Format("{0}.{1}.{2}", major, minor, patch);
                        result = SemanticVersion.TryParse(verStr, out version);
                        Assert.False(result, verStr);
                        Assert.Null(version);

                        foreach (var prerelease in testValues) {

                            verStr = string.Format("{0}.{1}.{2}-{3}", major, minor, patch, prerelease);
                            result = SemanticVersion.TryParse(verStr, out version);
                            Assert.False(result, verStr);
                            Assert.Null(version);

                            foreach (var build in testValues) {

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


        [Fact(Skip = "Needs check with specification and regex refactoring")]
        public void TryParseWildcardWithMinorReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("*.2", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact(Skip = "Needs check with specification and regex refactoring")]
        public void TryParseWildcardWithMinorAndPatchReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("*.2.3", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact(Skip = "Needs check with specification and regex refactoring")]
        public void TryParseWildcardInMiddleReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.*.3", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact(Skip = "Needs check with specification and regex refactoring")]
        public void TryParseMinorWildcardWithPrereleaseReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.*-alpha", out version);

            Assert.False(result);
            Assert.Null(version);
        }

        [Fact(Skip = "Needs check with specification and regex refactoring")]
        public void TryParsePatchWildcardWithPrereleaseReturnsFalse()
        {
            SemanticVersion version;
            var result = SemanticVersion.TryParse("1.2.*-alpha", out version);

            Assert.False(result);
            Assert.Null(version);
        }
    }
}
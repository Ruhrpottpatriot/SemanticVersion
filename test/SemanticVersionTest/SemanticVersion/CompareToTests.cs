namespace SemanticVersionTest
{
    using Semver;

    using Xunit;

    public class CompareToTests
    {
        [Fact]
        public void CompareToInvaildObject()
        {
            SemanticVersion version = new SemanticVersion(1,0,0);

            Assert.Equal(1,version.CompareTo(new object()));
        }

        [Fact]
        public void CompareToValidObject()
        {
            SemanticVersion version = new SemanticVersion(1,0,0);
            SemanticVersion other = new SemanticVersion(1,0,0);
            Assert.Equal(0, version.CompareTo((object)other));
        }
        
        [Fact]
        public void CompareTo()
        {
            SemanticVersion version = new SemanticVersion(1, 0, 0);

            Assert.Equal(0, version.CompareTo(version));
        }

        [Fact]
        public void CompareToNull()
        {
            SemanticVersion version = new SemanticVersion(1, 0, 0);

            Assert.Equal(1, version.CompareTo(null));
        }

        [Fact]
        public void CompareToMinor()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0);
            SemanticVersion right = new SemanticVersion(1, 1, 0);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToPatch()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0);
            SemanticVersion right = new SemanticVersion(1, 1, 1);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToBuildRightEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0, build:"abc");
            SemanticVersion right = new SemanticVersion(1, 1, 0);

            Assert.Equal(1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToBuildLeftEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 1, 0);
            SemanticVersion right = new SemanticVersion(1, 1, 0, build: "abc");

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseRightEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "alpha");
            SemanticVersion right = new SemanticVersion(1, 0, 0);

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseLeftEmpty()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0);
            SemanticVersion right = new SemanticVersion(1, 0, 0, "alpha");

            Assert.Equal(1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareTo()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "alpha");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "beta");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToWithNumbers()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "alpha1");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "beta2");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToWithBuild()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "alpha", "123");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "beta", "122");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseAsNumbersNotEqual()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "123.123");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "123.1233");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }
        [Fact]
        public void PrecedenceCompareToPrereleaseAsNumbersEqual()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "123.123");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "123.123");

            Assert.Equal(0, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseAsNumbersDifferentLength()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "123.123");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "123.123.123");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseLeftNum()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "alpha.2.2");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "alpha.a");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseRightNum()
        {
            SemanticVersion left = new SemanticVersion(1, 0, 0, "alpha.a");
            SemanticVersion right = new SemanticVersion(1, 0, 0, "alpha.2.2");

            Assert.Equal(1, left.PrecendenceCompareTo(right));
        }
    }
}

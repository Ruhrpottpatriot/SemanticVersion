namespace SemanticVersionTest
{
    using SemanticVersion;

    using Xunit;

    public class CompareToTests
    {
        [Fact]
        public void CompareToInvaildObject()
        {
            Version version = new Version(1,0,0);

            Assert.Equal(1,version.CompareTo(new object()));
        }

        [Fact]
        public void CompareToValidObject()
        {
            Version version = new Version(1,0,0);
            Version other = new Version(1,0,0);
            Assert.Equal(0, version.CompareTo((object)other));
        }
        
        [Fact]
        public void CompareTo()
        {
            Version version = new Version(1, 0, 0);

            Assert.Equal(0, version.CompareTo(version));
        }

        [Fact]
        public void CompareToNull()
        {
            Version version = new Version(1, 0, 0);

            Assert.Equal(1, version.CompareTo(null));
        }

        [Fact]
        public void CompareToMinor()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 1, 0);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToPatch()
        {
            Version left = new Version(1, 1, 0);
            Version right = new Version(1, 1, 1);

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToBuildRightEmpty()
        {
            Version left = new Version(1, 1, 0, build:"abc");
            Version right = new Version(1, 1, 0);

            Assert.Equal(1, left.CompareTo(right));
        }

        [Fact]
        public void CompareToBuildLeftEmpty()
        {
            Version left = new Version(1, 1, 0);
            Version right = new Version(1, 1, 0, build: "abc");

            Assert.Equal(-1, left.CompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseRightEmpty()
        {
            Version left = new Version(1, 0, 0, "alpha");
            Version right = new Version(1, 0, 0);

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseLeftEmpty()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 0, "alpha");

            Assert.Equal(1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareTo()
        {
            Version left = new Version(1, 0, 0, "alpha");
            Version right = new Version(1, 0, 0, "beta");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToWithNumbers()
        {
            Version left = new Version(1, 0, 0, "alpha1");
            Version right = new Version(1, 0, 0, "beta2");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToWithBuild()
        {
            Version left = new Version(1, 0, 0, "alpha", "123");
            Version right = new Version(1, 0, 0, "beta", "122");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseAsNumbersNotEqual()
        {
            Version left = new Version(1, 0, 0, "123.123");
            Version right = new Version(1, 0, 0, "123.1233");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }
        [Fact]
        public void PrecedenceCompareToPrereleaseAsNumbersEqual()
        {
            Version left = new Version(1, 0, 0, "123.123");
            Version right = new Version(1, 0, 0, "123.123");

            Assert.Equal(0, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseAsNumbersDifferentLength()
        {
            Version left = new Version(1, 0, 0, "123.123");
            Version right = new Version(1, 0, 0, "123.123.123");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseLeftNum()
        {
            Version left = new Version(1, 0, 0, "alpha.2.2");
            Version right = new Version(1, 0, 0, "alpha.a");

            Assert.Equal(-1, left.PrecendenceCompareTo(right));
        }

        [Fact]
        public void PrecedenceCompareToPrereleaseRightNum()
        {
            Version left = new Version(1, 0, 0, "alpha.a");
            Version right = new Version(1, 0, 0, "alpha.2.2");

            Assert.Equal(1, left.PrecendenceCompareTo(right));
        }
    }
}

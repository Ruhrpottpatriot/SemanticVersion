namespace SemanticVersionTest
{
    using SemanticVersion;

    using Xunit;

    public class OperatorTests
    {
        [Fact]
        public void Equals()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 0);

            Assert.True(left == right);
        }

        [Fact]
        public void NotEquals()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(2, 0, 0);

            Assert.True(left != right);

        }

        [Fact]
        public void Greater()
        {
            Version left = new Version(2, 0, 0);
            Version right = new Version(1, 0, 0);

            Assert.True(left > right);
        }

        [Fact]
        public void Less()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(2, 0, 0);

            Assert.True(left < right);
        }

        [Fact]
        public void GreaterEquals()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 0);


            Version left1 = new Version(2, 0, 0);
            Version right1 = new Version(1, 0, 0);

            Assert.True(left >= right);
            Assert.True(left1 >= right1);
        }

        [Fact]
        public void LessEquals()
        {
            Version left = new Version(1, 0, 0);
            Version right = new Version(1, 0, 0);


            Version left1 = new Version(1, 0, 0);
            Version right1 = new Version(2, 0, 0);

            Assert.True(left <= right);
            Assert.True(left1 <= right1);

        }
    }
}
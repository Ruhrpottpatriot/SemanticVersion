namespace SemanticVersionTest.Parser
{
    using System;
    using System.Diagnostics;
    using SemVersion.Parser;
    using Xunit;

    public class RangeParserTests
    {
        [Fact]
        public void ThrowArgumentException_NullString()
        {
            var parser = new RangeParser();
            Assert.Throws<ArgumentException>(() => parser.Parse(""));
        }

        [Fact]
        public void ThrowArgumentException_EmptyString()
        {
            var parser = new RangeParser();
            Assert.Throws<ArgumentException>(() => parser.Parse(null));
        }

        [Fact]
        public void Success_WildcardOnly()
        {   
            var parser = new RangeParser();
            Debug.WriteLine(parser.Parse("*"));       
        }
    }
}
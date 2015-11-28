using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticVersionTest
{
    using SemanticVersion;

    using Xunit;

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ParseTests
    {
        [Fact]
        public void Parse()
        {
            Version version = Version.Parse("1.1.1");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
        }

        [Fact]
        public void ParsePrerelease()
        {
            Version version = Version.Parse("1.1.1-alpha-12");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal("alpha-12", version.Prerelease);
        }

        [Fact]
        public void ParseBuild()
        {
            Version version = Version.Parse("1.1.1+nightly.23.43-foo");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal("nightly.23.43-foo", version.Build);
        }

        [Fact]
        public void ParseComplete()
        {
            Version version = Version.Parse("1.1.1-alpha-12+nightly.23.43-foo");

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
            Assert.Equal("alpha-12", version.Prerelease);
            Assert.Equal("nightly.23.43-foo", version.Build);
        }

        [Fact]
        public void ParseFail()
        {
            Assert.Throws<ArgumentException>(() => Version.Parse("foo-1.232+1"));
        }

        [Fact]
        public void ParseInvalidOperationNoMinorNoPatch()
        {
            Assert.Throws<InvalidOperationException>(() => Version.Parse("1"));
        }

        [Fact]
        public void ParseInvalidOperationNoPatch()
        {
            Assert.Throws<InvalidOperationException>(() => Version.Parse("1.3"));
        }

        [Fact]
        public void ParseInvalidOperationNoPatchWithPrerelease()
        {
            Assert.Throws<InvalidOperationException>(() => Version.Parse("1.3-alpha"));
        }

        [Fact]
        public void ImplicitConversion()
        {
            Version version = "1.1.1";

            Assert.Equal(1, version.Major);
            Assert.Equal(1, version.Minor);
            Assert.Equal(1, version.Patch);
        }

        [Fact]
        public void ImplicitConversionFail()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Version version = "1";
            });
        }
    }
}

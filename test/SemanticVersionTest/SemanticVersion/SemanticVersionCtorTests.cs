namespace SemanticVersionTest
{
    using SemVersion;
	using System;
	using System.Collections.Generic;
	using Xunit;

    public class SemanticVersionCtorTests
    {
		#region Test cases

		[Theory]
		[InlineData(1, 0, 0, null, null, "1.0.0")]
		[InlineData(1, 0, 0, "", "", "1.0.0")]
		[InlineData(20, 30, 40, null, null, "20.30.40")]
		[InlineData(0, 0, 0, null, null, "0.0.0")]
		[InlineData(1, 1, 0, "12", null, "1.1.0-12")]
		[InlineData(1, 1, 0, "alpha", null, "1.1.0-alpha")]
		[InlineData(1, 1, 0, "alpha.1", null, "1.1.0-alpha.1")]
		[InlineData(1, 1, 0, "alpha.1", "032", "1.1.0-alpha.1+032")] 

		#endregion
		public void ConstructValidStrictVersion(int? major, int? minor, int? patch, string prerelease, string build, string expectedString)
		{
            var version = new SemanticVersion(major, minor, patch, prerelease, build);

            Assert.Equal(expectedString, version.ToString());
			Assert.False(version.HasWildcard);
		}

		#region Test cases

		public static IEnumerable<object[]> ConstructValidVersionWithWildcards_TestCases()
		{
			yield return new object[] { 1, 0, 0, "*", "1.0.0-*" };

			foreach (var prerelease in new[] { "*", null })
			{
				yield return new object[] { 1, 0, null, prerelease, "1.0.*" };
				yield return new object[] { 1, null, null, prerelease, "1.*" };
				yield return new object[] { null, null, null, prerelease, "*" };
			}
		}

		[Theory, MemberData(nameof(ConstructValidVersionWithWildcards_TestCases))] 
		#endregion
		public void ConstructValidVersionWithWildcards(int? major, int? minor, int? patch, string prerelease, string expectedString)
        {
            var version = new SemanticVersion(major, minor, patch, prerelease);

            Assert.Equal(expectedString, version.ToString());
			Assert.True(version.HasWildcard);
        }

		public void ConstructVersionWithBuildWildcard()
		{
			var version = new SemanticVersion(1, 2, 3, "beta", "*");

			Assert.True(string.IsNullOrEmpty(version.Build));
			Assert.False(version.HasWildcard);
		}

		#region Test cases

		[Theory]
		[InlineData(null, null, null, "beta")]
		[InlineData(null, null, 1, null)]
		[InlineData(null, 1, null, null)]
		[InlineData(1, null, null, "beta")]
		[InlineData(1, 1, null, "beta")] 

		#endregion
		public void ConstructVersionWithValueAfterWildcards(int? major, int? minor, int? patch, string prerelease)
        {
            Assert.ThrowsAny<ArgumentException>(() => new SemanticVersion(major, minor, patch, prerelease));
        }

		#region Test cases

		[Theory]
		[InlineData(-1, 0, 0, "beta", "12")]
		[InlineData(1, -1, 0, "beta", "12")]
		[InlineData(1, 0, -1, "beta", "12")]
		[InlineData(1, 0, 0, "be=ta", "12")]
		[InlineData(1, 0, 0, "beta", "+12")]
		[InlineData(1, 0, 0, "beta", "1:2")]

		#endregion
		public void ConstructVersionWithIncorrectComponents(int? major, int? minor, int? patch, string prerelease, string build)
		{
			Assert.ThrowsAny<ArgumentException>(() => new SemanticVersion(major, minor, patch, prerelease, build));
		}
	}
}

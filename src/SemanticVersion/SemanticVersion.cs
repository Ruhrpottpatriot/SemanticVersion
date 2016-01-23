namespace SemVersion
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;


    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    /// <summary>
    /// Reprensents a verrsion, compliant with the Semantic Version standard 2.0 (http://semver.org)
    /// </summary>
    public class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion>
    {
        private static readonly VersionComparer Comparer = new VersionComparer();

        private static readonly Regex VersionExpression = new Regex(@"^(?<major>[0-9]+|[*])(\.(?<minor>[0-9]+|[*]))?(\.(?<patch>[0-9]+|[*]))?(\-(?<pre>[0-9A-Za-z\-\.]+|[*]))?(\+(?<build>[0-9A-Za-z\-\.]+|[*]))?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        /// <summary>Initializese a new instance of the <see cref="SemanticVersion"/> class.</summary>
        /// <param name="major">The major version component.</param>
        /// <param name="minor">The minor version component.</param>
        /// <param name="patch">The patch version component.</param>
        /// <param name="prerelease">The pre-release version component.</param>
        /// <param name="build">The build version component.</param>
        public SemanticVersion(int? major, int? minor, int? patch, string prerelease = "", string build = "")
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.Prerelease = prerelease;
            this.Build = build;
        }

        /// <summary>Gets the major version component.</summary>
        public int? Major { get; }

        /// <summary>Gets the minor version component.</summary>
        public int? Minor { get; }

        /// <summary>Gets the patch version component.</summary>
        public int? Patch { get; }

        /// <summary>Gets the pre-release version component.</summary>
        public string Prerelease { get; }

        /// <summary>Gets the build version component.</summary>
        public string Build { get; }

        public static bool operator ==(SemanticVersion left, SemanticVersion right)
        {
            return Comparer.Equals(left, right);
        }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
        {
            return !Comparer.Equals(left, right);
        }

        public static bool operator <(SemanticVersion left, SemanticVersion right)
        {
            return Comparer.Compare(left, right) < 0;
        }

        public static bool operator >(SemanticVersion left, SemanticVersion right)
        {
            return Comparer.Compare(left, right) > 0;
        }

        public static bool operator <=(SemanticVersion left, SemanticVersion right)
        {
            return left == right || left < right;
        }

        public static bool operator >=(SemanticVersion left, SemanticVersion right)
        {
            return left == right || left > right;
        }

        /// <summary>Implicitly converts a string into a <see cref="SemanticVersion"/>.</summary>
        /// <param name="versionString">The string to convert.</param>
        /// <returns>The <see cref="SemanticVersion"/> object.</returns>

        public static implicit operator SemanticVersion(string versionString)
        {
            return Parse(versionString);
        }

        /// <summary>Explicitly converts a <see cref="System.Version"/> onject into a <see cref="SemanticVersion"/>.</summary>
        /// <param name="dotNetVersion">The version to convert.</param>
        /// <remarks>
        /// <para>This operator conversts a C# <see cref="System.Version"/> object into the corresponding <see cref="SemanticVersion"/> object.</para>
        /// <para>Note, that with a C# version the <see cref="System.Version.Build"/> property is identical to the <see cref="Patch"/> propertry on a semantic version compliant object.
        /// Whereas the <see cref="System.Version.Revision"/> property is equivalent to the <see cref="Build"/> property on a semantic version.
        /// The <see cref="Prerelease"/> property is never set, since the C# version object does not use such a notation.</para>
        /// </remarks>
        public static explicit operator SemanticVersion(Version dotNetVersion)
        {
            if (dotNetVersion == null)
            {
                throw new ArgumentNullException(nameof(dotNetVersion), "The version to convert was null.");
            }

            int major = dotNetVersion.Major;
            int minor = dotNetVersion.Minor;
            int patch = dotNetVersion.Build >= 0 ? dotNetVersion.Build : 0;
            string build = dotNetVersion.Revision >= 0 ? dotNetVersion.Revision.ToString() : string.Empty;

            return new SemanticVersion(major, minor, patch, string.Empty, build);
        }

        public static SemanticVersion BaseVersion()
        {
            return new SemanticVersion(1, 0, 0);
        }

        public static bool IsVersion(string inputString)
        {
            return VersionExpression.IsMatch(inputString);
        }

        /// <summary>Parses the specified string to a semantic version.</summary>
        /// <param name="versionString">The version string.</param>
        /// <returns>A new <see cref="SemanticVersion"/> object that has the specified values.</returns>
        /// <exception cref="ArgumentException">Raised when the the whole version string is in an invalid format.</exception>
        /// <exception cref="InvalidOperationException">Raised when some part of the version string has an invalid format.</exception>
        public static SemanticVersion Parse(string versionString)
        {
            Match versionMatch = VersionExpression.Match(versionString);

            if (!versionMatch.Success)
            {
                throw new ArgumentException("The provided version string is invalid", nameof(versionString));
            }

            Group majorMatch = versionMatch.Groups["major"];
            if (majorMatch.Value == "*")
            {
                return new SemanticVersion(null, null, null);
            }
            int? major = int.Parse(majorMatch.Value, CultureInfo.InvariantCulture);

            Group minorMatch = versionMatch.Groups["minor"];
            if (minorMatch.Value == "*")
            {
                return new SemanticVersion(major, null, null);
            }
            int? minor = int.Parse(minorMatch.Value, CultureInfo.InvariantCulture);

            Group patchMatch = versionMatch.Groups["patch"];
            if (patchMatch.Value == "*")
            {
                return new SemanticVersion(major, minor, null);
            }
            int? patch = int.Parse(patchMatch.Value, CultureInfo.InvariantCulture);

            Group prereleaseMatch = versionMatch.Groups["pre"];
            string prerelease = prereleaseMatch.Value != "*" ? prereleaseMatch.Value : string.Empty;

            Group buildMatch = versionMatch.Groups["build"];
            string build = buildMatch.Value != "*" ? buildMatch.Value : string.Empty;


            return new SemanticVersion(major, minor, patch, prerelease, build);
        }

        /// <summary>Tries to parse the specified string into a semantic version.</summary>
        /// <param name="versionString">The version string.</param>
        /// <param name="version">When the method returns, contains a SemVersion instance equivalent 
        /// to the version string passed in, if the version string was valid, or <c>null</c> if the 
        /// version string was not valid.</param>
        /// <returns><c>False</c> when a invalid version string is passed, otherwise <c>true</c>.</returns>
        public static bool TryParse(string versionString, out SemanticVersion version)
        {
            try
            {
                version = Parse(versionString);
                return true;
            }
            catch (Exception)
            {
                version = null;
                return false;
            }
        }

        /// <summary>Determines whether the specified <see cref="SemanticVersion" /> is equal to this instance.</summary>
        /// <param name="other">The <see cref="SemanticVersion" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="SemanticVersion" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(SemanticVersion other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (this.Major != other.Major)
            {
                return false;
            }

            if (this.Minor != other.Minor)
            {
                return false;
            }

            if (this.Patch != other.Patch)
            {
                return false;
            }

            if (this.Prerelease != other.Prerelease)
            {
                return false;
            }

            if (this.Build != other.Build)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates
        /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings: Value Meaning Less than zero
        /// This instance precedes <paramref name="obj" /> in the sort order.
        /// Zero This instance occurs in the same position in the sort order as <paramref name="obj" />.
        /// Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            SemanticVersion version = obj as SemanticVersion;
            return version == null ? 1 : this.CompareTo(version);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates 
        /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the 
        /// other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings: Value Meaning Less than zero
        /// This instance precedes <paramref name="other" /> in the sort order.
        /// Zero This instance occurs in the same position in the sort order as <paramref name="other" />.
        /// Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(SemanticVersion other)
        {
            int compare = this.PrecendenceCompareTo(other);
            return compare != 0 ? compare : -this.Build.CompareComponent(other.Build);
        }

        /// <summary>
        /// Compares the current instance with another instance of the same type for precedence
        /// as described in the 2.0 standard and returns an integer that indicates whether
        /// the current instance precedes, follows, or occurs in the same position in the sort order as the 
        /// other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings: Value Meaning Less than zero
        /// This instance precedes <paramref name="other" /> in the sort order.
        /// Zero This instance occurs in the same position in the sort order as <paramref name="other" />.
        /// Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int PrecendenceCompareTo(SemanticVersion other)
        {
            if ((object)other == null)
            {
                return 1;
            }

            int majorComp = this.Major.CompareToBoxed(other.Major);
            if (majorComp != 0)
            {
                return majorComp;
            }

            int minorComp = this.Minor.CompareToBoxed(other.Minor);
            if (minorComp != 0)
            {
                return minorComp;
            }

            int patchComp = this.Patch.CompareToBoxed(other.Patch);
            if (patchComp != 0)
            {
                return patchComp;
            }

            return this.Prerelease.CompareComponent(other.Prerelease);
        }

        /// <summary>Determines whether the specified <see cref="SemanticVersion" /> is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            SemanticVersion other = obj as SemanticVersion;
            return other != null && this.Equals(other);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Major ?? 0;
                hashCode = (hashCode * 397) ^ this.Minor ?? 0;
                hashCode = (hashCode * 397) ^ this.Patch ?? 0;
                hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Prerelease) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Prerelease) : 0);
                hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Build) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Build) : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (this.Major.HasValue)
            {
                builder.Append($"{this.Major.Value}.");
            }
            else
            {
                return "*";
            }

            if (this.Minor.HasValue)
            {
                builder.Append($"{this.Minor.Value}.");
            }
            else
            {
                builder.Append("*");
                return builder.ToString();
            }

            if (this.Patch.HasValue)
            {
                builder.Append($"{this.Patch.Value}");
            }
            else
            {
                builder.Append("*");
                return builder.ToString();
            }
            
            builder.Append(string.IsNullOrWhiteSpace(this.Prerelease) ? string.Empty : $"-{this.Prerelease}");
            builder.Append(string.IsNullOrWhiteSpace(this.Build) ? string.Empty : $"+{this.Build}");

            return builder.ToString();
        }
    }
}

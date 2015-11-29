namespace SemanticVersion
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;
#if DNX451
#endif

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    /// <summary>
    /// Reprensents a verrsion, compliant with the Semantic Version standard 2.0 (http://semver.org)
    /// </summary>
    public class Version : IEquatable<Version>, IComparable<Version>
    {
        private static readonly VersionComparer Comparer = new VersionComparer();

        private static readonly Regex VersionExpression = new Regex(@"^(?<major>\d+)(\.(?<minor>\d+))?(\.(?<patch>\d+))?(\-(?<pre>[0-9A-Za-z\-\.]+))?(\+(?<build>[0-9A-Za-z\-\.]+))?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        /// <summary>Initializese a new instance of the <see cref="Version"/> class.</summary>
        /// <param name="major">The major version component.</param>
        /// <param name="minor">The minor version component.</param>
        /// <param name="patch">The patch version component.</param>
        /// <param name="prerelease">The pre-release version component.</param>
        /// <param name="build">The build version component.</param>
        public Version(int major, int minor, int patch, string prerelease = "", string build = "")
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.Prerelease = prerelease;
            this.Build = build;
        }

        /// <summary>Gets the major version component.</summary>
        public int Major { get; }

        /// <summary>Gets the minor version component.</summary>
        public int Minor { get; }

        /// <summary>Gets the patch version component.</summary>
        public int Patch { get; }

        /// <summary>Gets the pre-release version component.</summary>
        public string Prerelease { get; }

        /// <summary>Gets the build version component.</summary>
        public string Build { get; }

        public static bool operator ==(Version left, Version right)
        {
            return Comparer.Equals(left, right);
        }

        public static bool operator !=(Version left, Version right)
        {
            return !Comparer.Equals(left, right);
        }

        public static bool operator <(Version left, Version right)
        {
            return Comparer.Compare(left, right) < 0;
        }

        public static bool operator >(Version left, Version right)
        {
            return Comparer.Compare(left, right) > 0;
        }

        public static bool operator <=(Version left, Version right)
        {
            return left == right || left < right;
        }

        public static bool operator >=(Version left, Version right)
        {
            return left == right || left > right;
        }

        /// <summary>Implicitly converts a string into a <see cref="Version"/>.</summary>
        /// <param name="versionString">The string to convert.</param>
        /// <returns>The <see cref="Version"/> object.</returns>
#if DNX451
        [SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier", Justification = "Qualifier is intended here.")]
#endif
        public static implicit operator Version(string versionString)
        {
            return Version.Parse(versionString);
        }

        /// <summary>Explicitly converts a <see cref="System.Version"/> onject into a <see cref="Version"/>.</summary>
        /// <param name="dotNetVersion">The version to convert.</param>
        /// <remarks>
        /// <para>This operator conversts a C# <see cref="System.Version"/> object into the corresponding <see cref="Version"/> object.</para>
        /// <para>Note, that with a C# version the <see cref="System.Version.Build"/> property is identical to the <see cref="Patch"/> propertry on a semantic version compliant object.
        /// Whereas the <see cref="System.Version.Revision"/> property is equivalent to the <see cref="Build"/> property on a semantic version.
        /// The <see cref="Prerelease"/> property is never set, since the C# version object does not use such a notation.</para>
        /// </remarks>
        public static explicit operator Version(System.Version dotNetVersion)
        {
            if (dotNetVersion == null)
            {
                throw new ArgumentNullException(nameof(dotNetVersion), "The version to convert was null.");
            }

            int major = dotNetVersion.Major;
            int minor = dotNetVersion.Minor;
            int patch = dotNetVersion.Build >= 0 ? dotNetVersion.Build : 0;
            string build = dotNetVersion.Revision >= 0 ? dotNetVersion.Revision.ToString() : string.Empty;

            return new Version(major, minor, patch, string.Empty, build);
        }

        public static Version BaseVersion()
        {
            return new Version(1, 0, 0);
        }

        /// <summary>Parses the specified string to a semantic version.</summary>
        /// <param name="versionString">The version string.</param>
        /// <returns>A new <see cref="Version"/> object that has the specified values.</returns>
        /// <exception cref="ArgumentException">Raised when the the whole version string is in an invalid format.</exception>
        /// <exception cref="InvalidOperationException">Raised when some part of the version string has an invalid format.</exception>
        public static Version Parse(string versionString)
        {
            Match match = VersionExpression.Match(versionString);

            if (!match.Success)
            {
                throw new ArgumentException("The provided version string is invalid", versionString);
            }

            int major = int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture); Group minorMatch = match.Groups["minor"];

            int minor;
            if (minorMatch.Success)
            {
                minor = int.Parse(minorMatch.Value, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new InvalidOperationException("The version string was invalid. No minor version was given.");
            }

            Group patchMatch = match.Groups["patch"];
            int patch;
            if (patchMatch.Success)
            {
                patch = int.Parse(patchMatch.Value, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new InvalidOperationException("The version string was invalid. No patch was given.");
            }

            string prerelease = match.Groups["pre"].Value;
            string build = match.Groups["build"].Value;

            return new Version(major, minor, patch, prerelease, build);
        }

        /// <summary>Tries to parse the specified string into a semantic version.</summary>
        /// <param name="versionString">The version string.</param>
        /// <param name="version">When the method returns, contains a SemVersion instance equivalent 
        /// to the version string passed in, if the version string was valid, or <c>null</c> if the 
        /// version string was not valid.</param>
        /// <returns><c>False</c> when a invalid version string is passed, otherwise <c>true</c>.</returns>
        public static bool TryParse(string versionString, out Version version)
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

        /// <summary>Determines whether the specified <see cref="Version" /> is equal to this instance.</summary>
        /// <param name="other">The <see cref="Version" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Version" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Version other)
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
            Version version = obj as Version;
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
        public int CompareTo(Version other)
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
        public int PrecendenceCompareTo(Version other)
        {
            if ((object)other == null)
            {
                return 1;
            }

            if (this.Major != other.Major)
            {
                return this.Major.CompareTo(other.Major);
            }

            if (this.Minor != other.Minor)
            {
                return this.Minor.CompareTo(other.Minor);
            }

            if (this.Patch != other.Patch)
            {
                return this.Patch.CompareTo(other.Patch);
            }

            return this.Prerelease.CompareComponent(other.Prerelease);
        }

        /// <summary>Determines whether the specified <see cref="Version" /> is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Version other = obj as Version;
            return other != null && this.Equals(other);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Major;
                hashCode = (hashCode * 397) ^ this.Minor;
                hashCode = (hashCode * 397) ^ this.Patch;
                hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Prerelease) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Prerelease) : 0);
                hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Build) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Build) : 0);
                return hashCode;
            }
        }



        public override string ToString()
        {
            string prerelease = string.IsNullOrWhiteSpace(this.Prerelease) ? string.Empty : $"-{this.Prerelease}";
            string build = string.IsNullOrWhiteSpace(this.Build) ? string.Empty : $"+{this.Build}";

            return $"{this.Major}.{this.Minor}.{this.Patch}{prerelease}{build}";
        }


    }
}

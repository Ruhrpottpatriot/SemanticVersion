namespace Semver
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    /// <summary>
    /// Reprensents a verrsion, compliant with the Semantic Version standard 2.0 (http://semver.org)
    /// </summary>
    public class SemanticVersion : IEquatable<SemanticVersion>
    {
        private static readonly SemanticVersionEqualityComparer EqualityComparer = new SemanticVersionEqualityComparer();

        private static readonly Regex VersionExpression = new Regex(@"^(?<major>\d+)(\.(?<minor>\d+))?(\.(?<patch>\d+))?(\-(?<pre>[0-9A-Za-z\-\.]+))?(\+(?<build>[0-9A-Za-z\-\.]+))?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        /// <summary>Initializese a new instance of the <see cref="SemanticVersion"/> class.</summary>
        /// <param name="major">The major version component.</param>
        /// <param name="minor">The minor version component.</param>
        /// <param name="patch">The patch version component.</param>
        /// <param name="prerelease">The pre-release version component.</param>
        /// <param name="build">The build version component.</param>
        public SemanticVersion(int major, int minor, int patch, string prerelease = "", string build = "")
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
        
        public static bool operator ==(SemanticVersion left, SemanticVersion right)
        {
            return EqualityComparer.Equals(left, right);
        }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
        {
            return !EqualityComparer.Equals(left, right);
        }

        /// <summary>Implicitly converts a string into a <see cref="SemanticVersion"/>.</summary>
        /// <param name="versionString">The string to convert.</param>
        /// <returns>The <see cref="SemanticVersion"/> object.</returns>
        [SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier", Justification = "Qualifier is intended here.")]
        public static implicit operator SemanticVersion(string versionString)
        {
            return SemanticVersion.Parse(versionString);
        }

        /// <summary>Parses the specified string to a semantic version.</summary>
        /// <param name="versionString">The version string.</param>
        /// <returns>A new <see cref="SemanticVersion"/> object that has the specified values.</returns>
        /// <exception cref="ArgumentException">Raised when the the whole version string is in an invalid format.</exception>
        /// <exception cref="InvalidOperationException">Raised when some part of the version string has an invalid format.</exception>
        public static SemanticVersion Parse(string versionString)
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

        /// <summary>Determines whether the specified <see cref="SemanticVersion" /> is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            SemanticVersion other = obj as SemanticVersion;
            return other != null && this.Equals(other);
        }

        /// <summary>
        /// Fungiert als Hashfunktion für einen bestimmten Typ.
        /// </summary>
        /// <returns>
        /// Ein Hashcode für das aktuelle Objekt.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Major;
                hashCode = (hashCode * 397) ^ this.Minor;
                hashCode = (hashCode * 397) ^ this.Patch;
                hashCode = (hashCode * 397) ^ (this.Prerelease != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Prerelease) : 0);
                hashCode = (hashCode * 397) ^ (this.Build != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Build) : 0);
                return hashCode;
            }
        }
    }
}

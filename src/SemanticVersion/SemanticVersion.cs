namespace Semver
{
    using System;
#if DNX451 
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.Globalization;
    using System.Text.RegularExpressions;

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    /// <summary>
    /// Reprensents a verrsion, compliant with the Semantic Version standard 2.0 (http://semver.org)
    /// </summary>
    public class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion>
    {
        private static readonly SemanticVersionComparer Comparer = new SemanticVersionComparer();

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
#if DNX451
        [SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier", Justification = "Qualifier is intended here.")]
#endif
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
            SemanticVersion semanticVersion = obj as SemanticVersion;
            return semanticVersion == null ? 1 : this.CompareTo(semanticVersion);
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
            return compare != 0 ? compare : this.CompareComponentString(this.Build, other.Build);
        }

        public int PrecendenceCompareTo(SemanticVersion other)
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
            
            return this.CompareComponentString(this.Prerelease, other.Prerelease, true);
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
                int hashCode = this.Major;
                hashCode = (hashCode * 397) ^ this.Minor;
                hashCode = (hashCode * 397) ^ this.Patch;
                hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Prerelease) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Prerelease) : 0);
                hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Build) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Build) : 0);
                return hashCode;
            }
        }

        /// <summary>Compares two component parts for equality.</summary>
        /// <param name="left"> The left part to compare.</param>
        /// <param name="right">The right part to compare.</param>
        /// <param name="leftIsLowerPart"><c>True</c> if the left part should be treated as the lower part in the comparison.</param>
        private int CompareComponentString(string left, string right, bool leftIsLowerPart = false)
        {
            bool leftIsEmpty = string.IsNullOrWhiteSpace(left);
            bool rightIsEmpty = string.IsNullOrWhiteSpace(right);

            if (leftIsEmpty && rightIsEmpty)
            {
                return 0;
            }

            if (leftIsEmpty)
            {
                return leftIsLowerPart ? 1 : -1;
            }

            if (rightIsEmpty)
            {
                return leftIsLowerPart ? -1 : 1;
            }

            string[] partsLeft = left.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string[] partsRight = right.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Math.Min(partsLeft.Length, partsRight.Length); i++)
            {
                string leftCh = partsLeft[i];
                string rightCh = partsRight[i];

                int leftNumVal, rightNumVal;
                bool leftIsNum = int.TryParse(leftCh, out leftNumVal);
                bool rightIsNum = int.TryParse(rightCh, out rightNumVal);

                if (leftIsNum && rightIsNum)
                {
                    if (leftNumVal.CompareTo(rightNumVal) == 0)
                    {
                        continue;
                    }
                    return leftNumVal.CompareTo(rightNumVal);
                }
                else
                {
                    if (leftIsNum)
                    {
                        return -1;
                    }

                    if (rightIsNum)
                    {
                        return 1;
                    }

                    int comp = string.Compare(leftCh, rightCh, StringComparison.OrdinalIgnoreCase);
                    if (comp != 0)
                    {
                        return comp;
                    }
                }
            }

            return partsLeft.Length.CompareTo(partsRight.Length);
        }
    }
}

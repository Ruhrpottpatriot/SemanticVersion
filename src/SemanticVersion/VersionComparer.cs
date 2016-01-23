namespace SemVersion
{
    using System.Collections.Generic;

    public sealed class VersionComparer : IEqualityComparer<SemanticVersion>, IComparer<SemanticVersion>
    {
        /// <summary>Compares two instances of <see cref="SemanticVersion"/> for equality.</summary>
        /// <param name="left">The first version.</param>
        /// <param name="right">The second version.</param>
        /// <returns></returns>
        public bool Equals(SemanticVersion left, SemanticVersion right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null))
            {
                return false;
            }

            if (ReferenceEquals(right, null))
            {
                return false;
            }

            if (left.GetType() != right.GetType())
            {
                return false;
            }

           return left.Equals(right);
        }

        /// <summary>Compares the specified versions.</summary>
        /// <param name="left">The version to compare to.</param>
        /// <param name="right">The version to compare against.</param>
        /// <returns>If versionA &lt; versionB <c>&lt; 0</c>, if versionA &gt; versionB <c>&gt; 0</c>,
        /// if versionA is equal to versionB <c>0</c>.</returns>
        public int Compare(SemanticVersion left, SemanticVersion right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null) ? 0 : -1;
            }

            return left.CompareTo(right);
        }

        public int GetHashCode(SemanticVersion obj)
        {
            return obj.GetHashCode();
        }
    }
}

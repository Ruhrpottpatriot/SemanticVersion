namespace Semver
{
    using System;
    using System.Collections.Generic;

    public sealed class SemanticVersionEqualityComparer : IEqualityComparer<SemanticVersion>
    {
        public bool Equals(SemanticVersion x, SemanticVersion y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (ReferenceEquals(x, null))
            {
                return false;
            }
            if (ReferenceEquals(y, null))
            {
                return false;
            }
            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.Major == y.Major
                && x.Minor == y.Minor
                && x.Patch == y.Patch
                && string.Equals(x.Prerelease, y.Prerelease, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(x.Build, y.Build, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(SemanticVersion obj)
        {
            return obj.GetHashCode();
        }
    }
}

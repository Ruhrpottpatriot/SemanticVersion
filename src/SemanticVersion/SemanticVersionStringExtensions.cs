namespace SemVersion
{
    using System;

    /// <summary>Contains extensions to the string class to improve comparison.</summary>
    internal static class SemanticVersionStringExtensions
    {
        /// <summary>Compares two component parts for equality.</summary>
        /// <param name="component"> The left part to compare.</param>
        /// <param name="other">The right part to compare.</param>
        internal static int CompareComponent(this string component, string other)
        {
            var componentEmpty = string.IsNullOrWhiteSpace(component);
            var otherEmpty = string.IsNullOrWhiteSpace(other);

            if ((componentEmpty && otherEmpty) || (component == "*" && other == "*"))
            {
                return 0;
            }

            if (componentEmpty || component == "*")
            {
                return +1;
            }

            if (otherEmpty || other == "*")
            {
                return -1;
            }

            var componentParts = component.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var otherParts = other.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < Math.Min(componentParts.Length, otherParts.Length); i++)
            {
                var componentChar = componentParts[i];
                var otherChar = otherParts[i];

                var componentIsNum = int.TryParse(componentChar, out var componentNumVal);
                var otherIsNum = int.TryParse(otherChar, out var otherNumVal);

                if (componentIsNum && otherIsNum)
                {
                    if (componentNumVal.CompareTo(otherNumVal) == 0)
                    {
                        continue;
                    }
                    return componentNumVal.CompareTo(otherNumVal);
                }

                if (componentIsNum)
                {
                    return -1;
                }

                if (otherIsNum)
                {
                    return 1;
                }

                var comp = string.Compare(componentChar, otherChar, StringComparison.OrdinalIgnoreCase);
                if (comp != 0)
                {
                    return comp;
                }
            }

            return componentParts.Length.CompareTo(otherParts.Length);
        }
    }
}

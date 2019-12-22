namespace SemVersion.Parser
{
    using System;

    /// <summary>Represents a parenthesis in a version query.</summary>
    internal class Parentheses : Symbol
    {
        /// <summary>A left/opening parenthesis.</summary>
        public static readonly Parentheses Left = new Parentheses();

        /// <summary>A right/closing parenthesis.</summary>
        public static readonly Parentheses Right = new Parentheses();

        /// <summary>Initializes a new instance of the <see cref="Parentheses"/> class.</summary>
        private Parentheses()
        {
        }

        /// <summary>Explicitly converts a char to a parenthesis symbol.</summary>
        public static explicit operator Parentheses(char parenthesis)
        {
            switch (parenthesis)
            {
                case '(':
                    return Left;
                case ')':
                    return Right;
                default:
                    throw new InvalidCastException($"Could not cast char '{parenthesis}' to parenthesis.");
            }
        }
    }
}
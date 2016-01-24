# SemanticVersion

## Abstract
This project aims to implement the complete 2.0 semantic versions standard as a portable asp.net/DnxCore library. In addition to 2.0 standard features it extends the standard to allow users to specify version ranges via simple to use and know C# operators.

## Description
Current implemetations of the semantic versions standard are either only implemented as full .NET projects, or old PCL profiles. For newer ASP.NET/DnxCore based projects these implementations often could not be used appropriately, if at all. This project aims to provide a simple to use and easily extendable implementation of said standard.
### Equality
There are two ways to to compare two versions for equality. However these methods differ subtle, but can produce a vastly different outcome.
Any call to `SemanticVersion.Equals` compares a version for total equality. This means that Major, Minor, Patch, Prerelease *and* Build components are compared for equality. On the other hand the `SemanticVersion.CompareTo` method compares two semantic versions as described in the 2.0 standard (i.e. only Major, Minor, Patch and Prerelease components).
The `==` and `!=` operators do not compare equality via the `SemanticVersion.Equals` method, but rather the `SemanticVersion.CompareTo`. This was done by purpose, since every other comparison operator (i.e `<`, `<=`, `>`, `>=`) also compare via `SemanticVersion.CompareTo`. This also enables version range parsing with more predictable results.
The library also supports an implicit cast from a string, which will throw an exception if the string can't be parsed into a suitable semantic version. It also supports explicit casts from a C# version struct. Note, that with a C# version the *Build* property is identical to the *Patch* propertry on a semantic version compliant object. Whereas the *Revision* property is equivalent to the *Build* proerty on a semantic version. *Prerelease* property is never set, since the C# version object does not use such a notation.


### Version Ranges
In addition to the standard this implementation includes a way to specify a version range that can be matched against either a single version or another range.
The range syntax is as follows:
* **Partial:** A `*` many be substituted for any part of the version. Any part following the the "\*" is then ignored and does not factor into version order. *Example:* 1.2.* equals to any version greater than or equal to 1.2, including prereleases.
* **Comparison Operators:** The `<`, `<=`, `>`, `>=` comparison operators define a range based on the specified version right side of the operator. *Example:* >= 1.0.0 equals to any version greater than  or equal to 1.0.0
* **Logical Or:** A `||` is used to delimit two or more versions (or combionations thereof) with a logical or. *Example:* 1.2.0-alpha || 1.0.0 equals to either version 1.0.0 or version 1.2.0-alpha
* **Logical And:** A `&&` is used to delimit two or more versions (or combionations thereof) with a logical and. *Example:* >= 1.0.0 && < 2.0.0 equals to any version greater than or equal to 1.0.0 but smaller than 2.0.0
* **Logical Not** A `!` is used to specify a version that should not be included. *Example:* >= 1.0.0 && < 2.0.0 && !1.5.0 equals to any version between 1.0.0 and 2.0.0 but *not* version 1.5.0
* **Parentheses:** `(` and `)` may be used to decide a version range befor another (since precedence factors in). *Example:* (1.2.0-alpha || 1.0.0) && !2.0.0

#### Operator Precedence
The higher a precedence, the higher the priority of deciding a particular expression. *Parentheses* have the highest precedence, followed by the *logical and*. The *logical or* has the third higest precedence, while any *comparison operator* (including the *logical not*) have the lowest precedence.

#### Range parsing
This library also includes a range parser taht parses a given range string into the appropriate expression tree via the `RangeParser.Parse` method. The parser can also compile the expression into a suitable `Func<SemanticVersion, bool>` via the `RangeParser.Evaluate` method, although this comes with a certain time cost.
**Example:**
```
    public static void Main(string[] args)
    {
        RangeParser parser = new RangeParser();
        Func<SemanticVersion, bool> rangeFunc = parser.Evaluate(">= 1.0.0 && < 2.0.0")

        if(rangeFunc(new SemanticVersion(1,2,0, "alpha")))
            Console.WriteLine("Passed")
        else
            Console.WriteLine("Failed")
    }
```

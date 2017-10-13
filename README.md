# SemanticVersion.NET
[![AltaModa dev CI](https://altamoda.visualstudio.com/_apis/public/build/definitions/0939118e-8ff6-4dd0-904f-67c3adf1aae6/3/badge)]()
[![Visual Studio Team services](https://img.shields.io/vso/build/larsbrinkhoff/953a34b9-5966-4923-a48a-c41874cfb5f5/1.svg)]()

[![Ruhrpot Patriot dev status](https://ci.appveyor.com/api/projects/status/a1lsuyalpn9u51ot?svg=true)](https://ci.appveyor.com/project/Ruhrpottpatriot/semanticversion)
[![Ruhrpot Patriot master status](https://ci.appveyor.com/api/projects/status/a1lsuyalpn9u51ot/branch/master?svg=true)](https://ci.appveyor.com/project/Ruhrpottpatriot/semanticversion/branch/master)

## Abstract
This project aims to implement the complete 2.0 semantic versions standard as a portable ASP.NET/DnxCore library. In addition to 2.0 standard features it extends the standard to allow users to specify version ranges via simple to use and know C# operators.

## Description
Current implementations of the semantic versions standard available on NuGet are either implemented as full .NET projects, or old PCL profiles. In many cases the last update to the codebase was from over a year ago and an update is not likely anymore. Since full .NET versions can't be used from ASP.NET/DnxCore projects and PCLs are sometimes a bit "wonky", a implementation based entirely on the new ASP.NET/DnxCore was in order.
This library aims to provide a strict implementation of the version standard with extras added *on top* of it. This means, that every single semantic version is also a valid version for this library, but not necessary vice versa.

An extension of the the standard includes a method to parse multiple versions from a string into a set of versions (also called: *version range*) and partial versions. Partial versions are mostly syntactic sugar for a version range and will be described in more detail below.

### Equality
By default this library compares two semantic versions as described in the standard. This means, that `1.0.0 == 1.0.0 == 1.0.0+1234` equals to true, but `1.2.0 == 1.2.0-alpha` equals to false.
If a different comparison behaviour is intended a user can roll out their own implementation of the `IComparer<SemanticVersion>` interface and pass it to the version via the static factory method `SemanticVersion.ChangeComparer`. Usually it is not necessary to  implement the `IEqualityComparer<T>` interface, since two versions are considered equal if `CompareTo == 0` applies. However the reference equality comparer implements this interface, too.

### Parsing
This library supports implicit casting from strings into a suitable semantic version object. Parsing via an implicit conversion will -- like the `SemanticVersion.Parse` method -- throw an error, if the string has the wrong format. Additionally the `SemanticVersion` class includes a `TryParse` method which returns `true` if the cast was successful. See the documentation for additional details.
Some users might want to convert from a C# version object into an appropriate semantic version. Since a C# version object handles builds and prereleases different from semantic versions and any conversion inevitably leads to data loss, this library only supports explicit casting. With this method, the C# *Build* property is identical to the *Patch* property, whereas the *Revision* property is equivalent to the *Build* property. A *Prerelease* property is never set.

### Version Ranges
As describes above, this implementation supports a set of versions, mainly to describe dependencies against other APIs. A goal was to make version ranges as intuitive as possible. Thus, many C# logic operators apply to version ranges. Currently the supported operators are:
* OrElse (`x || y`)
* AndAlso (`x && y`)
* Not (`!x`)

In addition to the above operators, any equality (`==`/`!=`) and ordering (`<`, `<=`, `>`, `>=`) operators can be used to create version ranges. The syntax requires the variable to reside on the left side of the operator (i.e. `x >= 1.0.0`), however the variable may be omitted entirely.

If an expression has to be handled before another expression the expression can be wrapped into the `(` and `)` parentheses. This will raise the expressions precedence to the highest level. Nested parentheses are also possible.

#### Operator Precedence
All expressions in this library are left-assiciative, meaning if two expression with the same precedence have to be evaluated, the goes from left to right. Implicitly an expression with a higher precedence is evaluated before any expression with a lower precedence. The current precedence order is (frigh high to low):
* Parentheses
* AndAlso
* OrElse
* Equality, Ordering, Not

#### Range parsing
Parsing a version range from a string can be beneficial on many occasions. Thus this library includes a version range parser, that parses a version range from a string into the appropriate `Expression<Func<SemanticVersion, bool>>`, which the user can further alter to suit his needs. In most cases the `RangeParser.Evaluate` method will suffice, since it compiles the expression into a usable `Func<SemanticVersion, bool>`, although this comes with a certain performance cost.
Since the `RangeParser.Parse` and `RangeParser.Evaluate` methods throw exceptions when encountering an error, the library also includes a `RangeParser.TryParse` and `RangeParser.TryEvaluate` method.

## Eamples
### Example 1, Creating a version with a constructor
```
SemVer.SemanticVersion version = new SemVer.SemanticVersion(1,0,0, "alpha", "1234");
```

### Example 2, Implicitly converting a string into a SemanticVersion
```
SemVer.SemanticVersion version = "1.0.0-alpha.1+1234"
```

### Example 3, Parsing a version range and compare to specific version
```
SemVer.Parser.RangeParser parser = new SemVer.Parser.RangeParser();
Func<SemVer.SemanticVersion, bool> range = parser.Evaluate(">= 1.0.0 && < 2.0.0");

if(range(new SemVer.SemanticVersion(1,2,0, "alpha")))
    Console.WriteLine("Passed");
else
    Console.WriteLine("Failed");
```

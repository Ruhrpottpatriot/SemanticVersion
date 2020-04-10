The key words "MUST", "MUST NOT", "REQUIRED", "SHALL", "SHALL NOT", "SHOULD", "SHOULD NOT", "RECOMMENDED", "MAY", and "OPTIONAL" in this document are to be interpreted as described in [RFC 2119](http://tools.ietf.org/html/rfc2119).

The version system is a strict superset of the [2.0 Semantic version](http://semver.org/) standard. Every *single* version is also a valid **Semantic Version**

# 'x'-Modifier
For any of the Major, Minor or Patch version attributes an `x` MAY be substituted. This will declare the attribute as OPTIONAL.

**Example:**
* 1.x ⇔ >=1.0.0 ∧ <2.0.0
* x.2.3 ⇔ {1.2.3, 2.2.3, 3.2.3, ...}


# Ranges
More often than not a piece of software has to specify more than one version as dependency. A range can be constructed by combining a single version with a single comparison operator. The comparison operators are defined as follows:
* `<` Less than
* `<=` Less than or equal to
* `>` Greater than
* `>=` Greater than or equal to
* `==` Equal
* `!=` Not Equal

Multiple ranges can be joined by using a set of operators to for a new range. See below for the definition.
If A and B are version ranges and x a specific version, then a
* **Union** MUST be written as `A || B` := {x | (x ∈ A) ∨ (x ∈ B) }
* **Intersection** MUST be written as `A && B` := {x | (x ∈ A) ∧ (x ∈ B) }

﻿### TODO
- Ability to transform model before validation.
- Const Warnings like it was implemented for Const Errors.
- Exception on first error.
- Specific ValidationFuncs (?).

### 2.6.3 - 2025-10-15
- The source generated Regex applied to speed up.
- The func IsEmail() improved.
- The func IsPhoneNumber() improved.

### 2.6.2 - 2025-05-28
- Minor bug of the func IsAbsoluteUri() fixed.
- Minor performance improvements.
- Some codes clarification (suggested by DoctorKrolic).
- Parallel execution has been simplified.
- Tests have been improved.

### 2.6.1 - 2025-03-08
- _Obsolete_ methods removed.

### 2.6.0 - 2025-02-03

- Versions .NET 6.x and .NET 7.x do not supported now to avoid possible conflicts. \
This is because the package Microsoft.Extensions.DependencyInjection.Abstractions for the .NET 9 pushes warnings that it have already not supported them compeltely.
- `If(...)` operators have been replaced to `When(...)` to avoid any confusion for VB.NET. All `If(...)` operators have been marked as _obsolete_.
- Tests prepared to get rid of any warnings.

### 2.5.0 - 2024-11-18

- Support .NET 9.0
- Fix IsPhoneNumber func.

### 2.4.0 - 2024-09-20

- Overloaded built-in GetShannonEntropy added to return ShannonEntropy in bits.
- PasswordStrengthClaculator+ShannonEntropy sample updated.
- Usage 'When' instead of 'If'. 'If' marked as 'obsolete' due to possibile conflict with Basic's reserved word.
- Some tests updated.
- An example added.
- Spec updated.
 
### 2.3.0 - 2024-05-07

- IsPassword func modified.
- GetPasswordStrength func added.
- GetShannonEntropy func added.
- Tests for some IsPassword modified.
- Tests for some GetPasswordStrength added.
- Tests for some GetShannonEntropy added.
- Sample with password funcs usage added.
- Spec updated.

### 2.2.0 - 2024-03-17

- Performance improvements.
- Tests for some ValidationFuncs added.
- IsPassword func modified.
- PasswordHasDigit func removed.
- PasswordHasUpperChar func removed.
- PasswordHasLowerChar func removed.
- PasswordHasSpecialCharacter func removed.
- PasswordHasLengthAtLeast func removed.
- Bug fixed.


### 2.1.1 - 2024-03-16

- Performance improvements.
- Tests for some ValidationFuncs added.
- Specs fixed.


### 2.1.0 - 2024-02-27

- Performance improved.
- MetaData added. It allows to add custom info about validation process.
- Tests for MetaData added.
- Added EndpointFilter with validation in the example NativeAOT.
- Modified the example MinimalAPI+EndpoinFilter+Validation.


### 2.0.0 - 2024-02-01

- Add WarningIf functionality
- Tests for WarningIf added
- Warnings collection added into FlatValidatiorResult
- Grouped() replaced to If()
- Fix of documentations


### 1.0.0 - 2024-01-27

- Additional tests implemented 
- Fix of IFlatValidator interface to provide default CancellationToken
- Fix of warnings in part of method summaries
- Fix of documentation


### 1.0.0-rc - 2024-01-24

- Support asynchronous version of validator and rules.
- Support validating for several (3) properties in one rule.
- Support validating for nested properties by path.
- Support preconditions, support then/else statement.
- Allow inline mode of validation.

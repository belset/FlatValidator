### TODO
- Specific ValidationFuncs.
- Exception on first error.
- Const Warnings like it was implemented for Const Errors.
- Prop name and error message tuple.


### 2.3.0 - 2024-05-07

- IsPassword func modified.
- GetPasswordStrength func added.
- GetShannonEntropy func added.
- Tests for some IsPassword modified.
- Tests for some GetPasswordStrength added.
- Tests for some GetShannonEntropy added.
- Sample with password funcs usage added.

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

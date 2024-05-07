The `FlatValidator` is a validation library for .NET that delivers an high performance and memory prudence by using lambda-based and strongly-typed rules.

## Quick examples

In general, there are two simple ways to validate custom data with the `FlatValidator`.

### 1. Inline mode

> You can define validation rules in your code to validate object locally.

```js
var model = new SomeModel(Email: "email@email.com", BirthDate: DateTime.Now, Rate: -100);

// synchronous version
var result = FlatValidator.Validate(model, v =>
{
    // IsEmail() is built-in func for typical data formats 
    // like Email, Phone, Url, CreditCard, Password, etc.
    v.ValidIf(m => m.Email.IsEmail(), 
              m => $"Invalid email: {m.Email}", 
              m => m.Email);

    v.ErrorIf(m => m.Rate < 0, "Negative Rate", m => m.Rate);

    v.WarningIf(m => m.BirthDate.AddYears(10) >= DateTime.Now, 
                "Age looks like incorrect", m => m.BirthDate);
});

// or asynchronous version
var result = await FlatValidator.ValidateAsync(model, v => 
{
    v.ErrorIf(async m => await remoteApi.IsEmailBlockedAsync(m.Email),
              "Email is in black list.", m => m.Email);

    // the same without `async/await`
    v.ErrorIf(m => remoteApi.IsEmailBlockedAsync(m.Email),
              m => $"Email {m.Email} is in black list.", 
              m => m.Email);
});

// to check the validation result
if (!result) 
{
    // ToDictionary() => Dictionary<PropertyName, ErrorMessage[]>
    return TypedResults.ValidationProblem(result.ToDictionary()) 
}

// possibility to inspect occured validation failures
bool success = result.IsValid;
var errors = result.Errors;
var warnings = result.Warnings;

```

### 2. Inheritance

> Another way is to inherit the `FlatValidator` to define custom rules in the constructor. 
Also you can pass dependencies into constructor to get additional functionality inside of the validation rules.

```js
public record UserModel(string Forename, string Surname, ....);

public class UserValidator: FlatValidator<UserModel> 
{
    public UserValidator(ILogger logger, IPostalService postalService) 
    {
        logger.LogInfo("Validating...");

        ErrorIf(m => m.Forename.IsEmpty() || m.Surname.IsEmpty(),
                "Forename and Surname can not be empty.", 
                m => m.Forename, m => m.Surname);
        
        // use 'If(...)' to control a validation flow
        If(m => m.ShipmentAddress.NotEmpty(), @then: m =>
        {
            ValidIf(async m => await postalService.AddressExistsAsync(m.Address),
                     "Postal address not found.", m => m.Address);

            WarningIf(m => !m.Phone.IsPhone(), "No contact phone.");
        },
        @else: m => // @else section is optional
        {
            ValidIf(m => m.Phone.IsPhone(), "Invalid phone number.", m => m.Phone);
        });
    }
}
```
> Now lets validate some object with it
```js
// create instance of the custom validator
var validator = new UserValidator();

// validate _asynchronously_ and get a result
var result = await validator.ValidateAsync(customer, cancellationToken);

// OR validate _synchronously_ and get a result
var result = validator.Validate(new UserModel(...)); 

if (!result) // check, is there any errors?
{
    // ToDictionary() => Dictionary<PropertyName, ErrorMessage[]>
    var dict = result.ToDictionary();
    
    var errors = result.Errors;
    var warnings = result.Warnings;
}
```

> **TIP** -
> The package **`FlatValidator.DependencyInjection`** helps you to register all inherited validators in the ServiceCollection automatically.


### 3. Meta data
Using MetaData can extend functionality and can help to return certain data beyond the validator:
```js
var result = FlatValidator.Validate(model, v =>
{
    v.MetaData["ValidationTime"] = DateTime.UtcNow.ToString();
    // ....
});

// access to the MetaData value outside of the validation
return result.MetaData["ValidationTime"];
```

### 4. Built-in validators

1. Built-in validators for primitive data:
    - `ErrorIf(str => str.IsEmpty(), ...` - ensure the string is empty.
    - `ValidIf(str => str.NotEmpty(), ...` - ensure the string is not empty.
    - `ErrorIf(guid => guid.IsEmpty(), ...` - ensure the GUID is empty.
    - `ValidIf(guid => guid.NotEmpty(), ...` - ensure the GUID is not empty.
    - `ErrorIf(guid => guid.IsEmpty(), ...` - ensure the GUID? is null or empty.
    - `ValidIf(guid => guid.NotEmpty(), ...` - ensure the GUID? is not null and not empty.

---
2. Built-in validators for typical custom data:
    - `ValidIf(eml => eml.IsEmail(), ...` - check the string contains an email.
    - `ValidIf(phnum => phnum.IsPhoneNumber(), ...` - check the string contains a phone number.
    - `ValidIf(cardnum => cardnum.IsCreditCardNumber(), ...` - check the string contains a credit card number.
    - `ValidIf(carddt => carddt.IsCreditCardExpiryDate(), ...` - check the string contains an expiration date for credit card in format `MM/yy`. \
    If credit card is expired, it will also return `false`.
    - `ValidIf(cvv => cvv.IsCreditCardCVV(), ...` - check the string contains a CVV.
    - `ValidIf(uri => uri.IsAbsoluteUri(), ...` - returns `false` if URI value:
        - is not correctly escaped as per URI spec excluding intl UNC name case.
        - is an absolute Uri that represents implicit file Uri `c:\dir\file`.
        - is an absolute Uri that misses a slash before path `file://c:/dir/file`.
        - contains unescaped backslashes even if they will be treated as forward slashes like `http:\\host/path\file` or `file:\\\c:\path`.

---
3. Build-in password helpers:
    - `ValidIf(str => str.IsPassword(), ...` - check password occupancy rate; \
    some additional parameters may be passed to adopt logic:
        - Length of the password must be at least 'minLength' symbols (by default = 8).
        - Password must contain at least the 'minLower' number of the lower case symbols (by default = 1).
        - Password must contain at least the 'minUpper' number of the upper case symbols (by default = 1).
        - Password must contain at least the 'minDigits' number of the digits (by default = 1).
        - Password must contain at least the 'minSpecial' number of the special symbols which may also be provided additionally (none by default).
    - `FlatValidatorFuncs.GetPasswordStrength(string? password)` - calculates the cardinality of the minimal character sets necessary to brute force the password (roughly).\
    Returns `PasswordStrength` as one value of the `VeryWeak, Weak, Medium, Strong, VeryStrong` enum.
    - `FlatValidatorFuncs.GetPasswordStrength(string? password, out int score, out int maxScore)` \
    out param `score` - score for the password, it is always less than maxScore;\
    out param `maxScore` - calculated max score that is possible for this password. \
    Returns `PasswordStrength` as one value of the `VeryWeak, Weak, Medium, Strong, VeryStrong` enum.
    - `FlatValidatorFuncs.GetShannonEntropy(string password)` - this uses the Shannon entropy equation to estimate the average minimum number of bits needed to encode a string of symbols, based on the frequency of the symbols. \
    Returns a `double` value that's Shannon entropy.

---
4. Built-in validators for localization:
    - `ValidIf(str => str.AllCyrillic(), ...` - `true`, if there are only Cyrillic symbols.
    - `ValidIf(str => str.HasCyrillic(), ...` - `true`, if there is at least one Cyrillic symbol.
    - `ValidIf(str => str.AllCyrillicSupplement(), ...` - `true`, if there are only Cyrillic symbols from Cyrillic Supplement that's a Unicode block containing Cyrillic letters for writing several minority languages, including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    - `ValidIf(str => str.AllBasicLatin(), ...` - `true`, if there are only Latin symbols.
    - `ValidIf(str => str.HasBasicLatin(), ...` - `true`, if there is at least one Latin symbols.
---


### 5. Error message format
The error message for each validator can be formatted with checked data that may be filled in when the error message is constructed.

The `ErrorId()` and `ValidIf()` have two possibilities to return some error message:
   - as a simple string - `ErrorIf(eml => eml.IsEmail(), "Invalid email.")`
   - as a formatted string - `ErrorIf(eml => eml.IsEmail(), eml => "Email {eml} is invalid.")`


## Release Notes and Change Log

Release notes [can be found on GitHub](https://github.com/belset/FlatValidator/blob/main/CHANGELOG.md).



## Supporting the project

The `FlatValidator` is developed and supported by [`@belset`](https://github.com/belset) for free in spare time, so that financial help keeps the projects to be going successfully.

 You can sponsor the project via [**`Buy me a coffee`**](https://www.buymeacoffee.com/belset).

<p align="center"><img src="doc/images/logo.png" alt="logo" style="width:10em;height:auto;"></p>

The `FlatValidator` is a validation library for .NET that delivers an high performance and memory prudence by using lambda-based and strongly-typed rules.

## Quick examples

In general, there are two simple ways to validate custom data with the `FlatValidator`.

### 1. Inline mode

> You can define validation rules in your code to validate object locally.

```js
var model = new SomeModel(Email: "email", BirthDate: DateTime.Now, Rate: -100);

// synchronous version
var result =  FlatValidator.Validate(model, v =>
{
    // IsEmail() is one of funcs for typical data formats like Phone, Url, CreditCard, etc.
    v.ValidIf(m => m.Email.IsEmail(), "Invalid email", m => m.Email);

    v.ErrorIf(m => m.Rate < 0, "Negative Rate", m => m.Rate);

    v.WarningIf(m => m.BirthDate.AddYears(10) >= DateTime.Now, 
                "Age looks like incorrect", m => m.BirthDate);
});

// or asynchronous version
var result = await FlatValidator.ValidateAsync(model, v => 
{
    v.ErrorIf(async m => await userService.IsUserExistAsync(m.Email),
              "User already registered", m => m.Email);

    // the same without `async/await`
    v.ErrorIf(m => userService.IsUserExistAsync(m.Email),
              m => $"Email {m.Email} already registered", m => m.Email);
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
`FlatValidator` provides simple built-in validators. 

1. Built-in validators for primitive data:
    - `ErrorIf(str => str.IsEmpty(), ...` - ensure the string is empty.
    - `ValidIf(str => str.NotEmpty(), ...` - ensure the string is not empty.
    - `ErrorIf(guid => guid.IsEmpty(), ...` - ensure the GUID is empty.
    - `ValidIf(guid => guid.NotEmpty(), ...` - ensure the GUID is not empty.
    - `ErrorIf(guid => guid.IsEmpty(), ...` - ensure the GUID? is null or empty.
    - `ValidIf(guid => guid.NotEmpty(), ...` - ensure the GUID? is not null and not empty.

2. Built-in validators for typical data:
    - `ValidIf(str => str.IsEmail(), ...` - check the string contains an email.
    - `ValidIf(str => str.IsPhoneNumber(), ...` - check the string contains a phone number.
    - `ValidIf(str => str.IsCreditCardNumber(), ...` - check the string contains a credit card number.
    - `ValidIf(str => str.IsCreditCardExpiryDate(), ...` - check the string contains an expiration date for credit card in format `MM/yy`. \
    If credit card is expired, it will also return `false`.
    - `ValidIf(str => str.IsCreditCardCVV(), ...` - check the string contains a CVV.
    - `ValidIf(uri => uri.IsAbsoluteUri(), ...` - returns `false` if URI value:
        - is not correctly escaped as per URI spec excluding intl UNC name case.
        - is an absolute Uri that represents implicit file Uri `c:\dir\file`.
        - is an absolute Uri that misses a slash before path `file://c:/dir/file`.
        - contains unescaped backslashes even if they will be treated as forward slashes like `http:\\host/path\file` or `file:\\\c:\path`.

4. Built-in validators for localization:
    - `ValidIf(str => str.AllCyrillic(), ...` - `true`, if there are only Cyrillic symbols.
    - `ValidIf(str => str.HasCyrillic(), ...` - `true`, if there is at least one Cyrillic symbol.
    - `ValidIf(str => str.AllCyrillicSupplement(), ...` - `true`, if there are only Cyrillic symbols from Cyrillic Supplement that's a Unicode block containing Cyrillic letters for writing several minority languages, including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    - `ValidIf(str => str.AllBasicLatin(), ...` - `true`, if there are only Latin symbols.
    - `ValidIf(str => str.HasBasicLatin(), ...` - `true`, if there are only Latin symbols.

### 5. Error message format
The error message for each validator can be formatted with checked data that may be filled in when the error message is constructed.

The `ErrorId()` and `ValidIf()` have two possibilities to return some error message:
   - as a simple string - `ErrorIf(eml => eml.IsEmail(), "Invalid email.")`
   - as a formatted string - `ErrorIf(eml => eml.IsEmail(), eml => "Email {eml} is invalid.")`

## Benchmarks

![With no errors](doc/images/Benchmark_with_NoErrors.png)

![With many errors](doc/images/Benchmark_with_ManyErrors.png)



## Installation
[![Nuget](https://img.shields.io/nuget/v/FlatValidator)](https://www.nuget.org/packages/FlatValidator/)

Install the [FlatValidator](https://www.nuget.org/packages/FlatValidator) from NuGet:
``` console
❯ dotnet add package FlatValidator
```

If installing into an ASP.NET Core project, consider using the [FlatValidator.DependencyInjection](https://www.nuget.org/packages/FlatValidator.DependencyInjection) package that adds extensions specific to ASP.NET Core
``` console
❯ dotnet add package FlatValidator.DependencyInjection
```


## Release Notes and Change Log

Release notes [can be found on GitHub](https://github.com/belset/FlatValidator/blob/main/CHANGELOG.md).



## Supporting the project

If you like my activities, it may be great to give me a ⭐ and/or share this link 🤗

The `FlatValidator` is developed and supported by [@belset](https://github.com/belset) for free in spare time, so that financial help keeps the projects to be going successfully.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png 'Buy me a coffee')](https://www.buymeacoffee.com/belset)

---

The `FlatValidator` is a validation library for .NET that delivers an high performance and memory prudence by using lambda-based and strongly-typed rules.

## Quick examples

In general, there are two simple ways to validate custom data with the `FlatValidator`.

### 1. Inline mode

> You can define validation rules in your code to validate object locally.

```js
var model = new Model(Email: "email", BirthDate: DateTime.Now, Rate: -100);

// validate with _synchronous_ version here
var result = FlatValidator.Validate(model, v =>
{
    // IsEmail() is one of funcs for typical data formats like Phone, Url, CreditCard, etc.
    v.ValidIf(m => m.Email.IsEmail(), 
              m => $"Invalid email: {m.Email}", 
              m => m.Email);

    v.ErrorIf(m => m.Rate < 0, "Negative Rate", m => m.Rate);

    v.WarningIf(m => m.BirthDate.AddYears(10) >= DateTime.Now, 
                "Age looks like incorrect", m => m.BirthDate);
});
if (!result) 
{
    // ToDictionary() => Dictionary<PropertyName, ErrorMessage[]>
    return TypedResults.ValidationProblem(result.ToDictionary()) 
}

// or validate with _asynchronous_ version
var result = await FlatValidator.ValidateAsync(model, v => 
{
    v.ErrorIf(async m => await userService.IsUserExistAsync(m.Email),
              "User already registered", m => m.Email);

    // the same without `async/await`
    v.ErrorIf(m => userService.IsUserExistAsync(m.Email),
              m => $"Email {m.Email} already registered", 
              m => m.Email);
});

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

        ErrorIf(m => m.Forename.IsEmpty(), "Forename can not be empty.", m => m.Forename);
        ErrorIf(m => m.Surname.IsEmpty(), "Surname can not be empty.", m => m.Surname);
        
        // you can define one or more groups of preconditions
        If(m => m.ShipmentAddress.NotEmpty(), @then: m =>
        {
            ValidIf(async m => await postalService.AddressExistsAsync(m.Address),
                     "Postal address not found.", m => m.Address);

            WarningIf(m => !m.Phone.IsPhone(), "No contact phone.");
        },
        @else: m => // optionally
        {
            ValidIf(m => m.Phone.IsPhone(), "invalid phone number.", m => m.Phone);
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

if (!result) // is there any errors?
{
    return result.ToDictionary(); // Dictionary<PropertyName, ErrorMessage[]>
}
```

> **TIP** -
> The package **`FlatValidator.DependencyInjection`** helps you to register all inherited validators in the ServiceCollection automatically.


### 3. Built-in validators
`FlatValidator` provides simple built-in validators. The error message for each validator can contain special placeholders that will be filled in when the error message is constructed.

1. Built-in validators for primitive data:
    - `ErrorIf(str => str.IsEmpty(), ...` - ensure the string is empty.
    - `ValidIf(str => str.NotEmpty(), ...` - ensure the string is not empty.
    - `ErrorIf(guid => guid.IsEmpty(), ...` - ensure the GUID is empty.
    - `ValidIf(guid => guid.NotEmpty(), ...` - ensure the GUID is not empty.
    - `ErrorIf(guid => guid.IsEmpty(), ...` - ensure the GUID? is null or empty.
    - `ValidIf(guid => guid.NotEmpty(), ...` - ensure the GUID? is not null and not empty.

2. Check URI `ValidIf(uri => uri.IsAbsoluteUri(), ...` - returns `false` if URI value:
    - is not correctly escaped as per URI spec excluding intl UNC name case.
    - or is an absolute Uri that represents implicit file Uri `c:\dir\file`.
    - or is an absolute Uri that misses a slash before path `file://c:/dir/file`.
    - or contains unescaped backslashes even if they will be treated as forward slashes like `http:\\host/path\file` or `file:\\\c:\path`.

3. Built-in validators for typical data:
    - `ValidIf(str => str.IsEmail(), ...` - check the string contains an email.
    - `ValidIf(str => str.IsPhoneNumber(), ...` - check the string contains a phone number.
    - `ValidIf(str => str.IsCreditCardNumber(), ...` - check the string contains a credit card number.
    - `ValidIf(str => str.IsCreditCardExpiryDate(), ...` - check the string contains an expiration date for credit card in format `MM/yy`. \
    If credit card is expired, it will also return `false`.
    - `ValidIf(str => str.IsCreditCardCVV(), ...` - check the string contains a CVV.

4. Built-in validators for localization:
    - `ValidIf(str => str.AllCyrillic(), ...` - `true`, if there are only Cyrillic symbols.
    - `ValidIf(str => str.HasCyrillic(), ...` - `true`, if there is at least one Cyrillic symbol.
    - `ValidIf(str => str.AllCyrillicSupplement(), ...` - `true`, if there are only Cyrillic symbols from Cyrillic Supplement that's a Unicode block containing Cyrillic letters for writing several minority languages, including Abkhaz, Kurdish, Komi, Mordvin, Aleut, Azerbaijani, and Jakovlev's Chuvash orthography.
    - `ValidIf(str => str.AllBasicLatin(), ...` - `true`, if there are only Latin symbols.
    - `ValidIf(str => str.HasBasicLatin(), ...` - `true`, if there are only Latin symbols.



## Release Notes and Change Log

Release notes [can be found on GitHub](https://github.com/belset/FlatValidator/blob/main/CHANGELOG.md).



## Supporting the project

The `FlatValidator` is developed and supported by [`@belset`](https://github.com/belset) for free in spare time, so that financial help keeps the projects to be going successfully.

 You can sponsor the project via [**`Buy me a coffee`**](https://www.buymeacoffee.com/belset).

The `FlatValidator` is a validation library for .NET that delivers an high performance and memory prudence by using lambda-based and strongly-typed rules.

## Quick examples

In general, there are two simple ways to validate custom data with the `FlatValidator`.

### 1. Inline mode

> You can define validation rules in your code to validate object locally.

```c#
var model = new Model(Email: "email", BirthDate: DateTime.Now, Rate: -100);

// synchronous version
var result =  FlatValidator.Validate(model, v =>
{
    // IsEmail() is one of funcs for typical data formats like Phone, Url, CreditCard, etc.
    v.ValidIf(m => m.Email.IsEmail(), "Invalid email", m => m.Email);

    v.ErrorIf(m => m.BirthDate.AddYears(15) >= DateTime.Now, 
              m => $"Incorrect age: {m.BirthDate}", m => m.BirthDate);

    v.ErrorIf(m => m.Rate < 0 && m.BirthDate.AddYears(15) >= DateTime.Now, 
             "Rate cannot be negative for age < 15", m => m.Rate);
});
if (!result) 
{
    // ToDictionary() => IDictionary<PropertyName, ErrorMessage[]>
    TypedResults.Problems(result.ToDictionary()) 
}

// or asynchronous version
var result = await FlatValidator.ValidateAsync(model, v => 
{
    v.ErrorIf(async m => await userService.IsUserExistAsync(m.Email),
              "User already registered", m => m.Email);

    // the same without `async/await`
    v.ErrorIf(m => userService.IsUserExistAsync(m.Email),
              m => $"Email {m.Email} already registered", m => m.Email);
});
```

### 2. Inheritance of the `FlatValidator` class

> Another way is to inherit the `FlatValidator` to define custom rules in the constructor. 
Also you can pass dependencies into constructor to get additional functionality inside of the validation rules.

```c#
public record UserModel(string Forename, string Surname, ....);

public class UserValidator: FlatValidator<UserModel> 
{
    public UserValidator(ILogger logger, IPostalService postalService) 
    {
        logger.LogInfo("Validating...");

        ErrorIf(m => m.Forename.IsEmpty(), "Forename can not be empty.", m => m.Forename);
        ValidIf(m => m.Surname.NotEmpty(), "Surname can not be empty.", m => m.Surname);
        
        // you can define one or more groups of preconditions
        Group(m => m.ShipmentAddress.NotEmpty(), m =>
        {
            ValidIf(async m => await postalService.AddressExistsAsync(m.Address),
                     "Postal address not found.", m => m.Address);
        },
        @else: m => // optionally
        {
            ValidIf(m => m.Phone.IsPhone(), "invalid phone number.", m => m.Phone);
        });
    }
}
```
> Now lets validate some object with it
```c#
// now let's validate
var validator = new UserValidator();
var result = validator.Validate(new UserModel(...)); // synchronous call of your UserValidator
var result = await validator.ValidateAsync(customer, cancellationToken); // the same asynchronously
if (!result)
{
    var errors = result.Errors; // result.Errors is a List<PropertyName, error, Tag>
    var dict = result.ToDictionary(); // dict is a Dictionary<PropertyName, ErrorMessage[]>
}

// Inspect any validation failures.
bool success = results.IsValid;
List<ValidationFailure> failures = results.Errors;
```

> **TIP** -
> The package **`FlatValidator.DependencyInjection`** helps you to register all inherited validators in the ServiceCollection automatically.



## Release Notes and Change Log

Release notes [can be found on GitHub](https://github.com/belset/FlatValidator/blob/main/CHANGELOG.md).



## Supporting the project

If you like my activities, it may be great to give me a ⭐ and/or share this link with friends 🤗

The `FlatValidator` is developed and supported by [`@belset`](https://github.com/belset) for free in spare time, so that financial help keeps the projects to be going successfully.

 You can sponsor the project via [**`Buy me a coffee`**](https://www.buymeacoffee.com/belset).

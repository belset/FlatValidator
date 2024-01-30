The `FlatValidator` is a validation library for .NET that delivers an high-performance and memory prudence by using lambda-based and strongly-typed rules.

The `FlatValidator.DependencyInjection` package extends the `FlatValidator` package to register all custom inherited validators in the `IServiceCollection` (Microsoft.Extensions.Dependencyinjection.Abstractions) automatically.

```c#
public static IServiceCollection AddCustomValidators(this IServiceCollection services)
{
    services.AddFlatValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    return services;
}
```

## Quick examples

### 1. Inheritance of the `FlatValidator` class

```c#
public record UserModel(string Phone, string ShipmentAddress, string PostalCode);

public class UserValidator: FlatValidator<UserModel> 
{
    public UserValidator(IPostalService postalService) 
    {
        ErrorIf(m => m.Phone.IsPhoneNumber(), "Invalid phone number.", m => m.Phone);
        
        // define one or more groups for preconditions
        Group(m => m.ShipmentAddress.NotEmpty(), m =>
        {
            ValidIf(m => postalService.AddressExistsAsync(m.ShipmentAddress, m.PostalCode), 
                    "Invalid postal address and/or postal code.", 
                    m => m.ShipmentAddress, m => m.PostalCode);
        });
    }
}

// .... we want a synchronous version to validate here!
var result = new UserValidator().Validate(new UserModel(...)); 

```

### 2. Using `FlatValidator` in inline mode:

```c#
var model = new Model(Email: "email", BirthDate: DateTime.Now, Rate: -100);

// .... now use an asynchronous version!
var result = await FlatValidator.ValidateAsync(model, v => 
{
    // IsEmail() is one of funcs for typical data formats like Phone, Url, CreditCard, etc.
    v.ValidIf(m => m.Email.IsEmail(), "Invalid email", m => m.Email);

    v.ErrorIf(async m => await userService.IsUserExistAsync(m.Email),
              m => $"Email {m.Email} already registered", m => m.Email);
});
if (!result) { ..... }
```
>**Note** -
> You don't need to install the `FlatValidator.DependencyInjection` package for inline mode usage.


## Release Notes and Change Log

Release notes [can be found on GitHub](https://github.com/belset/FlatValidator/blob/main/CHANGELOG.md).


## Supporting the project

The `FlatValidator` is developed and supported by [`@belset`](https://github.com/belset) for free in spare time, so that financial help keeps the projects to be going successfully.

 You can sponsor the project via [**`Buy me a coffee`**](https://www.buymeacoffee.com/belset).



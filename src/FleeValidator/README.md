The `FlatValidator` is a validation library for .NET that uses modern lambda-expression style 
to build strongly-typed validation rules.

### Examples

In general, there are two ways with the `FlatValidator` to validate custom data. Let me show a couple of examples below to give you a perception of how easy it is.

##### Inline mode:

```c#
var model = new Model(Id: -1, Email: "email", BirthDate: DateTime.Now, Rate: -100);

// synchronous version
var result =  FlatValidator.Validate(model, v =>
{
    v.ValidIf(m => m.Id > 0, "Id is incorrect", m => m.Id);
    v.ValidIf(m => m.Email.IsEmail(), "Invalid email", m => m.Email);

    v.ErrorIf(m => m.BirthDate.AddYears(15) >= DateTime.Now, 
              m => $"Incorrect age: {m.BirthDate}", m => m.BirthDate);

    v.ErrorIf(m => m.Rate < 0 && m.BirthDate.AddYears(15) >= DateTime.Now, 
             "Rate cannot be negative for age < 15", m => m.Rate);
});
Assert.False(result.IsValid);
Assert.True(result.Errors.Count == 4);

if (!result) // result.ToDictionary() => IDictionary<PropertyName, ErrorMessage[]>
{
    TypedResults.Problems(result.ToDictionary()) 
}

// or asynchronous version
var result = await FlatValidator.ValidateAsync(model, v => 
{
    v.ErrorIf(m => userService.IsUserExistAsync(m.Email),
              m => $"Email {m.Email} already registered", m => m.Email);

    v.ErrorIf(async m => await userService.IsUserExistAsync(m.Email),
              "User already registered", m => m.Email);
});

```


Another way is to inherit the `FlatValidator` and define some validation rules in the constructor. This is well-known approach to define custom validation functionality. 

As usual you can pass any dependencies into constructor to provide custom functionality inside of the validation rules. 

##### Lets take a look at the example below:

```c#
public class UserValidator: FlatValidator<UserModel> 
{
    public UserValidator(ILogger logger, IUserService userService, IPostalService postalService) 
    {
        ErrorIf(m => m.Forename.IsEmpty(), "Forename can not be empty.", m => m.Forename);
        ValidIf(m => m.Surname.NotEmpty(), "Surname can not be empty.", m => m.Surname);
        
        Group(m => m.ShipmentAddress.NotEmpty(), m =>
        {
            ValidIf(async m => await postalService.AddressExistsAsync(m.Address),
                     "Postal address not found.", m => m.Address);

            Group(m => m.Rate > 1000 && m.Discount > 0, async m => 
            {
                ErrorIf(m => userService.IsEmailBlockedAsync(m.Email, 
                        m => $"User {m.Email} is blocked.", 
                        m => m.Email).Tag("BLOCKED USER");

                await logger.LogError($"Discount for email {m.Email}.");
            },
            @else: m => 
            {
              // you van define groups or rules for 'else' case
            });
        });
    }
}

//.....

var user = new UserModel();
var validator = new UserValidator();

ValidationResult result = validator.Validate(customer);
ValidationResult result = await validator.ValidateAsync(customer, cancellationToken);
if (result)
{ 
    // this is valid... 
}
else
{
    var errors = result.Errors; // List<PropertyName, error, Tag>
    var dict = result.ToDictionary(); // Dictionary<PropertyName, ErrorMessage[]>
}

// Inspect any validation failures.
bool success = results.IsValid;
List<ValidationFailure> failures = results.Errors;
```


### Release Notes and Change Log

Release notes [can be found on GitHub](https://github.com/belset/FlatValidator/releases).

### Supporting the project

Please be informed, if you are going to use the `FlatValidator` in a commercial project, 
we will have an expectation you sponsor the project `FlatValidator` financially.
The `FlatValidator` is developed and supported by [@belset](https://github.com/belset) for 
free in spare time, so that financial help keeps the project to be going successfully.

The project may be sponsored via either [GitHub sponsors](https://github.com/sponsors/belset).

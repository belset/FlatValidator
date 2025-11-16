# Minimal API + CleanArchitecture + CQRS + FlatValidator

It is a sample template that's using an MinimalAPI with the well-known [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) alongside [Minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis) under .NET.

## Prerequisites

The solution prepared to be built on the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), you need to install that before it will start working for you. 

## Features 

There are plenty of handy implementations of features throughout this solution, in no particular order here are some that might interest you.
One of this is the [FlatValidator](https://www.nuget.org/packages/FlatValidator/) of course. 

Let's pay your attention to some moments of the quick example below.

1. Even you faced with the `FlatValidator` first times, the code semantic will be absolutely obvious.
2. You can control the validation flow with help of the `Grouped`.
3. `ErrorIf` and `ValidIf` are two sides of the same coin, just use the most convenient way.
4. It is possibile to initizate a creation of an error without any conditions.
5. Synchronous and Asynchronous are supported completely.
6. You can implement a validation inside of independent class or _inline mode_ may be used.

```js
public class CreateProductValidator : FlatValidator<CreateProductCommand>
{
    public CreateProductValidator(IProductRepository productRepository, IBrandRepository brandRepository)
    {
        ErrorIf(m => m.ProductName.IsEmpty(), "Product name cannot be empty.", m => m.ProductName);
        ErrorIf(m => m.BrandName.IsEmpty(), "Brand name cannot be empty.", m => m.BrandName);

        When(m => brandRepository.BrandExists(m.BrandName), @then: m =>
        {
            ErrorIf(m => productRepository.ProductExists(m.ProductName, m.BrandName), 
                    m => $"Product '{m.ProductName}' for '{m.BrandName}' already exists.", 
                    m => m.ProductName, m => m.BrandName);
        },
        @else: m =>
        {
            Error($"Brand '{m.BrandName}' not found.", m => m.BrandName);
        });

        ValidIf(m => m.Rate < 1 || m.Rate > 5, m => $"Incorrect rate value: {m.Rate}.", m => m.Rate);
    }
}

var validator = new CreateProductValidator();
var result = await validator.ValidateAsync(model, cancellationToken);

```


### Inline mode

You can also define validation rules "in place".

```js

// synchronous version
var result = FlatValidator.Validate(model, v =>
{
    v.ErrorIf(m => m.Id <= 0, "Invalid Id", m => m.Id);
    v.ErrorIf(m => m.DueBy is null, "DueBy can not be null.", m => m.DueBy);
});

// or asynchronous version
var result = await FlatValidator.ValidateAsync(model, v =>
{
    v.ValidIf(m => m.Id > 0, "Invalid Id", m => m.Id);
    v.ValidIf(m => m.DueBy is not null, "DueBy can not be null.", m => m.DueBy);
});

```

## API result with error, typical form

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "BrandName": [
      "Brand 'Morarer LLC' not found."
    ],
    "Rate": [
      "Too small rate value: 0."
    ]
  }
}
```

This solution is loosely based on Clean Architecture patterns.Some structural decisions were made there but it was just to take some things further and scaled back others. Obviously, you can continue in the same manner but it may be modified also. 

There's a little CQRS type stuff going on here. Actually, I can not see any benefits on separating GET/UPDATE stuffs but it was implemented here as it should be.

### Project Structure

The solution divided into 3 functional projects. All serve their own purpose and segregate aspects of the application to allow easier replacement and updating.

1. **Presentation** - Configuring the interaction between the application layer and the consumer. In the project this happens with help of a Minimal API routing. Usual Minimal API's endpoints are used to route execution flow to the layer that owns the domain.
2. **Application** - This project contains a domain and business logic.  The validation of the Commands and Queries also happens here so that and handling of domain entities in their own separated structures.  In accordance of the concept 'Clean Architecture' each domain type has it's own interface to a datasource downstream, this project doesn't care what fulfills this contract, as long as someone does.
3. **Infrastructure** - Here the database comes to be playing.  The project contains the data objects, also it works with the repository interfaces to support CRUD approach to object management under the data source.  Some entity mapping is implemented here, but it is just to allow specific models with attributes to remain in this layer and not bleed through to the **Application** layer.

## Support

If you like this, or want to checkout my other work, please touch me on [LinkedIn](https://www.linkedin.com/in/ilya-rudenka-398877203/) or [GitHub](https://github.com/belset), and consider supporting me by sponsoring the project.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png 'Buy me a coffee')](https://www.buymeacoffee.com/belset)
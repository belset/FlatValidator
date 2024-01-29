using Application.Common;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Entities;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProducts;

using Microsoft.AspNetCore.Http.Extensions;

using Presentation._Startup;
using Presentation.Filters;


namespace Presentation.Endpoints;

public static class ProductEndpoints
{
    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        root.MapGet("/", async ([AsParameters] GetProductsQuery query, IQueryDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch<GetProductsQuery, List<Product>>(query);
            return result.MatchToTypedResult();
        })
        .Produces<List<Product>>()
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("View all Products");

        root.MapGet("/{id:guid}", async ([AsParameters] GetProductByIdQuery query, IQueryDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch<GetProductByIdQuery, Product>(query);
            return result.MatchToTypedResult();
        })
        .AddEndpointFilter<ValidationFilter<GetProductByIdQuery>>()
        .Produces<Product>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesValidationProblem()
        .WithSummary("View a Product by Id");

        root.MapPost("/", async ([AsParameters] CreateProductCommand command, ICommandDispatcher dispatcher, HttpRequest httpRequest) =>
        {
            var result = await dispatcher.Dispatch<CreateProductCommand, Product>(command);
            return result.MatchToTypedResult(p => TypedResults.Created(UriHelper.GetEncodedUrl(httpRequest), p));
        })
        .AddEndpointFilter<ValidationFilter<CreateProductCommand>>()
        .Produces<Product>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesValidationProblem()
        .WithSummary("Create a Product");

        root.MapDelete("/{id:guid}", async ([AsParameters] DeleteProductCommand command, ICommandDispatcher dispatcher, HttpRequest httpRequest) =>
        {
            var result = await dispatcher.Dispatch<DeleteProductCommand, bool>(command);
            return result.MatchToTypedResult(_ => TypedResults.NoContent());
        })
        .AddEndpointFilter<ValidationFilter<DeleteProductCommand>>()
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesValidationProblem()
        .WithSummary("Delete a Product by Id");

        root.MapPut("/{id:guid}", async ([AsParameters] UpdateProductCommand command, ICommandDispatcher dispatcher, HttpRequest httpRequest) =>
        {
            var result = await dispatcher.Dispatch<UpdateProductCommand, bool>(command);
            return result.MatchToTypedResult(p => TypedResults.NoContent());
        })
        .AddEndpointFilter<ValidationFilter<UpdateProductCommand>>()
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesValidationProblem()
        .WithSummary("Update a Product");

        return app;
    }
}

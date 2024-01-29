using Application.Brands.Entities;
using Application.Brands.Queries.GetBrandById;
using Application.Brands.Queries.GetBrands;
using Application.Common;

using Presentation._Startup;
using Presentation.Filters;


namespace Presentation.Endpoints;

public static class BrandEndpoints
{
    public static WebApplication MapBrandEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/brands")
            .WithTags("Brands")
            .WithOpenApi();

        root.MapGet("/", async ([AsParameters] GetBrandsQuery query, IQueryDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch<GetBrandsQuery, List<Brand>>(query);
            return result.MatchToTypedResult();
        })
        .Produces<List<Brand>>()
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("View all brands");

        root.MapGet("/{id:guid}", async ([AsParameters] GetBrandByIdQuery query, IQueryDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch<GetBrandByIdQuery, Brand>(query);
            return result.MatchToTypedResult();
        })
        .AddEndpointFilter<ValidationFilter<GetBrandByIdQuery>>()
        .Produces<Brand>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("View a Brand by their Id");

        return app;
    }
}

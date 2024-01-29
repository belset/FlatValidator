using Application.Common;
using Application.Stores.Entities;
using Application.Stores.Queries.GetStoreById;
using Application.Stores.Queries.GetStores;

using Presentation._Startup;
using Presentation.Filters;


namespace Presentation.Endpoints;

public static class StoreEndpoints
{
    public static WebApplication MapStoreEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/stores")
            .WithTags("Stores")
            .WithOpenApi();

        root.MapGet("/", async ([AsParameters] GetStoresQuery query, IQueryDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch<GetStoresQuery, List<Store>>(query);
            return result.MatchToTypedResult();
        })
        .Produces<List<Store>>()
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("View all stores");

        root.MapGet("/{id:guid}", async ([AsParameters] GetStoreByIdQuery query, IQueryDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch<GetStoreByIdQuery, Store>(query);
            return result.MatchToTypedResult();
        })
        .AddEndpointFilter<ValidationFilter<GetStoreByIdQuery>>()
        .Produces<Store>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesValidationProblem()
        .WithSummary("View a Store by Id");

        return app;
    }
}

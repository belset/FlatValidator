namespace System.Validation;

/// <summary>
/// FlatValidator EndpointFilter for HTTP routers
/// </summary>
public class ValidationFilter<T>(IServiceProvider serviceProvider) : IEndpointFilter
{
    /// <summary>
    /// Some validation logic associated with the filter given a <see cref="EndpointFilterInvocationContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="EndpointFilterInvocationContext"/> associated with the current request/response.</param>
    /// <param name="next">The next filter in the pipeline.</param>
    /// <returns>An awaitable result of calling the handler and apply any modifications made by filters in the pipeline.</returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) is not T model)
        {
            return TypedResults.Problem(
                        detail: "Parameter that's declared for validation is not defined.",
                        statusCode: StatusCodes.Status500InternalServerError);
        }

        var validators = serviceProvider.GetServices<IFlatValidator<T>>();
        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(model, CancellationToken.None);
            if (!result)
            {
                return TypedResults.ValidationProblem(result.ToDictionary());
            }
        }

        // call next handler in the chain
        return await next(context);
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Application.Common;

public interface IQueryHandler<in TQuery, TResult>
{
    ValueTask<QueryResult<TResult>> Handle(TQuery query, CancellationToken cancellation);
}
public interface ICommandHandler<in TCommand, TResult>
{
    ValueTask<CommandResult<TResult>> Handle(TCommand command, CancellationToken cancellation);
}

public interface IQueryDispatcher
{
    ValueTask<QueryResult<TResult>> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellation = default);
}
public interface ICommandDispatcher
{
    ValueTask<CommandResult<TResult>> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellation = default);
}

public class QueryDispatcher : IQueryDispatcher
{
    public IServiceProvider serviceProvider;
    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async ValueTask<QueryResult<TResult>> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellation = default)
    {
        var validator = serviceProvider.GetService<IFlatValidator<TQuery>>();
        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(query, cancellation);
            if (!validationResult)
            {
                return new Exceptions.ValidationException(validationResult.ToDictionary());
            }
        }

        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.Handle(query, cancellation);
    }
}

public record CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async ValueTask<CommandResult<TResult>> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellation = default)
    {
        var validator = serviceProvider.GetService<IFlatValidator<TCommand>>();
        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(command, cancellation);
            if (!validationResult)
            {
                return new Exceptions.ValidationException(validationResult.ToDictionary());
            }
        }

        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.Handle(command, cancellation);
    }
}

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddDispatching(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.Scan(selector =>
        {
            selector.FromAssemblyOf<QueryDispatcher>()
                    .AddClasses(filter =>
                    {
                        filter.AssignableTo(typeof(IQueryHandler<,>));
                    })
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime();
        });
        services.Scan(selector =>
        {
            selector.FromAssemblyOf<QueryDispatcher>()
                    .AddClasses(filter =>
                    {
                        filter.AssignableTo(typeof(ICommandHandler<,>));
                    })
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime();
        });

        return services;
    }
}


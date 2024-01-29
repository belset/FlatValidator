using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Validation;

using Application.Common;

using Microsoft.Extensions.DependencyInjection;


namespace Application._Startup;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDispatching();
        services.AddFlatValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}

using System.Reflection;
using System.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FlatValidator.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlatValidatorsFromAssemblies(
                                        this IServiceCollection services,
                                        IEnumerable<Assembly> assemblies,
                                        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var assignedTypes = assemblies
            .SelectMany(a => a
                .DefinedTypes
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IFlatValidator<>)))
            )
            .ToList();

        foreach (var assignedType in assignedTypes)
        {
            var serviceType = assignedType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IFlatValidator<>));
            services.Add(new ServiceDescriptor(serviceType, assignedType, lifetime));
        }

        return services;
    }

    public static IServiceCollection AddFlatValidatorsFromAssembly(
                                        this IServiceCollection services,
                                        Assembly assembly,
                                        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return AddFlatValidatorsFromAssemblies(services, [assembly], lifetime);
    }
}

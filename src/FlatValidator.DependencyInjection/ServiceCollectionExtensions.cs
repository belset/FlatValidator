using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace System.Validation;

/// <summary>
/// Extensions to register classes in IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all classes which are inherited of the FlatValidator from the assemblies in the IServiceCollection.
    /// </summary>
    /// <param name="services">IServiceCollection in which registration will be created.</param>
    /// <param name="assemblies">List of assemblies which will be examined to find inheritances of the FlatValidator.</param>
    /// <param name="lifetime">Specifies which kind of instance type will be registered.</param>
    /// <returns>The same IServiceCollection</returns>
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

    /// <summary>
    /// Register all classes which are inherited of the FlatValidator from the assembliy in the IServiceCollection.
    /// </summary>
    /// <param name="services">IServiceCollection in which registration will be created.</param>
    /// <param name="assembly">Assembly which will be examined to find inheritances of the FlatValidator.</param>
    /// <param name="lifetime">Specifies which kind of instance type will be registered.</param>
    /// <returns>The same IServiceCollection</returns>
    public static IServiceCollection AddFlatValidatorsFromAssembly(
                                        this IServiceCollection services,
                                        Assembly assembly,
                                        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return AddFlatValidatorsFromAssemblies(services, [assembly], lifetime);
    }
}

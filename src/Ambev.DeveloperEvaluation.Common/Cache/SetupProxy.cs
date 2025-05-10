using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// SetupProxy is a static class that provides methods to set up proxy caching.
/// </summary>
public static class SetupProxy
{
    /// <summary>
    /// AddProxyCache is an extension method for IServiceCollection that registers proxy caching services.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddProxyCache(this IServiceCollection services)
    {
        var cachePolicyAttr = typeof(CachePolicyAttribute);

        var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(i => i.GetTypes());

        var interfacesToRegister = allTypes.Where(type => type.IsInterface && type.GetMethods().Any(method => method.GetCustomAttributes(cachePolicyAttr, false).Length != 0));

        foreach (var interfaceType in interfacesToRegister)
        {
            var implementationType = allTypes.FirstOrDefault(type => interfaceType.IsAssignableFrom(type) && type.IsClass)
                ?? throw new InvalidOperationException($"No implementation found for {interfaceType.Name}.");

            services.AddScoped(implementationType);

            services.AddSingleton(interfaceType, serviceProvider =>
            {
                var cacheProvider = serviceProvider.GetRequiredService<IDistributedCache>();
                var implementationInstance = serviceProvider.GetRequiredService(implementationType);

                var proxy = typeof(CacheDispatchProxy)
                    .GetMethod(nameof(CacheDispatchProxy.GetProxy))?
                    .MakeGenericMethod(interfaceType)
                    .Invoke(null, [implementationInstance, cacheProvider]);

                return proxy ?? throw new InvalidOperationException($"Failed to create proxy for {implementationType.Name}.");
            });
        }

        return services;
    }
}

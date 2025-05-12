using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Infra.Events;

namespace Ambev.DeveloperEvaluation.Infra;

public static class InfraSetup
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBroker, MQTTSevices>();

        return services;
    }
}

using Microsoft.AspNetCore.Builder;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Common.Redis;
using Ambev.DeveloperEvaluation.Common.Cache;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddProxyCache();
        builder.Services.AddORM(builder.Configuration);
        builder.Services.AddRedis(builder.Configuration);
    }
}
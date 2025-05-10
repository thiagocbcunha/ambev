using Microsoft.AspNetCore.Builder;
using Ambev.DeveloperEvaluation.Application;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddApplication(builder.Configuration);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM;

public static class ORMSetup
{
    public static IServiceCollection AddORM(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();

        services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());

        services.AddDbContext<DefaultContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ORMSetup).Namespace)));       

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var listRepositories = typeof(ORMSetup).Assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository"))
            .ToList();

        foreach (var repository in listRepositories)
        {
            if(repository.GetInterfaces().FirstOrDefault(serv => serv.Name.EndsWith("Repository")) is not Type @interface)
                services.AddScoped(repository);

            else
                services.AddScoped(@interface, repository);
        }

        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}

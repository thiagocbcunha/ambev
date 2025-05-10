using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Common.Redis;

public static class RedisSetup
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()
            ?? throw new ArgumentNullException("RedisSettings is empty");

        redisSettings.UserName = Environment.GetEnvironmentVariable("REDIS_USERNAME") ?? redisSettings.UserName;
        redisSettings.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD") ?? redisSettings.Password;

        services.AddStackExchangeRedisCache(options =>
        {            
            options.ConfigurationOptions = ConfigurationOptions.Parse(redisSettings.Endpoints);

            options.ConfigurationOptions.ClientName = redisSettings.ClientName;
            options.ConfigurationOptions.Ssl = redisSettings.SslEnable;
            options.ConfigurationOptions.User = redisSettings.UserName;
            options.ConfigurationOptions.Password = redisSettings.Password;
        });
        return services;
    }
}

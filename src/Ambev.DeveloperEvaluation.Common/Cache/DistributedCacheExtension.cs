using System.Text.Json;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;

namespace Ambev.DeveloperEvaluation.Common.Cache;

internal static class DistributedCacheExtension
{
    private static Parser _parser = new();
    private static readonly ConcurrentDictionary<Type, Delegate> _cacheGetDelegates = [];

    internal static dynamic? Get(this IDistributedCache cache, string key, Type returnType)
    {
        if (typeof(Task).IsAssignableFrom(returnType))
            returnType = returnType.IsGenericType ? returnType.GetGenericArguments()[0] : typeof(object);

        if (!_cacheGetDelegates.TryGetValue(returnType, out Delegate? deleg))
        { 
            
            var method = _parser.GetType().GetMethod("GetAndParseType")?.MakeGenericMethod(returnType);
            if (method is not null)
            {
                var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var delegateType = Expression.GetDelegateType([.. parameters, returnType]);
                var del = method.CreateDelegate(delegateType, _parser);

                _cacheGetDelegates[returnType] = del;
            }
        }

        var value = cache.GetString(key);

        return deleg?.DynamicInvoke(value);
    }

    public static void Set(this IDistributedCache cache, string key, object value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
        };

        var json = JsonSerializer.Serialize(value);
        cache.SetString(key, json, options);
    }

    private class Parser
    {
        public T? GetAndParseType<T>(string value)
            => value is null ? default : JsonSerializer.Deserialize<T>(value);
    }
}

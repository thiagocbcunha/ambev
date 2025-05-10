using System.Text;
using System.Text.Json;
using System.Reflection;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;

namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// CacheDispatchProxy is a dynamic proxy class that intercepts method calls to cache the results.
/// </summary>
/// <typeparam name="TService"></typeparam>
public class CacheDispatchProxy : DispatchProxy
{
    /// <summary>
    /// Set the instance and cache provider.
    /// </summary>
    /// <param name="CachePolicy"></param>
    /// <param name="Delegate"></param>
    private record DelegateInfo(CachePolicyAttribute? CachePolicy, Delegate Delegate);

    private object? _instance;
    private IDistributedCache? _cache;
    private readonly ConcurrentDictionary<MethodInfo, DelegateInfo> _methodDelegates = [];

    /// <summary>
    /// Proxy method to intercept calls to the target instance.
    /// </summary>
    /// <param name="method"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected override object? Invoke(MethodInfo method, object[] args)
    {
        DelegateInfo delegateInfo = GetOrCreateDelegate(method);

        if (delegateInfo.Delegate is null)
            throw new InvalidOperationException($"Delegate for method {method.Name} is null.");

        if (delegateInfo.CachePolicy is null)
            return delegateInfo.Delegate.DynamicInvoke(args);

        string key = $"{method.Name}:{GetUniqueKey(args)}";

        var value = _cache?.Get(key, method.ReturnType);

        if (value is not null)
            return IsAsyncMethod(method) ? Task.FromResult(value) : value;

        var dynamicResult = delegateInfo.Delegate.DynamicInvoke(args);
        var result = HandleAsyncResult(dynamicResult);

        if (result is not null)
            _cache?.Set(key, result, delegateInfo.CachePolicy.DueDate);

        return dynamicResult ?? throw new InvalidOperationException($"Method {method.Name} returned null.");
    }

    /// <summary>
    /// Get or create a delegate for the method.
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    private DelegateInfo GetOrCreateDelegate(MethodInfo method)
    {
        if (!_methodDelegates.TryGetValue(method, out var delegateInfo))
        {
            CachePolicyAttribute? attr = method.GetCustomAttribute<CachePolicyAttribute>();

            var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
            var delegateType = Expression.GetDelegateType([.. parameters, method.ReturnType]);
            var del = method.CreateDelegate(delegateType, _instance);

            delegateInfo = new(attr, del);

            _methodDelegates[method] = delegateInfo;
        }

        return delegateInfo;
    }

    /// <summary>
    /// Check if the method is asynchronous.
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    private static bool IsAsyncMethod(MethodInfo method)
        => typeof(Task).IsAssignableFrom(method.ReturnType);

    /// <summary>
    /// Generate a unique key based on the method arguments.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static string GetUniqueKey(object[] args)
    {
        var serializedData = string.Join("|", args.Select(Serializer));
        var dataBytes = Encoding.UTF8.GetBytes(serializedData);
        var hashBytes = SHA256.HashData(dataBytes);
        var hashString = new StringBuilder();

        foreach (byte b in hashBytes)
            hashString.Append(b.ToString("x2"));

        return hashString.ToString();
    }

    /// <summary>
    /// Serialize the object to a string.
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    private static string Serializer(object arg)
    {
        try { return JsonSerializer.Serialize(arg); } catch { return ""; }
    }

    /// <summary>
    /// Handle the result of the method call.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private static object? HandleAsyncResult(dynamic? result)
    {
        if (result is Task task)
        {
            task.Wait();
            if (task.GetType().IsGenericType)
                return ((dynamic)task).Result;

            return null;
        }

        return result;
    }

    /// <summary>
    /// Creates a proxy for the specified instance.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="instance"></param>
    /// <param name="cacheProvider"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TService GetProxy<TService>(TService instance, IDistributedCache distributedCache)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(distributedCache);

        if (Create<TService, CacheDispatchProxy>() is not TService proxy)
            throw new InvalidOperationException($"Failed to create proxy for {typeof(TService).Name}.");

        ((CacheDispatchProxy)(object)proxy)._instance = instance;
        ((CacheDispatchProxy)(object)proxy)._cache = distributedCache;

        return proxy;
    }
}
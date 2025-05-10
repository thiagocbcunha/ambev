namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// CachePolicyAttribute is an attribute that specifies the cache duration for a method.
/// </summary>
/// <param name="durationInSeconds"></param>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class CachePolicyAttribute(int durationInSeconds) : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CachePolicyAttribute"/> class.
    /// </summary>
    internal TimeSpan DueDate { get; init; } = new TimeSpan(0, 0, durationInSeconds);
}
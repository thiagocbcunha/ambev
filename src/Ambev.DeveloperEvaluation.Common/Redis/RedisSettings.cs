namespace Ambev.DeveloperEvaluation.Common.Redis;

internal class RedisSettings
{
    public bool SslEnable { get; set; }
    public required string ClientName { get; set; }
    public required string Endpoints { get; set; }
    public string? UserName { get; internal set; }
    public string? Password { get; internal set; }
}

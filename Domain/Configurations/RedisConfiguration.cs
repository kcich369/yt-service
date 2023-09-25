using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class RedisConfiguration : IConfiguration
{
    public string ConnectionPort { get; set; }
    public int ExpirationMinutes { get; set; }
}
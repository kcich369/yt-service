using Domain.Configurations;
using StackExchange.Redis;

namespace Infrastructure.Factories;

public class RedisFactory : IRedisFactory
{
    private ConnectionMultiplexer _connection;
    private readonly RedisConfiguration _configuration;

    public RedisFactory(RedisConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ConnectionMultiplexer Connect() =>
        _connection ??= ConnectionMultiplexer.Connect(_configuration.ConnectionPort);

    public IDatabase GetDatabase() => Connect().GetDatabase();
}
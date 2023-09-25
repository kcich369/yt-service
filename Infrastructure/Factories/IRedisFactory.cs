using StackExchange.Redis;

namespace Infrastructure.Factories;

public interface IRedisFactory
{
    ConnectionMultiplexer Connect();
    
    IDatabase GetDatabase();
}
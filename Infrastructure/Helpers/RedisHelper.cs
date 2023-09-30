using System.Text.Json;
using Domain.Helpers;
using StackExchange.Redis;

namespace Infrastructure.Helpers;

public sealed class RedisHelper : IRedisHelper
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisHelper(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<T> GetData<T>(string key)
    {
        var value = await _connectionMultiplexer.GetDatabase().StringGetAsync(key);
        return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task<bool> SetData<T>(string key, T data, TimeSpan expiry) =>
        await _connectionMultiplexer.GetDatabase()
            .StringSetAsync(key, JsonSerializer.Serialize<T>(data), expiry);

    public async Task<bool> Exist(string key)=>
         await _connectionMultiplexer.GetDatabase().KeyExistsAsync(key);

    public async Task<bool> Remove(string key)=>
        await _connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
}
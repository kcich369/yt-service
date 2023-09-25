using System.Text.Json;
using Domain.Helpers;
using Infrastructure.Factories;

namespace Infrastructure.Helpers;

public sealed class RedisHelper : IRedisHelper
{
    private readonly IRedisFactory _redisFactory;

    public RedisHelper(IRedisFactory redisFactory)
    {
        _redisFactory = redisFactory;
    }

    public async Task<T> GetData<T>(string key)
    {
        var value = await _redisFactory.GetDatabase().StringGetAsync(key);
        return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task<bool> SetData<T>(string key, T data, TimeSpan expiry) =>
        await _redisFactory.GetDatabase()
            .StringSetAsync(key, JsonSerializer.Serialize<T>(data), expiry);

    public async Task<bool> Exist(string key) =>
        await _redisFactory.GetDatabase().KeyExistsAsync(key);
}
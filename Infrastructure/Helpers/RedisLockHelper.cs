using Domain.Helpers;
using Infrastructure.Factories;
using Medallion.Threading.Redis;

namespace Infrastructure.Helpers;

public sealed class RedisLockHelper : IRedisLockHelper
{
    private readonly IRedisFactory _redisFactory;
    private readonly IRedisHelper _redisHelper;
    private readonly RedisDistributedLock _lock;
    private const string RedisLock = nameof(RedisLock);


    public RedisLockHelper(IRedisFactory redisFactory,
        IRedisHelper redisHelper)
    {
        _redisFactory = redisFactory;
        _redisHelper = redisHelper;
        _lock = new RedisDistributedLock(RedisLock, _redisFactory.GetDatabase());
    }

    public async Task<T> GetData<T>(string key)
    {
        await using var handle = await _lock.TryAcquireAsync();
        if (handle == null)
            throw new Exception("Redis lock exception");
        return await _redisHelper.GetData<T>(key);
    }

    public async Task<bool> SetData<T>(string key, T data, TimeSpan expiry)
    {
        await using var handle = await _lock.TryAcquireAsync();
        if (handle == null)
            throw new Exception("Redis lock exception");
        return await _redisHelper.SetData<T>(key, data, expiry);
    }

    public async Task<bool> Exist(string key)
    {
        await using var handle = await _lock.TryAcquireAsync();
        if (handle == null)
            throw new Exception("Redis lock exception");
        return await _redisHelper.Exist(key);
    }
}
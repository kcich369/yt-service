using Domain.Configurations;
using Domain.EntityIds.Base;
using Domain.Helpers;
using Domain.Messages;
using Domain.Providers;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Helpers;

public sealed class MessageHelper : IMessageHelper
{
    private readonly IRedisLockHelper _redisLockHelper;
    private readonly RedisConfiguration _configuration;
    private readonly ICacheKeysProvider _cacheKeysProvider;
    private readonly ILogger<MessageHelper> _logger;

    public MessageHelper(IRedisLockHelper redisLockHelper,
        RedisConfiguration configuration,
        ICacheKeysProvider cacheKeysProvider,
        ILogger<MessageHelper> logger)
    {
        _redisLockHelper = redisLockHelper;
        _configuration = configuration;
        _cacheKeysProvider = cacheKeysProvider;
        _logger = logger;
    }

    public async Task<bool> Delivered<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId
    {
        var key = _cacheKeysProvider.GetKey(message);
        if (!await _redisLockHelper.Exist(key))
            return !await _redisLockHelper.SetData(key, message.Id,
                TimeSpan.FromMinutes(_configuration.ExpirationMinutes));
        _logger.LogError(
            "Message is processed! Message name: {Name}, id name: {EntityIdName}, id value {MessageId}",
            message.GetType().Name, message.Id.GetType().Name, message.Id);
        return true;
    }

    public async Task<bool> PreviousProcessIsFinished<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId
    {
        var key = _cacheKeysProvider.GetPreviousMessageKey(message);
        if (await _redisLockHelper.Exist(key))
            return await _redisLockHelper.Remove(key);
        _logger.LogError("Can not clear previous lock because given key: {Key} does not exist", key);
        return false;
    }
}
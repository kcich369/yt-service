using Domain.Configurations;
using Domain.EntityIds.Base;
using Domain.Helpers;
using Domain.Messages;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Helpers;

public sealed class MessageHelper : IMessageHelper
{
    private readonly IRedisLockHelper _redisLockHelper;
    private readonly RedisConfiguration _configuration;
    private readonly ILogger<MessageHelper> _logger;

    public MessageHelper(IRedisLockHelper redisLockHelper,
        RedisConfiguration configuration,
        ILogger<MessageHelper> logger)
    {
        _redisLockHelper = redisLockHelper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> MessageIsProcessing<T>(MessageBase<T> message) where T : EntityId
    {
        var key = $"{typeof(MessageBase<T>).Name}-{message.Id}";
        if (await _redisLockHelper.Exist(key))
        {
            _logger.LogError(
                "Message is processed! Message name: {Name}, id name: {EntityIdName}, id value {MessageId}",
                typeof(MessageBase<T>).Name, nameof(T), message.Id);
            return true;
        }

        await _redisLockHelper.SetData(key, message.Id,
            TimeSpan.FromMinutes(_configuration.ExpirationMinutes));
        return false;
    }
}
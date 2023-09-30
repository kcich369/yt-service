using Domain.Entities;
using Hangfire;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class ChannelCreatedRetryingJob : IChannelCreatedRetryingJob
{
    private readonly IAppDbContext _dbContext;
    private readonly IMessagePublisher _messagePublisher;

    public ChannelCreatedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute()
    {
        var channels = await _dbContext.Set<YtChannel>()
            .Select(x => new ChannelCreated(x.Id))
            .ToListAsync();
        await _messagePublisher.Send(channels);
    }
}
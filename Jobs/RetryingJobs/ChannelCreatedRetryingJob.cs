using Domain.Entities;
using Hangfire;
using Infrastructure.Services.Base;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class ChannelCreatedRetryingJob : MessagePublisherService<ChannelCreated>, IChannelCreatedRetryingJob
{
    private readonly IAppDbContext _dbContext;

    public ChannelCreatedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher publisher) : base(publisher)
    {
        _dbContext = dbContext;
    }

    public async Task Execute()
    {
        var channels = await _dbContext.Set<YtChannel>()
            .Include(x=>x.Videos)
            .Where(x=>!x.Videos.Any())
            .Select(x => new ChannelCreated(x.Id))
            .ToListAsync();
        await Publish(channels);
    }
}
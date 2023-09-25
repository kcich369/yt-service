using Domain.Entities;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Hangfire;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class NewVideoCreatedRetryingJob : INewVideoCreatedRetryingJob
{
    private readonly IAppDbContext _dbContext;
    private readonly IMessagePublisher _messagePublisher;

    public NewVideoCreatedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute()
    {
        var filesCount = Enumeration.GetAll<VideoQualityEnum>().Count();
        var videos = await _dbContext.Set<YtVideo>()
            .Where(x => x.Process && x.Files.Count() < filesCount)
            .Select(x => new NewVideoCreated(x.Id))
            .ToListAsync();
        await _messagePublisher.Send(videos);
    }
}
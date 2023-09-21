using Domain.Entities;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Hangfire;
using Infrastructure.Services.Base;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class NewVideoCreatedRetryingJob : MessagePublisherService<NewVideoCreated>, INewVideoCreatedRetryingJob
{
    private readonly IAppDbContext _dbContext;

    public NewVideoCreatedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher) : base(messagePublisher)
    {
        _dbContext = dbContext;
    }

    public async Task Execute()
    {
        var filesCount = Enumeration.GetAll<VideoQualityEnum>().Count();
        var videos = await _dbContext.Set<YtVideo>()
            .Where(x => x.Process && x.Files.Count() < filesCount)
            .Select(x => new NewVideoCreated(x.Id))
            .ToListAsync();
        await Publish(videos);
    }
}
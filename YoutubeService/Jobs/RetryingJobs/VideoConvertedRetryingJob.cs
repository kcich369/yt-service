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
internal sealed class VideoConvertedRetryingJob : MessagePublisherService<VideoConverted>, IVideoConvertedRetryingJob
{
    private readonly IAppDbContext _dbContext;

    public VideoConvertedRetryingJob(IAppDbContext dbContext, IMessagePublisher messagePublisher) : base(messagePublisher)
    {
        _dbContext = dbContext;
    }

    public async Task Execute()
    {
        var videos = await _dbContext.Set<YtVideoFileWav>()
            .Where(x =>x.Process && x.Language == null)
            .Select(x =>new VideoConverted(x.Id))
            .ToListAsync();

        await Publish(videos);
    }
}
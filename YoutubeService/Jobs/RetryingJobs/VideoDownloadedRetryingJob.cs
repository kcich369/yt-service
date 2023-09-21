using Domain.Entities;
using Domain.Enumerations;
using Hangfire;
using Infrastructure.Services.Base;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class VideoDownloadedRetryingJob : MessagePublisherService<VideoDownloaded>, IVideoDownloadedRetryingJob
{
    private readonly IAppDbContext _dbContext;

    public VideoDownloadedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher) : base(messagePublisher)
    {
        _dbContext = dbContext;
    }

    public async Task Execute()
    {
        var convertVideos = await _dbContext.Set<YtVideoFile>()
            .Include(x => x.Video)
            .Where(x => x.Process && x.Quality == VideoQualityEnum.High.Name)
            .Select(x => new VideoDownloaded(x.Id))
            .ToListAsync();
        await Publish(convertVideos);
    }
}
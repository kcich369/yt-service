using Domain.Entities;
using Domain.Enumerations;
using Hangfire;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class VideoDownloadedRetryingJob : IVideoDownloadedRetryingJob
{
    private readonly IAppDbContext _dbContext;
    private readonly IMessagePublisher _messagePublisher;

    public VideoDownloadedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute()
    {
        var convertVideos = await _dbContext.Set<YtVideoFile>()
            .Include(x => x.Video)
            .Where(x => x.Process && x.Quality == VideoQualityEnum.High)
            .Select(x => new VideoDownloaded(x.Id, null))
            .ToListAsync();
        await _messagePublisher.Send(convertVideos);
    }
}
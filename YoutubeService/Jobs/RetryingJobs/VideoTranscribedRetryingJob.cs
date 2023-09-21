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
internal sealed class VideoTranscribedRetryingJob : MessagePublisherService<VideoTranscribed>, IVideoTranscribedRetryingJob
{
    private readonly IAppDbContext _dbContext;

    public VideoTranscribedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher) : base(messagePublisher)
    {
        _dbContext = dbContext;
    }

    public async Task Execute()
    {
        var transcribeVideos = await _dbContext.Set<YtVideoTranscription>()
            .Include(x => x.Description)
            .Include(x => x.Tag)
            .Where(x => x.Process && x.Description == null || x.Tag == null)
            .Select(x => new VideoTranscribed(x.Id))
            .ToListAsync();
        await Publish(transcribeVideos);
    }
}
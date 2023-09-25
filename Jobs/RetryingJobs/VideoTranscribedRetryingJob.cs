using Domain.Entities;
using Hangfire;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class VideoTranscribedRetryingJob : IVideoTranscribedRetryingJob
{
    private readonly IAppDbContext _dbContext;
    private readonly IMessagePublisher _messagePublisher;

    public VideoTranscribedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute()
    {
        var transcribeVideos = await _dbContext.Set<YtVideoTranscription>()
            .Include(x => x.Description)
            .Include(x => x.Tag)
            .Where(x => x.Process && x.Description == null || x.Tag == null)
            .Select(x => new VideoTranscribed(x.Id))
            .ToListAsync();
        await _messagePublisher.Send(transcribeVideos);
    }
}
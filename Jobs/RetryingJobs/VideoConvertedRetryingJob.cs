using Domain.Entities;
using Hangfire;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class VideoConvertedRetryingJob : IVideoConvertedRetryingJob
{
    private readonly IAppDbContext _dbContext;
    private readonly IMessagePublisher _messagePublisher;

    public VideoConvertedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute()
    {
        var videos = await _dbContext.Set<YtVideoFileWav>()
            .Where(x =>x.Process && x.Language == null)
            .Select(x =>new VideoConverted(x.Id))
            .ToListAsync();

        await _messagePublisher.Send(videos);
    }
}
using Domain.Entities;
using Hangfire;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Jobs.RetryingJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
internal sealed class LanguageRecognisedRetryingJob : ILanguageRecognisedRetryingJob
{
    private readonly IAppDbContext _dbContext;
    private readonly IMessagePublisher _messagePublisher;

    public LanguageRecognisedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext;
        _messagePublisher = messagePublisher;
    }

    public async Task Execute()
    {
        var transcribeVideos = await _dbContext.Set<YtVideoFileWav>()
            .Include(x => x.YtVideoTranscription)
            .Where(x => x.Process && x.YtVideoTranscription == null)
            .Select(x => new LanguageRecognised(x.Id,null))
            .ToListAsync();

        await _messagePublisher.Send(transcribeVideos);
    }
}
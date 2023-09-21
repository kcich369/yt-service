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
internal sealed class LanguageRecognisedRetryingJob : MessagePublisherService<LanguageRecognised>, ILanguageRecognisedRetryingJob
{
    private readonly IAppDbContext _dbContext;

    public LanguageRecognisedRetryingJob(IAppDbContext dbContext,
        IMessagePublisher messagePublisher) : base(messagePublisher)
    {
        _dbContext = dbContext;
    }

    public async Task Execute()
    {
        var transcribeVideos = await _dbContext.Set<YtVideoFileWav>()
            .Include(x => x.YtVideoTranscription)
            .Where(x => x.Process && x.YtVideoTranscription == null)
            .Select(x => new LanguageRecognised(x.Id))
            .ToListAsync();
        
        await Publish(transcribeVideos);
    }
}
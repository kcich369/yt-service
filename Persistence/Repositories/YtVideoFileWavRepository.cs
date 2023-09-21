using Domain.Entities;
using Domain.EntityIds;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public sealed class YtVideoFileWavRepository : IYtVideoFileWavRepository
{
    private readonly IAppDbContext _appDbContext;

    public YtVideoFileWavRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<YtVideoFileWav> GetToTranscription(YtVideoFileWavId videoFileWavId, CancellationToken token) =>
        _appDbContext.Set<YtVideoFileWav>().Include(x => x.YtVideoTranscription)
            .FirstOrDefaultAsync(x => x.Id == videoFileWavId, token);

    public Task<YtVideoFileWav>
        GetToLanguageRecognition(YtVideoFileWavId videoFileWavId, CancellationToken token) =>
        _appDbContext.Set<YtVideoFileWav>()
            .FirstOrDefaultAsync(x => x.Language == null, token);
}
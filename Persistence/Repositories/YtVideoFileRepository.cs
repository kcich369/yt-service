using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Providers;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Specifications.Base;
using Persistence.Specifications.YtVideoFile;

namespace Persistence.Repositories;

public sealed class YtVideoFileRepository : IYtVideoFileRepository
{
    private readonly IAppDbContext _dbContext;
    private readonly IDateProvider _dateProvider;

    public YtVideoFileRepository(IAppDbContext dbContext,
        IDateProvider dateProvider)
    {
        _dbContext = dbContext;
        _dateProvider = dateProvider;
    }

    public async Task<YtVideoFile> GetByIdToDownload(YtVideoFileId id, CancellationToken token) =>
        await _dbContext.Set<YtVideoFile>()
            .Include(x => x.Video)
            .FirstOrDefaultAsync(x => x.Id == id, token);

    public async Task<IEnumerable<YtVideoFileId>> GetAllToDownload(int amount, CancellationToken token)
    {
        var newFiles = await _dbContext.Set<YtVideoFile>()
            .ApplySelectedSpecification(new GetNewYtVideFilesToDownloadSpecification(amount))
            .ToListAsync(token);

        var retries = await _dbContext.Set<YtVideoFile>()
            .ApplySelectedSpecification(
                new GetYtVideFilesAfterRetryToDownloadSpecification(6, _dateProvider.DateTimeNow(), amount))
            .ToListAsync(token);

        return newFiles.Concat(retries);
    }

    public async Task<YtVideoFile> GetById(YtVideoFileId ytVideoFileId, CancellationToken token) =>
        await _dbContext.Set<YtVideoFile>().FirstOrDefaultAsync(x => x.Id == ytVideoFileId, token);
}
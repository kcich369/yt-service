using Domain.Dtos.YtVideo;
using Domain.Dtos.YtVideoFile;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.QueryableExtensions;
using Persistence.Specifications.Base;
using Persistence.Specifications.YtVideos;

namespace Persistence.Repositories;

public sealed class YtVideoRepository : IYtVideoRepository
{
    private readonly IAppDbContext _dbContext;

    public YtVideoRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<YtVideoSearchDto>> SearchByQuery(string query, int take, CancellationToken token) =>
        await _dbContext.Set<YtVideo>()
            .ApplySelectedSpecification(new SearchVideosByNameSelectedSpecification(query, take))
            .ToListAsync(token);

    public async Task<YtVideoDetailsDto> GetById(YtVideoId id, CancellationToken token) => await _dbContext
        .Set<YtVideo>()
        .ApplySelectedSpecification(new GetByIdSelectedSpecification(id))
        .FirstOrDefaultAsync(token);

    public async Task<YtVideo> GetForDownloading(YtVideoId id, CancellationToken token) => await _dbContext
        .Set<YtVideo>()
        .Include(x => x.Channel)
        .Include(x => x.Files)
        .FirstOrDefaultAsync(x => x.Id == id, token);
}
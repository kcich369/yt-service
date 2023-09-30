using Domain.Entities;
using Domain.Repositories;
using Persistence.Context;

namespace Persistence.Repositories;

public sealed class YtVideoTagsRepository : IYtVideoTagsRepository
{
    private readonly IAppDbContext _dbContext;

    public YtVideoTagsRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(YtVideoTag ytVideoTag, CancellationToken token) =>
        await _dbContext.Set<YtVideoTag>().AddAsync(ytVideoTag, token);
}
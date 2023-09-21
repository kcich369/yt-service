using Domain.Entities;
using Domain.Repositories;
using Persistence.Context;

namespace Persistence.Repositories;

public sealed class YtVideoDescriptionRepository : IYtVideoDescriptionRepository
{
    private readonly IAppDbContext _dbContext;

    public YtVideoDescriptionRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(YtVideoDescription ytVideoDescription, CancellationToken token) =>
        await _dbContext.Set<YtVideoDescription>().AddAsync(ytVideoDescription, token);
}
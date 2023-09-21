using Domain.UnitOfWork;
using Persistence.Context;

namespace Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAppDbContext _dbContext;

    public UnitOfWork(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken token) => _dbContext.SaveChangesAsync(token);
}
namespace Domain.UnitOfWork;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token);
}
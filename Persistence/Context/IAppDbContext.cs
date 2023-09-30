using Domain.Entities.Base;
using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public interface IAppDbContext : IUnitOfWork
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;
}
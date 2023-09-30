using Domain.Entities;
using Domain.Entities.Base;
using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    // public DbSet<YtChannel> YtChannels { get; set; }

    // public DbSet<YtVideo> YtVideos { get; set; }
    // public DbSet<YtVideoFile> YtVideoFiles { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken token = default) => base.SaveChangesAsync(token);

    public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity => base.Set<TEntity>();
}
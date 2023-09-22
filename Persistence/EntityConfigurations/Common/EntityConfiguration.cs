using Domain.Entities.Base;
using Domain.EntityIds;
using Domain.EntityIds.Base;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common.Converters;
using Persistence.EntityConfigurations.Common.Properties;

namespace Persistence.EntityConfigurations.Common;

public abstract class EntityConfiguration<T, TId> : IEntityTypeConfiguration<T>
    where T : Entity<TId> where TId : EntityId
{
    private readonly string _tableName;

    protected EntityConfiguration(string tableName)
    {
        _tableName = tableName;
    }

    public void Configure(EntityTypeBuilder<T> builder)
    {
        if (_tableName is null)
            throw new InvalidTableNameException("Table name can not be null");

        builder.ToTable(_tableName);
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => !x.Deleted);
        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.SetAuditablePropertiesDbConstraints();
        builder.SetConverters();
        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}
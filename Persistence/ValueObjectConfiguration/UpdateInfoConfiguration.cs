using Domain.Entities.Base;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityIdConverters;

namespace Persistence.ValueObjectConfiguration;

public static class UpdateInfoConfiguration
{
    public static OwnedNavigationBuilder<TEntity, TValueObject> ConfigureUpdateInfo<TEntity, TValueObject>(
        this OwnedNavigationBuilder<TEntity, TValueObject> ow)
        where TEntity : class, IEntity where TValueObject : UpdateInfo
    {
        ow.Property(x => x.UpdatedAt)
            .HasColumnName(nameof(UpdateInfo.UpdatedAt));
        ow.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(100)
            .HasColumnName(nameof(UpdateInfo.UpdatedBy));
        ow.Property(x => x.UpdatedById).IsRequired()
            .HasColumnName(nameof(UpdateInfo.UpdatedById));
        ow.Property(x=>x.UpdatedById).HasConversion(new UserIdConverter());
        return ow;
    }
}
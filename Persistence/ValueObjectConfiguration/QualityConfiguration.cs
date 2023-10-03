using Domain.Entities.Base;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ValueObjectConfiguration;

public static class QualityConfiguration
{
    public static OwnedNavigationBuilder<TEntity, TValueObject> ConfigureQuality<TEntity, TValueObject>(
        this OwnedNavigationBuilder<TEntity, TValueObject> ow)
        where TEntity : class, IEntity where TValueObject : Quality
    {
        ow.Property(x => x.Value).IsRequired().HasMaxLength(10)
            .HasColumnName("Quality");
        return ow;
    }
}
using Domain.Entities.Base;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityIdConverters;

namespace Persistence.ValueObjectConfiguration;

public static class CreationInfoConfiguration
{
    public static OwnedNavigationBuilder<TEntity, TValueObject> ConfigureCreationInfo<TEntity, TValueObject>(
        this OwnedNavigationBuilder<TEntity, TValueObject> ow)
        where TEntity : class, IEntity where TValueObject : CreationInfo
    {
        ow.Property(x => x.CreatedAt).IsRequired()
            .HasColumnName(GetName(nameof(CreationInfo.CreatedAt)));
        ow.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100)
            .HasColumnName(GetName(nameof(CreationInfo.CreatedBy)));
        ow.Property(x => x.CreatedById).IsRequired()
            .HasColumnName(GetName(nameof(CreationInfo.CreatedById)));
        ow.Property(x=>x.CreatedById).HasConversion(new UserIdConverter());
        return ow;
    }

    private static string GetName(string name) => name;
}
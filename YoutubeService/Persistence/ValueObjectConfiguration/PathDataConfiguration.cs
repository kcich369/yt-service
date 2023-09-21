using Domain.Abstractions;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ValueObjectConfiguration;

public static class PathDataConfiguration
{
    public static OwnedNavigationBuilder<TEntity, TValueObject> ConfigurePathData<TEntity, TValueObject>(
        this OwnedNavigationBuilder<TEntity, TValueObject> ow)
        where TEntity : class, IEntity where TValueObject : PathData
    {
        ow.Property(x => x.MainPath).IsRequired().HasMaxLength(150)
            .HasColumnName(GetName(nameof(PathData.MainPath)));
        ow.Property(x => x.DirectoryName).IsRequired().HasMaxLength(150)
            .HasColumnName(GetName(nameof(PathData.DirectoryName)));
        ow.Property(x => x.FileName).IsRequired().HasMaxLength(150)
            .HasColumnName(GetName(nameof(PathData.FileName)));
        return ow;
    }
    private static string GetName(string name) => $"Path_{name}";
}
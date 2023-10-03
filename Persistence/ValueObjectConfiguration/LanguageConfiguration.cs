using Domain.Entities.Base;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ValueObjectConfiguration;

public static class LanguageConfiguration
{
    public static OwnedNavigationBuilder<TEntity, TValueObject> ConfigureLanguage<TEntity, TValueObject>(
        this OwnedNavigationBuilder<TEntity, TValueObject> ow)
        where TEntity : class, IEntity where TValueObject : Language
    {
        ow.Property(x => x.Name).IsRequired().HasMaxLength(100)
            .HasColumnName(GetName(nameof(Language.Name)));
        ow.Property(x => x.CultureValue).IsRequired().HasMaxLength(10)
            .HasColumnName(GetName(nameof(Language.CultureValue)));
        return ow;
    }

    private static string GetName(string name) => $"{nameof(Language)}_{name}";
}
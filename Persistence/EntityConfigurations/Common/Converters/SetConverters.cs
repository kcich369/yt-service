using Domain.EntityIds;
using Domain.EntityIds.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.Common.Converters;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder SetConverters<T>(this EntityTypeBuilder<T> builder) where T : class
    {
        foreach (var strongTypedId in typeof(T).GetProperties()
                     .Where(x => typeof(EntityId).IsAssignableFrom(x.PropertyType)
                                 || (x.PropertyType.IsGenericType &&
                                     x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                     typeof(EntityId).IsAssignableFrom(Nullable.GetUnderlyingType(x.PropertyType)))))
        {
            var converter = ConvertersWithModelClrTypes.Get()!.First(x => x.Key == GetKey(strongTypedId.PropertyType));
            builder.Property(strongTypedId.Name).HasConversion(converter.Value);
        }

        return builder;
    }

    private static string GetKey(Type propertyType)
    {
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            return Nullable.GetUnderlyingType(propertyType)!.ToString();
        return propertyType.ToString();
    }
}
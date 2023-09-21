using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityConfigurations.Common.Converters;

public static class ConvertersWithModelClrTypes
{
    private static ImmutableDictionary<string, ValueConverter>? _converters;

    public static ImmutableDictionary<string, ValueConverter>? Get() =>
        _converters ??= typeof(EntityTypeBuilderExtensions).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ValueConverter)) && !t.IsAbstract)
            .Select(converterType => (ValueConverter)Activator.CreateInstance(converterType)!)
            .ToImmutableDictionary(key => key.ModelClrType.ToString(), value => value);
}
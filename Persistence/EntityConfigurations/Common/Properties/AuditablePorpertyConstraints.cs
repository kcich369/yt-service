using System.Collections.Immutable;
using Domain.Auditable;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.Common.Properties;

public static class AuditablePropertiesConstraints
{
    private static ImmutableDictionary<string, Func<PropertyBuilder, PropertyBuilder>>? _creating;
    private static ImmutableDictionary<string, Func<PropertyBuilder, PropertyBuilder>>? _updating;

    public static ImmutableDictionary<string, Func<PropertyBuilder, PropertyBuilder>> Creating() => _creating ??=
        new Dictionary<string, Func<PropertyBuilder, PropertyBuilder>>()
        {
            { nameof(ICreated.CreatedBy), (x) => x.HasMaxLength(100).IsRequired() },
            { nameof(ICreated.CreatedAt), (x) => x.IsRequired() },
            { nameof(ICreated.CreatedById), (x) => x.IsRequired() }
        }.ToImmutableDictionary();

    public static ImmutableDictionary<string, Func<PropertyBuilder, PropertyBuilder>> Updating() => _updating ??=
        new Dictionary<string, Func<PropertyBuilder, PropertyBuilder>>()
        {
            { nameof(IUpdated.UpdatedBy), (x) => x.HasMaxLength(100) }
        }.ToImmutableDictionary();
}
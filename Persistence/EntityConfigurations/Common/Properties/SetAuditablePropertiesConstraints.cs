using Domain.Auditable;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.Common.Properties;

public static class SetAuditablePropertiesDbConstraintsExtensions
{
    public static EntityTypeBuilder SetAuditablePropertiesDbConstraints<T>(this EntityTypeBuilder<T> builder)
        where T : class => builder.Creating<T>().Updating<T>();

    private static EntityTypeBuilder Creating<T>(this EntityTypeBuilder builder) where T : class =>
        SetConstraints<T, ICreated>(builder);

    private static EntityTypeBuilder Updating<T>(this EntityTypeBuilder builder) where T : class =>
        SetConstraints<T, IUpdated>(builder);

    private static EntityTypeBuilder SetConstraints<TEntity, TAuditable>(EntityTypeBuilder builder)
        where TEntity : class
    {
        if (!typeof(TAuditable).IsAssignableFrom(typeof(TEntity)))
            return builder;

        foreach (var propertiesConstraint in typeof(TAuditable) == typeof(ICreated)
                     ? AuditablePropertiesConstraints.Creating()
                     : AuditablePropertiesConstraints.Updating())
        {
            propertiesConstraint.Value.Invoke(builder.Property(propertiesConstraint.Key));
        }

        return builder;
    }
}
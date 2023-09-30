using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;

namespace Persistence.EntityConfigurations;

public class YtChannelConfiguration : EntityConfiguration<YtChannel, YtChannelId>
{
    public YtChannelConfiguration() : base("YtChannels")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtChannel> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150);
        builder.Property(x => x.Handle).HasMaxLength(150);
        builder.Property(x => x.YtId).HasMaxLength(100);
        builder.Property(x => x.Url).HasMaxLength(150);

        builder.HasIndex(x => x.YtId);
    }
}
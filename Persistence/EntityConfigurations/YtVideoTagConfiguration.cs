using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;

namespace Persistence.EntityConfigurations;

public class YtVideoTagConfiguration : EntityConfiguration<YtVideoTag, YtVideoTagId>
{
    public YtVideoTagConfiguration() : base("YtVideoTags")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtVideoTag> builder)
    {
        builder.HasOne(x => x.YtVideoTranscription)
            .WithOne(x => x.Tag)
            .HasForeignKey<YtVideoTag>(x => x.YtVideoTranscriptionId);

        builder.HasOne(x => x.YtVideo)
            .WithOne(x => x.Tag)
            .HasForeignKey<YtVideoTag>(x => x.YtVideoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
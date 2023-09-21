using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;

namespace Persistence.EntityConfigurations;

public sealed class YtVideoDescriptionConfiguration: EntityConfiguration<YtVideoDescription, YtVideoDescriptionId>
{
    public YtVideoDescriptionConfiguration() : base("YtVideoDescriptions")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtVideoDescription> builder)
    {
        builder.HasOne(x => x.YtVideoTranscription)
            .WithOne(x => x.Description)
            .HasForeignKey<YtVideoDescription>(x => x.YtVideoTranscriptionId);
        
        builder.HasOne(x => x.YtVideo)
            .WithOne(x => x.Description)
            .HasForeignKey<YtVideoDescription>(x => x.YtVideoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;
using Persistence.ValueObjectConfiguration;

namespace Persistence.EntityConfigurations;

public sealed class YtVideoTranscriptionConfiguration : EntityConfiguration<YtVideoTranscription, YtVideoTranscriptionId>
{
    public YtVideoTranscriptionConfiguration() : base("YtVideoTranscriptions")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtVideoTranscription> builder)
    {
        builder.OwnsOne(x => x.PathData, ow => ow.ConfigurePathData());

        builder.HasOne(x => x.WavFile)
            .WithOne(x => x.YtVideoTranscription)
            .HasForeignKey<YtVideoTranscription>(x => x.WavFileId);
    }
}
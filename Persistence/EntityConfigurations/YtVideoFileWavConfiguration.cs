using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;
using Persistence.ValueObjectConfiguration;

namespace Persistence.EntityConfigurations;

public class YtVideoFileWavConfiguration : EntityConfiguration<YtVideoFileWav, YtVideoFileWavId>
{
    public YtVideoFileWavConfiguration() : base("YtVideoFileWavs")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtVideoFileWav> builder)
    {
        builder.OwnsOne(x => x.PathData, ow => ow.ConfigurePathData());
        builder.OwnsOne(x => x.Language, ow => ow.ConfigureLanguage());

        builder.HasOne(x => x.YtVideoFile)
            .WithOne(x => x.WavFile)
            .HasForeignKey<YtVideoFileWav>(x => x.YtVideoFileId);
    }
}
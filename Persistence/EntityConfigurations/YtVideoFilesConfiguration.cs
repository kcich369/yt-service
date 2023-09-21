using Domain.Entities;
using Domain.EntityIds;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;
using Persistence.ValueObjectConfiguration;

namespace Persistence.EntityConfigurations;

public class YtVideoFilesConfiguration : EntityConfiguration<YtVideoFile, YtVideoFileId>
{
    public YtVideoFilesConfiguration() : base("YtVideoFiles")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtVideoFile> builder)
    {
        builder.OwnsOne(x => x.PathData, ow => ow.ConfigurePathData());

        builder.HasOne(x => x.Video)
            .WithMany(y => y.Files)
            .HasForeignKey(x => x.VideoId);
    }
}
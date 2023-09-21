using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Common;

namespace Persistence.EntityConfigurations;

public class YtVideosConfiguration : EntityConfiguration<YtVideo, YtVideoId>
{
    public YtVideosConfiguration() : base("YtVideos")
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<YtVideo> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150);
        builder.Property(x => x.YtId).HasMaxLength(100);
        builder.Property(x => x.Url).HasMaxLength(200);
        builder.Property(x => x.LanguageCulture).HasMaxLength(10);

        builder.HasOne(x => x.Channel)
            .WithMany(y => y.Videos)
            .HasForeignKey(x => x.ChannelId);
        
        builder.HasIndex(x => x.Name);
    }
}
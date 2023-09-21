using Domain.Dtos.YtVideo;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Factories;
using ExternalServices.Dto;

namespace Infrastructure.Mappers;

internal sealed class YtVideoMapper : IYtVideoMapper
{
    public YtVideoMapper(IYtVideoFileFactory ytVideoFileFactory)
    {
    }

    public IEnumerable<YtVideo> Map(IEnumerable<YtVideoData> ytVideos, YtChannelId channelId,
        string channelName) => ytVideos.Select(video =>
            YtVideo.Create(video.Name, video.YtId, video.Url, video.Duration, channelId))
        .ToList();

    public static IEnumerable<YtVideoDto> Map(IEnumerable<YtVideo> ytVideos) =>
        ytVideos.Select(v => new YtVideoDto() { Name = v.Name, YtId = v.YtId, Url = v.Url });
}
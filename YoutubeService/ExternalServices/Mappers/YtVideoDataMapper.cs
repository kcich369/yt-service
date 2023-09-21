using ExternalServices.Dto;
using YoutubeReExplode.Common;
using YoutubeReExplode.Playlists;

namespace ExternalServices.Mappers;

public sealed class YtVideoDataMapper : IYtVideoDataMapper
{
    public async Task<IEnumerable<YtVideoData>> Map(IAsyncEnumerable<PlaylistVideo> ytVideos, int? amount,
        CancellationToken token) =>
        amount.HasValue
            ? (await ytVideos.CollectAsync(amount.Value)).Select(Map)
            : ytVideos.ToBlockingEnumerable(token).Select(Map);

    private static YtVideoData Map(PlaylistVideo ytVideo) =>
        new(ytVideo.Title, ytVideo.Id, ytVideo.Url, ytVideo.Duration);
}
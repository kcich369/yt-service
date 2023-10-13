using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExternalServices.Dto;
using YoutubeReExplode.Common;
using YoutubeReExplode.Playlists;

namespace ExternalServices.Mappers;

public sealed class YtVideoDataMapper : IYtVideoDataMapper
{
    public async Task<IList<YtVideoData>> Map(IAsyncEnumerable<PlaylistVideo> ytVideos, int? amount,
        CancellationToken token) =>
        amount.HasValue
            ? (await ytVideos.CollectAsync(amount.Value)).Select(Map).ToList()
            : await Convert(ytVideos);
        // : ytVideos.ToBlockingEnumerable(token).Select(Map).ToList();


    private static async Task<IList<YtVideoData>> Convert(IAsyncEnumerable<PlaylistVideo> ytVideos)
    {
        var videos = new List<YtVideoData>();
        await foreach (var video in ytVideos)
        {
            videos.Add(Map(video));
        }
        return videos;
    }

    private static YtVideoData Map(PlaylistVideo ytVideo) =>
        new(ytVideo.Title, ytVideo.Id, ytVideo.Url, ytVideo.Duration);
}
using Domain.EntityIds;
using ExternalServices.Dto;
using YoutubeReExplode.Playlists;

namespace ExternalServices.Mappers;

internal interface IYtVideoDataMapper
{
    Task<IEnumerable<YtVideoData>> Map(IAsyncEnumerable<PlaylistVideo> ytVideos, int? amount, CancellationToken token);
}
using Domain.Dtos;
using Domain.Dtos.YtChannel;
using Domain.Entities;
using Domain.EntityIds;

namespace Domain.Repositories;

public interface IYtChannelRepository
{
    Task Add(YtChannel ytChannel, CancellationToken token);
    Task<YtChannel> GetWithVideos(YtChannelId ytChannel, int amount, CancellationToken token);
    Task<IEnumerable<YtChannelId>> GetAllIds(CancellationToken token);
    Task<bool> Exists(string name, CancellationToken token);

    Task<YtChannelVideosDto> GetYtVideoChannelWithDownloadedVideoNames(YtChannelId ytChannelId, CancellationToken token);
}
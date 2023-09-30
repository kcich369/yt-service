using Domain.Dtos.YtVideo;
using Domain.Entities;
using Domain.EntityIds;

namespace Domain.Repositories;

public interface IYtVideoRepository
{
    Task<IEnumerable<YtVideoSearchDto>> SearchByQuery(string query, int take, CancellationToken token);
    Task<YtVideoDetailsDto> GetById(YtVideoId id, CancellationToken token);
    Task<YtVideo> GetForDownloading(YtVideoId id, CancellationToken token);
}
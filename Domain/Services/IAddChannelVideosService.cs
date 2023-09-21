using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface IAddChannelVideosService
{
    Task<Result<bool>> ApplyNewVideos(YtChannelId ytChannelId, CancellationToken token);
}
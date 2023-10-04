using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface IAddChannelVideosService
{
    Task<IResult<bool>> ApplyNewVideos(YtChannelId ytChannelId, CancellationToken token);
}
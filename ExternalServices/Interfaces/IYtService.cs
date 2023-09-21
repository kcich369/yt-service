using Domain.Results;
using ExternalServices.Dto;

namespace ExternalServices.Interfaces;

public interface IYtService
{
    Task<IResult<YtChannelData>> GetChannel(string ytChannelName, bool getByCustomUrl, CancellationToken token);
    Task<IResult<IEnumerable<YtVideoData>>> GetChannelVideos(string ytChannelUrl, int? amount, CancellationToken token);
    Task<IResult<bool>> GetYtVideoClosedCaptions(VideoData videoData, CancellationToken token);
    Task<Result<YtVideoFileInfo>> DownloadYtVideoFile(VideoData videData, CancellationToken token);
}
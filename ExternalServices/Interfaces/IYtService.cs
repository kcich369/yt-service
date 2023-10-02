using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Results;
using ExternalServices.Dto;

namespace ExternalServices.Interfaces;

public interface IYtService
{
    Task<IResult<YtChannelData>> GetChannel(string ytChannelName, bool getByHandleName, CancellationToken token);
    Task<IResult<IList<YtVideoData>>> GetChannelVideos(string ytChannelUrl, int? amount, CancellationToken token);
    Task<IResult<bool>> GetYtVideoClosedCaptions(VideoData videoData, CancellationToken token);
    Task<Result<YtVideoFileInfo>> DownloadYtVideoFile(VideoData videData, CancellationToken token);
}
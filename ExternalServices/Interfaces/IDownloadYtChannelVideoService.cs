using System.Threading;
using System.Threading.Tasks;
using Domain.Results;
using ExternalServices.Dto;

namespace ExternalServices.Interfaces;

public interface IDownloadYtChannelVideoService
{
    Task<IResult<YtVideoFileInfo>> Download(VideoData videData, CancellationToken token);
}
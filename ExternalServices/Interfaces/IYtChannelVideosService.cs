using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Results;
using ExternalServices.Dto;

namespace ExternalServices.Interfaces;

public interface IYtChannelVideosService
{
    Task<IResult<IList<YtVideoData>>> GetVideos(string ytChannelUrl, int? amount, CancellationToken token);
}
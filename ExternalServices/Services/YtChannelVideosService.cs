using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Results;
using ExternalServices.Dto;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using ExternalServices.Mappers;

namespace ExternalServices.Services;

internal sealed class YtChannelVideosService : IYtChannelVideosService
{
    private readonly IYtClientFactory _ytClientFactory;
    private readonly IYtVideoDataMapper _dataMapper;

    public YtChannelVideosService(IYtClientFactory ytClientFactory,
        IYtVideoDataMapper dataMapper)
    {
        _ytClientFactory = ytClientFactory;
        _dataMapper = dataMapper;
    }

    public async Task<IResult<IList<YtVideoData>>> GetVideos(string ytChannelUrl, int? amount,
        CancellationToken token) => Result<IList<YtVideoData>>.Success(await _dataMapper.Map(
        _ytClientFactory.GetYtClient().Channels.GetUploadsAsync(ytChannelUrl, token), amount, token));
}
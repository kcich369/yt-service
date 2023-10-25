using System.Threading;
using System.Threading.Tasks;
using Domain.Configurations;
using Domain.Enumerations;
using Domain.Results;
using ExternalServices.Dto;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;

namespace ExternalServices.Services;

public static partial class ErrorMessages
{
    public static string ChannelNotExists(string channelName) =>
        $"Channel with given name: {channelName} does not exist.";
}

internal sealed class YtChannelService : IYtChannelService
{
    private readonly IYtClientFactory _ytClientFactory;
    private readonly YtServiceConfiguration _ytServiceConfiguration;

    public YtChannelService(IYtClientFactory ytClientFactory,
        YtServiceConfiguration ytServiceConfiguration)
    {
        _ytClientFactory = ytClientFactory;
        _ytServiceConfiguration = ytServiceConfiguration;
    }

    public async Task<IResult<YtChannelData>> Get(string ytChannelName, bool getByHandleName,
        CancellationToken token)
    {
        var channelDataResult = await GetChannelData(ytChannelName, getByHandleName, token).TryCatch();
        return channelDataResult.IsError
            ? ErrorResult(ytChannelName, channelDataResult)
            : Result<YtChannelData>.Success(channelDataResult.Data);
    }

    private async Task<YtChannelData> GetChannelData(string ytChannelName, bool getByHandleName,
        CancellationToken token)
    {
        var channel = getByHandleName
            ? await _ytClientFactory.GetYtClient().Channels
                .GetByHandleAsync($"{_ytServiceConfiguration.YtUrl}{ytChannelName}", token)
            : await _ytClientFactory.GetYtClient().Channels.GetByUserAsync(
                $"{_ytServiceConfiguration.YtUrl}{_ytServiceConfiguration.YtUrlUser}{ytChannelName}", token);
        return new YtChannelData(channel.Title, channel.Id, channel.Url);
    }

    private static IResult<YtChannelData> ErrorResult(string ytChannelName, IResult channelDataResult) =>
        channelDataResult.ErrorMessage.Contains("404 (Not Found)")
            ? Result<YtChannelData>.Error(ErrorTypesEnums.NotFound, ErrorMessages.ChannelNotExists(ytChannelName))
            : Result<YtChannelData>.Error(channelDataResult);
}
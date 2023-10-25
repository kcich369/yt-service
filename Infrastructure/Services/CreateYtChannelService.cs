using Domain.Dtos.YtChannel;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Helpers;
using Domain.Providers;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public static partial class ErrorMessages
{
    public static string ChannelAlreadyExist(string name, string ytId) =>
        $"Yt Channel with given name: {name} and ytId: {ytId} already exists.";
}

public sealed class CreateYtChannelService : ICreateYtChannelWithVideosService
{
    private readonly IYtChannelRepository _ytChannelRepository;
    private readonly IPathProvider _pathProvider;
    private readonly IYtChannelService _ytChannelService;
    private readonly IDirectoryProvider _directoryProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<CreateYtChannelService> _logger;

    public CreateYtChannelService(IYtChannelRepository ytChannelRepository,
        IPathProvider pathProvider,
        IYtChannelService ytChannelService,
        IDirectoryProvider directoryProvider,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher,
        ILogger<CreateYtChannelService> logger)
    {
        _ytChannelRepository = ytChannelRepository;
        _pathProvider = pathProvider;
        _ytChannelService = ytChannelService;
        _directoryProvider = directoryProvider;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task<IResult<YtChannelVideosDto>> Execute(string handleName, CancellationToken token)
    {
        var newChannelId = new YtChannelId();

        var createChannelResult = await (await _ytChannelService.Get(handleName, true, token))
            .ReturnOut(out var channel)
            .Next(chnlRes => Exist(chnlRes.Data.YtId, handleName, token))
            .Next((r) => _ytChannelRepository.Add(YtChannel.Create(newChannelId, channel.Data.Name,
                handleName, channel.Data.YtId, channel.Data.Url), token).TryCatch())
            .Next((res) => _unitOfWork.SaveChangesAsync(token).TryCatch());
        if (createChannelResult.IsError)
            return Result<YtChannelVideosDto>.Error(createChannelResult).LogErrorMessage(_logger);

        _directoryProvider.CreateDirectoryIfNotExists(
            _pathProvider.GetRelativePath(_pathProvider.GetChannelPath(channel.Data.Name)));
        await _messagePublisher.Send(new ChannelCreated(newChannelId));
        return Result<YtChannelVideosDto>.Success(new YtChannelVideosDto(newChannelId, channel.Data.Name,
            channel.Data.YtId, null));
    }

    private async Task<IResult<bool>> Exist(string ytId, string name, CancellationToken token) =>
        await _ytChannelRepository.YtIdExists(ytId, token)
            ? Result<bool>.Error(ErrorTypesEnums.BadRequest,
                    ErrorMessages.ChannelAlreadyExist(name, ytId))
                .LogErrorMessage(_logger)
            : Result<bool>.Success(true);
}
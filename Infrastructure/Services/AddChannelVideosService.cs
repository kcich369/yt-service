using Domain.Comparers;
using Domain.Configurations;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

static partial class ErrorMessages
{
    public static string ChannelNotExists(string ytChannelId) =>
        $"Yt channel with given id {ytChannelId} does not exist";
}

public sealed class AddChannelVideosService : IAddChannelVideosService
{
    private readonly IYtChannelRepository _ytChannelRepository;
    private readonly IYtService _ytService;
    private readonly ApplyingNewVideosConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddChannelVideosService> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public AddChannelVideosService(IYtChannelRepository ytChannelRepository,
        IYtService ytService,
        ApplyingNewVideosConfiguration configuration,
        IUnitOfWork unitOfWork,
        ILogger<AddChannelVideosService> logger,
        IMessagePublisher messagePublisher)
    {
        _ytChannelRepository = ytChannelRepository;
        _ytService = ytService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task<IResult<bool>> ApplyNewVideos(YtChannelId ytChannelId, CancellationToken token)
    {
        var ytChannel = await _ytChannelRepository.GetWithVideos(ytChannelId, _configuration.Amount, token);
        if (ytChannel is null)
            return Result<bool>.Error(ErrorTypesEnums.NotFound, ErrorMessages.ChannelNotExists(ytChannelId))
                .LogErrorMessage(_logger);

        var allVideosResult = await _ytService.GetChannelVideos(ytChannel.Url, null, token);
        if (allVideosResult.IsError)
            return Result<bool>.Error(allVideosResult).LogErrorMessage(_logger);

        var newVideos = allVideosResult.Data.ToList()
            .Select(x => YtVideo.Create(x.Name, x.YtId, x.Url, x.Duration, ytChannel.Id))
            .Where(x => x.Duration is not null)
            .Except(ytChannel.Videos, new YtVideoComparer())
            .ToList();

        ytChannel.AddVideos(newVideos);
        await _unitOfWork.SaveChangesAsync(token);
        // await _messagePublisher.Send(newVideos.Where(x => x.Process).Select(x => new NewVideoCreated(x.Id,ytChannel.Id)));
        await _messagePublisher.Send(newVideos.OrderBy(x=>x.Duration).Take(1).Select(x => new NewVideoCreated(x.Id,ytChannel.Id)));

        return Result<bool>.Success(true);
    }
}
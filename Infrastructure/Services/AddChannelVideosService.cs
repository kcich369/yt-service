using Domain.Comparers;
using Domain.Configurations;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Factories;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Extensions;
using Infrastructure.Services.Base;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class AddChannelVideosService : MessagePublisherService<NewVideoCreated>,
    IAddChannelVideosService
{
    private readonly IYtChannelRepository _ytChannelRepository;
    private readonly IYtService _ytService;
    private readonly ApplyingNewVideosConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddChannelVideosService> _logger;

    public AddChannelVideosService(IYtChannelRepository ytChannelRepository,
        IYtService ytService,
        ApplyingNewVideosConfiguration configuration,
        IUnitOfWork unitOfWork,
        ILogger<AddChannelVideosService> logger,
        IMessagePublisher publisher) : base(publisher)
    {
        _ytChannelRepository = ytChannelRepository;
        _ytService = ytService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> ApplyNewVideos(YtChannelId ytChannelId, CancellationToken token)
    {
        var ytChannel = await _ytChannelRepository.GetWithVideos(ytChannelId, _configuration.Amount, token);
        if (ytChannel is null)
            return Result<bool>.Error(ErrorTypesEnums.NotFound, "Yt channel with given id does not exist")
                .LogErrorMessage(_logger);
        var allVideosResult = await _ytService.GetChannelVideos(ytChannel.Url, null, token);
        if (allVideosResult.IsError)
            return Result<bool>.Error(allVideosResult).LogErrorMessage(_logger);

        var newVideos = allVideosResult.Data
            .Select(x => YtVideo.Create(x.Name, x.YtId, x.Url, x.Duration, ytChannel.Id))
            .Where(x => x.Duration is not null)
            .Except(ytChannel.Videos, new YtVideoComparer())
            .ToList();

        ytChannel.AddVideos(newVideos);
        await _unitOfWork.SaveChangesAsync(token);
        await Publish(newVideos.Select(x => new NewVideoCreated(x.Id)));
        
        return Result<bool>.Success(true);
    }
}
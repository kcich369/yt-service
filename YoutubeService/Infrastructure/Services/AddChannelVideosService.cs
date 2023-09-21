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
using Infrastructure.Services.Base;
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

    public AddChannelVideosService(IYtChannelRepository ytChannelRepository,
        IYtService ytService,
        ApplyingNewVideosConfiguration configuration,
        IUnitOfWork unitOfWork,
        IMessagePublisher publisher) : base(publisher)
    {
        _ytChannelRepository = ytChannelRepository;
        _ytService = ytService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> ApplyNewVideos(YtChannelId ytChannelId, CancellationToken token)
    {
        var ytChannel = await _ytChannelRepository.GetWithVideos(ytChannelId, _configuration.Amount, token);
        if (ytChannel is null)
            return Result<bool>.Error(ErrorTypesEnums.NotFound, "Yt channel with given id does not exist");
        var allVideosResult = await _ytService.GetChannelVideos(ytChannel.Url, null, token);
        if (allVideosResult.IsError)//todo: log errors
            return Result<bool>.Error(allVideosResult);

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
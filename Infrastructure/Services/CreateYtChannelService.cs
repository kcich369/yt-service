using Domain.Dtos.YtChannel;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Providers;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Dto;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Extensions;
using Infrastructure.Mappers;
using Infrastructure.Services.Base;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public sealed class CreateYtChannelService : MessagePublisherService<ChannelCreated>,
    ICreateYtChannelWithVideosService
{
    private readonly IYtChannelRepository _ytChannelRepository;
    private readonly IPathProvider _pathProvider;
    private readonly IYtService _ytService;
    private readonly IDirectoryProvider _directoryProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateYtChannelService> _logger;

    public CreateYtChannelService(IYtChannelRepository ytChannelRepository,
        IPathProvider pathProvider,
        IYtService ytService,
        IDirectoryProvider directoryProvider,
        IUnitOfWork unitOfWork,
        IMessagePublisher publisher,
        ILogger<CreateYtChannelService> logger) : base(publisher)
    {
        _ytChannelRepository = ytChannelRepository;
        _pathProvider = pathProvider;
        _ytService = ytService;
        _directoryProvider = directoryProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IResult<YtChannelVideosDto>> Execute(string name, bool downloadByCustomUrl,
        CancellationToken token)
    {
        var newChannelId = new YtChannelId();
        var createChannelResult = await (await _ytService.GetChannel(name, downloadByCustomUrl, token))
            .ReturnOut(out var channel)
            .Next((chnl) => _ytChannelRepository
                .Add(YtChannel.Create(newChannelId, chnl.Data.Name, chnl.Data.YtId, chnl.Data.Url), token)
                .TryCatch())
            .Next((res) => _unitOfWork.SaveChangesAsync(token).TryCatch());

        if (createChannelResult.IsError)
            Result<YtChannelVideosDto>.Error(createChannelResult).LogErrorMessage(_logger);

        _directoryProvider.CreateIfNotExists(_pathProvider.GetChannelPath(channel.Data.Name));
        await Publish(new ChannelCreated(newChannelId.ToString()));
        return Result<YtChannelVideosDto>.Success(new YtChannelVideosDto(newChannelId, channel.Data.Name,
            channel.Data.YtId, null));
    }
}
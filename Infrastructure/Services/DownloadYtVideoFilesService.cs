using Domain.Configurations;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Domain.Providers;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Dto;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Extensions;
using Infrastructure.Services.Base;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public class DownloadYtVideoFilesService : MessagePublisherService<VideoDownloaded>, IDownloadYtVideoFilesService
{
    private readonly IYtVideoRepository _ytVideoRepository;
    private readonly IYtService _ytService;
    private readonly FilesDataConfiguration _filesDataConfiguration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DownloadYtVideoFilesService> _logger;

    public DownloadYtVideoFilesService(IYtVideoRepository ytVideoRepository,
        IYtService ytService,
        FilesDataConfiguration filesDataConfiguration,
        IUnitOfWork unitOfWork,
        ILogger<DownloadYtVideoFilesService> logger,
        IMessagePublisher publisher) : base(publisher)
    {
        _ytVideoRepository = ytVideoRepository;
        _ytService = ytService;
        _filesDataConfiguration = filesDataConfiguration;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Download(YtVideoId ytVideoId, CancellationToken token)
    {
        var ytVideo = await _ytVideoRepository.GetForDownloading(ytVideoId, token);
        if (ytVideo is null)
            return Result<bool>.Error(ErrorTypesEnums.BadRequest, $"Yt video with given id {ytVideoId} does not exist.")
                .LogErrorMessage(_logger);
        if (!ytVideo.Process)
            return Result<bool>.Error(ErrorTypesEnums.BadRequest,
                    $"Yt video with given id {ytVideoId} can not be processed.")
                .LogErrorMessage(_logger);

        var existedQualities = ytVideo.Files.Select(x => x.Quality).ToList();
        var mainPath = $@"{_filesDataConfiguration.Path}\{ytVideo.Channel.Name}\{ytVideo.YtId}";

        foreach (var quality in Enumeration.GetAll<VideoQualityEnum>())
        {
            if (existedQualities.Contains(quality.Name))
                continue;
            var downloadedResult =
                await _ytService.DownloadYtVideoFile(new VideoData(ytVideo.Url, quality.Name, mainPath,ytVideo.YtId), token);
            if (downloadedResult
                .LogErrorMessage(_logger, $"Downloading video error: ytVideoId {ytVideoId} and quality {quality}.")
                .IsError)
                continue;

            ytVideo.AddFile(YtVideoFile.Create(mainPath, quality)
                .SetFileInfo(downloadedResult.Data.FileName, downloadedResult.Data.Extension,
                    downloadedResult.Data.Bytes));
        }

        await Publish(ytVideo.Process
            ? ytVideo.Files.Where(x => !existedQualities.Contains(x.Quality)).Select(x => new VideoDownloaded(x.Id))
            : Enumerable.Empty<VideoDownloaded>());

        await _unitOfWork.SaveChangesAsync(token);
        return Result<bool>.Success(true);
    }
}
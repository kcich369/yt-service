﻿using Domain.Configurations;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Dto;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public static partial class ErrorMessages
{
    public static string DownloadingError(string ytVideoId, string quality) =>
        $"Downloading video error: ytVideoId {ytVideoId} and quality {quality}.";

    public static string DownloadingValidationError(string ytVideoId) =>
        $"Yt video with given id {ytVideoId} can not be processed.";

    public static string YtVideoNotExists(string ytVideoId) =>
        $"Yt video with given id {ytVideoId} does not exist.";
}

public class DownloadYtVideoFilesService : IDownloadYtVideoFilesService
{
    private readonly IYtVideoRepository _ytVideoRepository;
    private readonly IDownloadYtChannelVideoService _downloadYtChannelVideoService;
    private readonly FilesDataConfiguration _filesDataConfiguration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DownloadYtVideoFilesService> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public DownloadYtVideoFilesService(IYtVideoRepository ytVideoRepository,
        IDownloadYtChannelVideoService downloadYtChannelVideoService,
        FilesDataConfiguration filesDataConfiguration,
        IUnitOfWork unitOfWork,
        ILogger<DownloadYtVideoFilesService> logger,
        IMessagePublisher messagePublisher)
    {
        _ytVideoRepository = ytVideoRepository;
        _downloadYtChannelVideoService = downloadYtChannelVideoService;
        _filesDataConfiguration = filesDataConfiguration;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task<IResult<bool>> Download(YtVideoId ytVideoId, CancellationToken token)
    {
        var ytVideo = await _ytVideoRepository.GetForDownloading(ytVideoId, token);

        if (ytVideo is null)
            return Result<bool>.Error(ErrorTypesEnums.BadRequest, ErrorMessages.YtVideoNotExists(ytVideoId))
                .LogErrorMessage(_logger);
        // if (!ytVideo.Process)
        //     return Result<bool>.Error(ErrorTypesEnums.BadRequest, ErrorMessages.DownloadingValidationError(ytVideoId))
        //         .LogErrorMessage(_logger);

        var existedQualities = ytVideo.Files.Select(x => x.Quality.Value).ToList();
        var mainPath = MainPath(ytVideo.Channel.Name, ytVideo.YtId);
        foreach (var quality in Enumeration.GetAll<VideoQualityEnum>())
        {
            if (existedQualities.Contains(quality.Name))
                continue;
            if (quality == VideoQualityEnum.Mp3 && CheckIfMp3FileExists(ytVideo.Files))
                continue;
            var downloadedResult =
                await _downloadYtChannelVideoService.Download(
                    new VideoData(ytVideo.Url, quality, mainPath, ytVideo.YtId),
                    token);
            if (downloadedResult
                .LogErrorMessage(_logger, ErrorMessages.DownloadingError(ytVideoId, quality.Name))
                .IsError)
                continue;

            ytVideo.AddFile(YtVideoFile.Create(mainPath, quality)
                .SetFileInfo(downloadedResult.Data.FileName, downloadedResult.Data.Extension,
                    downloadedResult.Data.Bytes));
        }

        // await _messagePublisher.Send(ytVideo.Process
        //     ? ytVideo.Files
        //         .Where(x => !existedQualities.Contains(x.Quality.Value) && x.Quality == VideoQualityEnum.High)
        //         .Select(x => new VideoDownloaded(x.Id, ytVideo.Id))
        //     : Enumerable.Empty<VideoDownloaded>());

        await _unitOfWork.SaveChangesAsync(token);
        return Result<bool>.Success(true);
    }

    private static bool CheckIfMp3FileExists(IEnumerable<YtVideoFile> ytVideoFiles) =>
        ytVideoFiles.Select(x => x.PathData.FileExtension).Any(x => x == VideoQualityEnum.Mp3.Name.ToLower());

    private string MainPath(string channelName, string channelYtId)
    {
        return $@"{_filesDataConfiguration.Path}\{channelName}\{channelYtId}";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Configurations;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Providers;
using Domain.Results;
using ExternalServices.Dto;
using ExternalServices.Factories;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using ExternalServices.Mappers;
using YoutubeReExplode.Videos.Streams;

namespace ExternalServices.Services;

internal sealed class YtService : IYtService
{
    private readonly IYtClientFactory _ytClientFactory;
    private readonly IYtVideoDataMapper _dataMapper;
    private readonly IPathProvider _pathProvider;
    private readonly IDirectoryProvider _directoryProvider;
    private readonly YtServiceConfiguration _ytServiceConfiguration;

    public YtService(IYtClientFactory ytClientFactory,
        IYtVideoDataMapper dataMapper,
        IPathProvider pathProvider,
        IDirectoryProvider directoryProvider,
        YtServiceConfiguration ytServiceConfiguration)
    {
        _ytClientFactory = ytClientFactory;
        _dataMapper = dataMapper;
        _pathProvider = pathProvider;
        _directoryProvider = directoryProvider;
        _ytServiceConfiguration = ytServiceConfiguration;
    }

    public async Task<IResult<YtChannelData>> GetChannel(string ytChannelName, bool getByHandleName,
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
            : await _ytClientFactory.GetYtClient().Channels
                .GetByUserAsync(
                    $"{_ytServiceConfiguration.YtUrl}{_ytServiceConfiguration.YtUrlUser}{ytChannelName}", token);
        return new YtChannelData(channel.Title, channel.Id, channel.Url);
    }

    private IResult<YtChannelData> ErrorResult(string ytChannelName, IResult channelDataResult) =>
        channelDataResult.ErrorMessage.Contains("404 (Not Found)")
            ? Result<YtChannelData>.Error(ErrorTypesEnums.NotFound,
                $"Channel with given name: {ytChannelName} does not exist.")
            : Result<YtChannelData>.Error(channelDataResult);

    public async Task<IResult<IList<YtVideoData>>> GetChannelVideos(string ytChannelUrl, int? amount,
        CancellationToken token) => Result<IList<YtVideoData>>.Success(
        await _dataMapper.Map(_ytClientFactory.GetYtClient().Channels.GetUploadsAsync(ytChannelUrl, token), amount,
            token));

    public async Task<IResult<bool>> GetYtVideoClosedCaptions(VideoData videoData, CancellationToken token)
    {
        // try
        // {
        //     var trackManifest = await _ytClientFactory.GetYtClient().Videos.ClosedCaptions
        //         .GetManifestAsync(videoData.Url, token);
        //
        //     var track = await _ytClientFactory.GetYtClient().Videos.ClosedCaptions
        //         .GetAsync(trackManifest.GetByLanguage(videoData.LanguageCulture?.Take(2).ToString() ?? string.Empty),
        //             token);
        //
        //     var transcriptionPath = _pathProvider.GetRelativePath(
        //         _pathProvider.GetVideoTranscriptionDirectoryPath(videoData.ChannelDirectoryName,
        //             videoData.VideoDirectoryName));
        //     _directoryProvider.CreateDirectoryIfNotExists(transcriptionPath);
        //     await File.AppendAllLinesAsync($"transcriptionPath{videoData.VideoDirectoryName}.txt",
        //         track.Captions.Select(x => x.Text), token);
        //     return Data<bool>.Success(true);
        // }
        // catch (Exception e)
        // {
        //     return Data<bool>.Error(ErrorTypesEnums.Exception, e.Message);
        // }
        throw new NotImplementedException();
    }

    public async Task<IResult<YtVideoFileInfo>> DownloadYtVideoFile(VideoData videoData, CancellationToken token)
    {
        var downloadingResult = await Download(videoData, token).TryCatch();
        return downloadingResult.IsError
            ? Result<YtVideoFileInfo>.Error(downloadingResult)
            : Result<YtVideoFileInfo>.Success(downloadingResult.Data);
    }

    private async Task<IResult<YtVideoFileInfo>> Download(VideoData videoData, CancellationToken token)
    {
        var streamInfo = SelectAudioOnlyStream(await _ytClientFactory.GetYtClient().Videos.Streams
            .GetManifestAsync(videoData.Url, token), videoData.Quality);
        if (streamInfo == null)
            return Result<YtVideoFileInfo>.Error(ErrorTypesEnums.NotFound,
                $"Can not find audio file while downloading for quality {videoData.Quality} and ytId: {videoData.YtId}");
        _directoryProvider.CreateDirectoryIfNotExists(_pathProvider.GetRelativePath(videoData.MainPath));
        var fileName = $"{videoData.YtId}_{videoData.Quality}";

        if (!_directoryProvider.FileExists(_pathProvider.GetRelativePath(_pathProvider.GetVideoFilePath(
                videoData.MainPath, fileName, streamInfo.Container.ToString()))))
            await _ytClientFactory.GetYtClient().Videos.Streams.DownloadAsync(
                streamInfo, _pathProvider.GetRelativePath(
                    _pathProvider.GetVideoFilePath(videoData.MainPath, fileName, streamInfo.Container.ToString())),
                null, token);

        return Result<YtVideoFileInfo>.Success(new YtVideoFileInfo(fileName,
            streamInfo.Container.ToString(), streamInfo.Size.Bytes));
    }

    private static IStreamInfo SelectAudioOnlyStream(StreamManifest streamManifests, VideoQualityEnum quality)
    {
        IStreamInfo streamInfo = null;
        var streams = streamManifests.GetAudioOnlyStreams().OrderByDescending(x => x.Size).ToList();
        if (quality == VideoQualityEnum.High)
            streamInfo = streams.GetWithHighestBitrate();
        if (quality == VideoQualityEnum.Mp3)
            streamInfo = streams.FirstOrDefault(x => x.Container.ToString() == "mp3");
        if (quality == VideoQualityEnum.Low)
            streamInfo = streams.ElementAt(streams.Count - 2);
        return streamInfo;
    }
}
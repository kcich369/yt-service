using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enumerations;
using Domain.Providers;
using Domain.Results;
using ExternalServices.Dto;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using YoutubeReExplode.Videos.Streams;

namespace ExternalServices.Services;

public static partial class ErrorMessages
{
    public static string ChannelNotExists(string quality, string ytId) =>
        $"Can not find audio file while downloading for quality {quality} and ytId: {ytId}";
}

internal sealed class DownloadYtChannelVideoService : IDownloadYtChannelVideoService
{
    private readonly IYtClientFactory _ytClientFactory;
    private readonly IPathProvider _pathProvider;
    private readonly IDirectoryProvider _directoryProvider;

    public DownloadYtChannelVideoService(IYtClientFactory ytClientFactory,
        IPathProvider pathProvider,
        IDirectoryProvider directoryProvider)
    {
        _ytClientFactory = ytClientFactory;
        _pathProvider = pathProvider;
        _directoryProvider = directoryProvider;
    }

    public async Task<IResult<YtVideoFileInfo>> Download(VideoData videoData, CancellationToken token)
    {
        var downloadingResult = await DownloadYtVideo(videoData, token).TryCatch();
        return downloadingResult.IsError
            ? Result<YtVideoFileInfo>.Error(downloadingResult)
            : Result<YtVideoFileInfo>.Success(downloadingResult.Data);
    }

    private async Task<IResult<YtVideoFileInfo>> DownloadYtVideo(VideoData videoData, CancellationToken token)
    {
        var streamInfo = SelectAudioOnlyStream(await _ytClientFactory.GetYtClient().Videos.Streams
            .GetManifestAsync(videoData.Url, token), videoData.Quality);
        if (streamInfo == null)
            return Result<YtVideoFileInfo>.Error(ErrorTypesEnums.NotFound,
                ErrorMessages.ChannelNotExists(videoData.Quality, videoData.YtId));
        _directoryProvider.CreateDirectoryIfNotExists(_pathProvider.GetRelativePath(videoData.MainPath));
        var fileName = $"{videoData.YtId}_{videoData.Quality}";

        if (!_directoryProvider.FileExists(_pathProvider.GetRelativePath(_pathProvider.GetVideoFilePath(
                videoData.MainPath, fileName, streamInfo.Container.ToString()))))
            await _ytClientFactory.GetYtClient().Videos.Streams.DownloadAsync(
                streamInfo, _pathProvider.GetRelativePath(
                    _pathProvider.GetVideoFilePath(videoData.MainPath, fileName, streamInfo.Container.ToString())),
                null, token);

        return Result<YtVideoFileInfo>.Success(new YtVideoFileInfo(fileName, streamInfo.Container.ToString(),
            streamInfo.Size.Bytes));
    }

    private static IStreamInfo SelectAudioOnlyStream(StreamManifest streamManifests, VideoQualityEnum quality)
    {
        IStreamInfo streamInfo = null;
        var streams = streamManifests.GetAudioOnlyStreams().OrderByDescending(x => x.Size).ToList();
        if (quality == VideoQualityEnum.High)
            streamInfo = streams.GetWithHighestBitrate();
        if (quality == VideoQualityEnum.Mp3)
            streamInfo = streams.FirstOrDefault(x => x.Container.ToString() == FileExtensionsEnum.Mp3.Name);
        if (quality == VideoQualityEnum.Low)
            streamInfo = streams.ElementAt(streams.Count - 2);
        return streamInfo;
    }
}
using Domain.Configurations;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Providers;
using Domain.Results;
using ExternalServices.Dto;
using ExternalServices.Factories;
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

    public async Task<IResult<YtChannelData>> GetChannel(string ytChannelName, bool getByCustomUrl,
        CancellationToken token)
    {
        try
        {
            var channel = getByCustomUrl
                ? await _ytClientFactory.GetYtClient().Channels
                    .GetByHandleAsync($"{_ytServiceConfiguration.YtUrl}{ytChannelName}", token)
                : await _ytClientFactory.GetYtClient().Channels
                    .GetByUserAsync(
                        $"{_ytServiceConfiguration.YtUrl}{_ytServiceConfiguration.YtUrlUser}{ytChannelName}", token);
            return Result<YtChannelData>.Success(new YtChannelData(channel.Title, channel.Id, channel.Url));
        }
        catch (Exception e)
        {
            return e.Message.Contains("404 (Not Found)")
                ? Result<YtChannelData>.Error(ErrorTypesEnums.NotFound, "Channel with given name does not exist.")
                : Result<YtChannelData>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }

    public async Task<IResult<IEnumerable<YtVideoData>>> GetChannelVideos(string ytChannelUrl, int? amount,
        CancellationToken token) => Result<IEnumerable<YtVideoData>>.Success(await _dataMapper.Map(
        _ytClientFactory.GetYtClient().Channels.GetUploadsAsync(ytChannelUrl, token), amount, token));

    public async Task<IResult<bool>> GetYtVideoClosedCaptions(VideoData videoData, CancellationToken token)
    {
        try
        {
            var trackManifest = await _ytClientFactory.GetYtClient().Videos.ClosedCaptions
                .GetManifestAsync(videoData.Url, token);

            var track = await _ytClientFactory.GetYtClient().Videos.ClosedCaptions
                .GetAsync(trackManifest.GetByLanguage(videoData.LanguageCulture?.Take(2).ToString() ?? string.Empty),
                    token);

            var transcriptionPath = _pathProvider.GetRelativePath(
                _pathProvider.GetVideoTranscriptionDirectoryPath(videoData.ChannelDirectoryName,
                    videoData.VideoDirectoryName));
            _directoryProvider.CreateIfNotExists(transcriptionPath);
            await File.AppendAllLinesAsync($"transcriptionPath{videoData.VideoDirectoryName}.txt",
                track.Captions.Select(x => x.Text), token);
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }

    public async Task<Result<YtVideoFileInfo>> DownloadYtVideoFile(VideoData videData, CancellationToken token)
    {
        try
        {
            var streamInfo = SelectAudioOnlyStream(await _ytClientFactory.GetYtClient().Videos.Streams
                .GetManifestAsync(videData.Url, token), videData.Quality);
            var videoFilePath = PrepareDirectoryAndPath(videData, streamInfo);
            await _ytClientFactory.GetYtClient().Videos.Streams.DownloadAsync(streamInfo,
                _pathProvider.GetRelativePath(videoFilePath), null, token);
            return Result<YtVideoFileInfo>.Success(new YtVideoFileInfo(videoFilePath, streamInfo.Size.Bytes,
                streamInfo.Container.ToString()));
        }
        catch (Exception e)
        {
            return Result<YtVideoFileInfo>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }

    private string PrepareDirectoryAndPath(VideoData videData, IStreamInfo streamInfo)
    {
        var videoDirectoryPath = _pathProvider.GetVideoDirectoryPath(videData.ChannelDirectoryName,
            videData.VideoDirectoryName);
        _directoryProvider.CreateIfNotExists(_pathProvider.GetRelativePath(videoDirectoryPath));
        return _pathProvider.GetVideoFilePath(videData.ChannelDirectoryName, videData.VideoDirectoryName,
            videData.Quality, streamInfo.Container.ToString());
    }

    private static IStreamInfo SelectAudioOnlyStream(StreamManifest streamManifests, string quality)
    {
        IStreamInfo streamInfo = null;
        var streams = streamManifests.GetAudioOnlyStreams().OrderByDescending(x => x.Size).ToList();
        if (quality == VideoQualityEnum.High.Name)
            streamInfo = streams.GetWithHighestBitrate();
        if (quality == VideoQualityEnum.Low.Name)
            streamInfo = streams.ElementAt(streams.Count - 2);
        return streamInfo ??= streams.Last();
    }
}
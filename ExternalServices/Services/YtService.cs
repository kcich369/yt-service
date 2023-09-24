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

    public async Task<IResult<YtChannelData>> GetChannel(string ytChannelName, bool getByHandleName,
        CancellationToken token)
    {
        try
        {
            var channel = getByHandleName
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
                ? Result<YtChannelData>.Error(ErrorTypesEnums.NotFound,
                    $"Channel with given name: {ytChannelName} does not exist.")
                : Result<YtChannelData>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }

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
        //     _directoryProvider.CreateIfNotExists(transcriptionPath);
        //     await File.AppendAllLinesAsync($"transcriptionPath{videoData.VideoDirectoryName}.txt",
        //         track.Captions.Select(x => x.Text), token);
        //     return Result<bool>.Success(true);
        // }
        // catch (Exception e)
        // {
        //     return Result<bool>.Error(ErrorTypesEnums.Exception, e.Message);
        // }
        throw new NotImplementedException();
    }

    public async Task<Result<YtVideoFileInfo>> DownloadYtVideoFile(VideoData videData, CancellationToken token)
    {
        try
        {
            var streamInfo = SelectAudioOnlyStream(await _ytClientFactory.GetYtClient().Videos.Streams
                .GetManifestAsync(videData.Url, token), videData.Quality);
            _directoryProvider.CreateIfNotExists(_pathProvider.GetRelativePath(videData.MainPath));
            var fileName = $"{videData.YtId}_{videData.Quality}";
            await _ytClientFactory.GetYtClient().Videos.Streams.DownloadAsync(streamInfo,
                _pathProvider.GetRelativePath(_pathProvider.GetVideoFilePath(videData.MainPath,
                    fileName, streamInfo.Container.ToString())), null, token);

            return Result<YtVideoFileInfo>.Success(new YtVideoFileInfo(fileName, streamInfo.Container.ToString(),
                streamInfo.Size.Bytes));
        }
        catch (Exception e)
        {
            return Result<YtVideoFileInfo>.Error(ErrorTypesEnums.Exception, e.Message);
        }
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
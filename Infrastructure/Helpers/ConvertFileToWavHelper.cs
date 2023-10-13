using System.Diagnostics;
using Domain.Enumerations;
using Domain.Providers;
using Domain.Results;
using FFMpegCore;
using Infrastructure.Dtos;
using Infrastructure.Extensions;
using Infrastructure.Helpers.Interfaces;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace Infrastructure.Helpers;

internal sealed class ConvertFileToWavHelper : IConvertFileToWavHelper
{
    private readonly IPathProvider _pathProvider;
    private readonly ILogger<ConvertFileToWavHelper> _logger;
    private const string FileExtension = "wav";

    public ConvertFileToWavHelper(IPathProvider pathProvider,
        ILogger<ConvertFileToWavHelper> logger)
    {
        _pathProvider = pathProvider;
        _logger = logger;
    }

    public async Task<IResult<PathDataDto>> ConvertFileToWav(PathDataDto pathData, VideoQualityEnum quality,
        CancellationToken token)
    {
        _logger.LogInformation($"Converting file with path {pathData.FullValue} and quality: {quality} is starting");
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var conversionResult = quality == VideoQualityEnum.Mp3
            ? await ConvertMp3(pathData).TryCatch()
            : await ConvertFile(pathData).TryCatch();
        stopWatch.Stop();
        _logger.LogInformation(
            $"Convertion file with path {pathData.FullValue} and quality: {quality}, time: {stopWatch.Elapsed}");

        if (conversionResult.IsError)
            return Result<PathDataDto>.Error(ErrorTypesEnums.Exception, "Error occurred  during converting file")
                .LogErrorMessage(_logger);

        return Result<PathDataDto>.Success(conversionResult.Data);
    }

    private async Task<PathDataDto> Convert(PathDataDto pathData)
    {
        var filePath = pathData.FullValue;
        var result = await FFMpegArguments
            .FromFileInput(_pathProvider.GetRelativePath(filePath))
            .OutputToFile(_pathProvider.GetRelativePath(pathData.ConvertTo(FileExtension).FullValue), false, options =>
                options
                    .ForceFormat(FileExtension)
                    .WithFastStart())
            .ProcessAsynchronously();
        return pathData;
    }
    
    private async Task<PathDataDto> ConvertFile(PathDataDto pathData)
    {
        var inputFile = new MediaFile { Filename = _pathProvider.GetRelativePath(pathData.FullValue)};
        var outputFile = new MediaFile { Filename = _pathProvider.GetRelativePath(pathData.ConvertTo(FileExtension).FullValue)};
        var conversionOptions = new ConversionOptions
        {
            AudioSampleRate = AudioSampleRate.Default//Separating audio from video
        };
        using var engine = new Engine();
        engine.Convert(inputFile, outputFile);//, conversionOptions);
        return pathData;
    }

    private async Task<PathDataDto> ConvertMp3(PathDataDto pathData)
    {
        // await using var reader =
        //     new MediaFoundationReader(pathData.FullValue);
        // WaveFileWriter.CreateWaveFile(pathData.ConvertTo(FileExtension).FullValue, reader);
        await using var mp3Reader = new Mp3FileReader(_pathProvider.GetRelativePath(pathData.FullValue));
        await using var waveWriter =
            new WaveFileWriter(_pathProvider.GetRelativePath(pathData.ConvertTo(FileExtension).FullValue),
                mp3Reader.WaveFormat);
        await mp3Reader.CopyToAsync(waveWriter);

        // await using var mp3 = new Mp3FileReader(_pathProvider.GetRelativePath(pathData.FullValue));
        // await using var pcm = WaveFormatConversionStream.CreatePcmStream(mp3);
        // WaveFileWriter.CreateWaveFile(_pathProvider.GetRelativePath(pathData.ConvertTo(FileExtension).FullValue), pcm);
        return pathData;
    }
}
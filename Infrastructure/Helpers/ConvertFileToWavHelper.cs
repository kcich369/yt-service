using System.Diagnostics;
using Domain.Enumerations;
using Domain.Results;
using FFMpegCore;
using Infrastructure.Dtos;
using Infrastructure.Extensions;
using Infrastructure.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace Infrastructure.Helpers;

internal sealed class ConvertFileToWavHelper : IConvertFileToWavHelper
{
    private readonly ILogger<ConvertFileToWavHelper> _logger;
    private const string FileExtension = "wav";

    public ConvertFileToWavHelper(ILogger<ConvertFileToWavHelper> logger)
    {
        _logger = logger;
    }

    public async Task<IResult<PathDataDto>> ConvertFileToWav(PathDataDto pathData, CancellationToken token)
    {
        _logger.LogInformation($"Converting file with path {pathData.FullValue} is starting");
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = await FFMpegArguments
            .FromFileInput(pathData.FullValue)
            .OutputToFile(pathData.ConvertTo(FileExtension).FullValue, false, options => options
                .ForceFormat(FileExtension)
                .WithFastStart())
            .ProcessAsynchronously();
        stopWatch.Stop();
        _logger.LogInformation($"Convertion time: {stopWatch.Elapsed}");

        if (!result)
            return Result<PathDataDto>
                .Error(ErrorTypesEnums.Exception, "Error occurred  during converting file").LogErrorMessage(_logger);



        return Result<PathDataDto>.Success(pathData);
    }

    private async Task<bool> ConvertMp3(string _inPath_, string _outPath_)
    {
        // await using var reader =
        //     new MediaFoundationReader(pathData.FullValue);
        // WaveFileWriter.CreateWaveFile(pathData.ConvertTo(FileExtension).FullValue, reader);
        await using Mp3FileReader mp3Reader = new Mp3FileReader(_inPath_);
        await using WaveFileWriter waveWriter = new WaveFileWriter(_outPath_, mp3Reader.WaveFormat);
        await mp3Reader.CopyToAsync(waveWriter);
        await using var mp3 = new Mp3FileReader(_inPath_);
        await using var pcm = WaveFormatConversionStream.CreatePcmStream(mp3);
        WaveFileWriter.CreateWaveFile(_outPath_, pcm);
        return true;
    }
}
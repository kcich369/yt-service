using Domain.Enumerations;
using Domain.Providers;
using Domain.Results;
using FFMpegCore;
using Infrastructure.Dtos;
using Infrastructure.Helpers.Interfaces;
using NAudio.Wave;

namespace Infrastructure.Helpers;

public static partial class ErrorMessages
{
    public static string ConvertingError =>
        $"Error occurred  during converting file.";

    public static string WavFileExists(string fileName) =>
        $"Wav file with given path and name:{fileName} already exists.";
}

public sealed class ConvertFileToWavHelper : IConvertFileToWavHelper
{
    private readonly IDirectoryProvider _directoryProvider;
    private readonly IPathProvider _pathProvider;
    private const string FileExtension = "wav";

    public ConvertFileToWavHelper(IDirectoryProvider directoryProvider,
        IPathProvider pathProvider)
    {
        _directoryProvider = directoryProvider;
        _pathProvider = pathProvider;
    }

    public async Task<IResult<PathDataDto>> ConvertFileToWav(PathDataDto pathData, CancellationToken token)
    {
        var audioFilePath = pathData.FullValue;
        var wavPathData = pathData.ConvertTo(FileExtension);
        if (_directoryProvider.FileExists(_pathProvider.GetRelativePath(wavPathData.FullValue)))
            return Result<PathDataDto>.Error(ErrorTypesEnums.Validation,
                ErrorMessages.WavFileExists(wavPathData.FileName));

        var result = await FFMpegArguments
            .FromFileInput(audioFilePath)
            .OutputToFile(pathData.ConvertTo(FileExtension).FullValue, false, options => options
                .ForceFormat(FileExtension)
                .WithFastStart())
            .ProcessAsynchronously();

        return result
            ? Result<PathDataDto>.Success(pathData)
            : Result<PathDataDto>.Error(ErrorTypesEnums.Exception, ErrorMessages.ConvertingError);
    }

    //unused
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
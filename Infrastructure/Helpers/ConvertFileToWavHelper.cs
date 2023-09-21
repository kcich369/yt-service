using Domain.Enumerations;
using Domain.Helpers;
using Domain.Results;
using Domain.Services;
using FFMpegCore;
using NAudio.Wave;

namespace Infrastructure.Helpers;

public sealed class ConvertFileToWavHelper : IConvertFileToWavHelper
{
    public async Task<IResult<bool>> ConvertFileToWav(string inputPath, string outputPath, CancellationToken token)
    {
        var result = await FFMpegArguments
            .FromFileInput(inputPath)
            .OutputToFile(outputPath, false, options => options
                .ForceFormat("wav")
                .WithFastStart())
            .ProcessAsynchronously();
        if(!result)
            return Result<bool>.Error(ErrorTypesEnums.Exception,"Error occurred  during converting file");
       
        await using var reader =
            new MediaFoundationReader(inputPath);
        WaveFileWriter.CreateWaveFile(outputPath, reader);

        return Result<bool>.Success(true);
    }
}
using Domain.Results;

namespace Domain.Helpers;

public interface IConvertFileToWavHelper
{
    Task<IResult<bool>> ConvertFileToWav(string inputPath, string outputPath, CancellationToken token);
}
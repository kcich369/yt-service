using Domain.Results;
using Infrastructure.Dtos;

namespace Infrastructure.Helpers.Interfaces;

public interface IConvertFileToWavHelper
{
    Task<IResult<PathDataDto>> ConvertFileToWav(PathDataDto pathData, CancellationToken token);
}
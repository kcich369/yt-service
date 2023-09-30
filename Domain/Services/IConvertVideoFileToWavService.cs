using Domain.Entities;
using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface IConvertVideoFileToWavService
{
    Task<IResult<bool>> Convert(YtVideoFileId ytVideoFileId, CancellationToken token);
}
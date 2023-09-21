using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface ITranscriptionDataService
{
    Task<IResult<bool>> Create(YtVideoTranscriptionId ytVideoTranscriptionId, CancellationToken token);
}
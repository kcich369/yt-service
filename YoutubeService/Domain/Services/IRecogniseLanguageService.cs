using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface IRecogniseLanguageService
{
    Task<IResult<bool>> Recognise(YtVideoFileWavId videoFileWavId, CancellationToken token);
}
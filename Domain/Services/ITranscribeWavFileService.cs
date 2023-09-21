using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface ITranscribeWavFileService
{
    Task<IResult<bool>> Transcribe(YtVideoFileWavId videoFileWavId,CancellationToken token);
}
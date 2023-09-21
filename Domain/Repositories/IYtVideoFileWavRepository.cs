using Domain.Entities;
using Domain.EntityIds;

namespace Domain.Repositories;

public interface IYtVideoFileWavRepository
{
    Task<YtVideoFileWav> GetToTranscription(YtVideoFileWavId videoFileWavId,CancellationToken token);
    
    Task<YtVideoFileWav> GetToLanguageRecognition(YtVideoFileWavId videoFileWavId, CancellationToken token);
}
using Domain.Entities;
using Domain.EntityIds;

namespace Domain.Repositories;

public interface IYtVideoTranscriptionRepository
{
    Task<YtVideoTranscription> GetToVideoDescription(YtVideoTranscriptionId ytVideoTranscriptionId,
        CancellationToken token);
}
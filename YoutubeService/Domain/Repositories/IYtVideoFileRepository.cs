using Domain.Entities;
using Domain.EntityIds;

namespace Domain.Repositories;

public interface IYtVideoFileRepository
{
    Task<YtVideoFile> GetByIdToDownload(YtVideoFileId id, CancellationToken token);

    Task<IEnumerable<YtVideoFileId>> GetAllToDownload(int amount, CancellationToken token);
    Task<YtVideoFile> GetById(YtVideoFileId ytVideoFileId, CancellationToken token);
}
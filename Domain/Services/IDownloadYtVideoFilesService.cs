using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface IDownloadYtVideoFilesService
{
    Task<Result<bool>> Download(YtVideoId ytVideoId, CancellationToken token);
}
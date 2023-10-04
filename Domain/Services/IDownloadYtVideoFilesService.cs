using Domain.EntityIds;
using Domain.Results;

namespace Domain.Services;

public interface IDownloadYtVideoFilesService
{
    Task<IResult<bool>> Download(YtVideoId ytVideoId, CancellationToken token);
}
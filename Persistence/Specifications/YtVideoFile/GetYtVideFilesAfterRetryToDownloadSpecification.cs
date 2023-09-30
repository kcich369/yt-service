using Domain.EntityIds;
using Persistence.Specifications.Base;

namespace Persistence.Specifications.YtVideoFile;

public class
    GetYtVideFilesAfterRetryToDownloadSpecification : SelectedSpecification<Domain.Entities.YtVideoFile, YtVideoFileId>
{
    public GetYtVideFilesAfterRetryToDownloadSpecification(int retriesLimit, DateTimeOffset currentDate, int amount) : base(x =>
            x.Retries >= 1 && x.Retries <= retriesLimit && x.Video.Process,
        f => f.Id)
    {
        AddInclude(x => x.Video);
        Take = amount;
    }
}
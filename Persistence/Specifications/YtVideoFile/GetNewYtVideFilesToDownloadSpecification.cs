using Domain.EntityIds;
using Persistence.Specifications.Base;

namespace Persistence.Specifications.YtVideoFile;

public class
    GetNewYtVideFilesToDownloadSpecification : SelectedSpecification<Domain.Entities.YtVideoFile, YtVideoFileId>
{
    public GetNewYtVideFilesToDownloadSpecification(int amount) : base(
        x => x.Retries == 0 && x.Video.Process,
        f => f.Id)
    {
        AddInclude(x => x.Video);
        Take = amount;
    }
}
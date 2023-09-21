using Domain.Dtos.YtVideo;
using Domain.Entities;
using Persistence.Specifications.Base;

namespace Persistence.Specifications.YtVideos;

public class SearchVideosByNameSelectedSpecification : SelectedSpecification<YtVideo, YtVideoSearchDto>
{
    public SearchVideosByNameSelectedSpecification(string search, int take) : base(x => x.Files.Any(),
        x => new YtVideoSearchDto()
        {
            Name = x.Name,
            YtVideoFileId = x.Id.Value.ToString()
        })
    {
        AddWhereIf(!string.IsNullOrEmpty(search), x => x.Name.Contains(search));
        Take = take;
    }
}
using Domain.Dtos.YtVideo;
using Domain.Dtos.YtVideoFile;
using Domain.Entities;
using Domain.EntityIds;
using Persistence.Specifications.Base;

namespace Persistence.Specifications.YtVideos;

public sealed class GetByIdSelectedSpecification : SelectedSpecification<YtVideo, YtVideoDetailsDto>
{
    public GetByIdSelectedSpecification(YtVideoId id) : base(v => v.Id == id,
        v => new()
        {
            Id = v.Id.ToString(),
            Name = v.Name,
            Files = v.Files.Select(f => new YtVideoFileDto()
            {
                Id = f.Id.ToString(),
                Url = f.PathData.FullValue,
                Quality = f.Quality
            })
        })
    {
    }
}
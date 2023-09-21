using Domain.Dtos;
using Domain.Dtos.YtChannel;
using Domain.EntityIds;
using Persistence.Specifications.Base;

namespace Persistence.Specifications.YtChannel;

public sealed class
    GetWithVideoNamesSelectedSpecification : SelectedSpecification<Domain.Entities.YtChannel, YtChannelVideosDto>
{
    public GetWithVideoNamesSelectedSpecification(YtChannelId ytChannelId) : base(c => c.Id == ytChannelId,
        c => new YtChannelVideosDto(c.Id.ToString(), c.Name, c.YtId,
            c.Videos.Where(x => x.Files.Any()).Select(x => x.Name)))
    {
    }
}
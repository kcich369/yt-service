using Domain.Dtos.YtVideoFile;

namespace Domain.Dtos.YtVideo;

public sealed class YtVideoDetailsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<YtVideoFileDto> Files { get; set; }
}
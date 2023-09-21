
namespace Domain.Dtos.YtVideo;

public sealed class YtVideoSearchResult
{
    public string SearchQuery { get; set; }
    public IEnumerable<YtVideoSearchDto> Results { get; set; }
}
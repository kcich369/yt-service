namespace Domain.Dtos.YtVideo;

public sealed class SearchVideosDto
{
    public string Query { get; set; }
    public int Amount { get; set; }
}
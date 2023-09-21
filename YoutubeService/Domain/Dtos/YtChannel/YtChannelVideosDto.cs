namespace Domain.Dtos.YtChannel;

public sealed record YtChannelVideosDto(string Id, string Name, string YtId, IEnumerable<string> VideosNames);

namespace Application.YtChannel.Commands.Create;

public sealed class CreateYtChannelDto
{
    public bool DownloadByCustomUrl { get; set; }
    public string Name { get; set; }
}
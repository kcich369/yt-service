using Domain.Entities.Base;
using Domain.EntityIds;

namespace Domain.Entities;

public sealed class YtChannel : Entity<YtChannelId>
{
    public string Name { get; private set; }
    public string Handle { get; private set; }
    public string YtId { get; private set; }
    public string Url { get; private set; }

    // relations
    public List<YtVideo> Videos { get; private set; } = new();

    private YtChannel(YtChannelId id, string name, string handle, string ytId, string url)
    {
        Id = id;
        Name = name;
        Handle = handle;
        YtId = ytId;
        Url = url;
    }

    public static YtChannel Create(YtChannelId id, string name, string handle, string ytId, string url)
    {
        return new YtChannel(id, name, handle, ytId, url);
    }

    public YtChannel AddVideos(IEnumerable<YtVideo> videos)
    {
        Videos.AddRange(videos);
        return this;
    }
}
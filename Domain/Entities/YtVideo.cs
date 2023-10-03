using Domain.Auditable;
using Domain.Entities.Base;
using Domain.EntityIds;

namespace Domain.Entities;

public sealed class YtVideo : Entity<YtVideoId>
{
    public string Name { get; private init; }
    public string YtId { get; private init; }
    public string Url { get; private init; }
    public TimeSpan? Duration { get; private set; }
    public bool Process { get;  private set; }
    public string LanguageCulture { get;  private set; }


    //relations
    public YtChannelId ChannelId { get; private set; }
    public YtChannel Channel { get; private set; }
    public List<YtVideoFile> Files { get; private set; } = new();
    public YtVideoDescription Description { get; private set; }
    public YtVideoTag Tag { get; private set; }

    private YtVideo()
    {
    }

    private YtVideo(string name, string ytId, string url, TimeSpan? duration, YtChannelId ytChannelId)
    {
        Id = new YtVideoId();
        Name = name;
        YtId = ytId;
        Url = url;
        ChannelId = ytChannelId;
        Duration = duration;
    }

    public static YtVideo Create(string name, string ytId, string url, TimeSpan? duration, YtChannelId ytChannelId)
    {
        return new YtVideo(name, ytId, url,duration, ytChannelId);
    }

    public YtVideo AddFiles(IEnumerable<YtVideoFile> ytVideFiles)
    {
        Files.AddRange(ytVideFiles);
        return this;
    }
    
    public YtVideo AddFile(YtVideoFile ytVideFile)
    {
        Files.Add(ytVideFile);
        return this;
    }
    
    public YtVideo SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
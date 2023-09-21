using Domain.Abstractions;
using Domain.Auditable;
using Domain.EntityIds;

namespace Domain.Entities;

public sealed class YtVideoTag : Entity<YtVideoTagId>, ICreated
{
    public string Tags { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId CreatedById { get; private set; }
    public string CreatedBy { get; private set; }
    public bool Process { get; private set; }

    //relations
    public YtVideoTranscriptionId YtVideoTranscriptionId { get; set; }
    public YtVideoTranscription YtVideoTranscription { get; set; }

    public YtVideoId YtVideoId { get; set; }
    public YtVideo YtVideo { get; set; }

    private YtVideoTag()
    {
    }

    private YtVideoTag(string tags)
    {
        Tags = tags;
    }

    public static YtVideoTag Create(string tags) => new(tags);

    public void SetCreationData(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedBy = createdBy;
    }

    public YtVideoTag AppendDescription(string tags)
    {
        Tags = tags;
        return this;
    }
    
    public YtVideoTag SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
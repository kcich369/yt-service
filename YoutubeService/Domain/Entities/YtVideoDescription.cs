using Domain.Abstractions;
using Domain.Auditable;
using Domain.EntityIds;

namespace Domain.Entities;

public sealed class YtVideoDescription : Entity<YtVideoDescriptionId>, ICreated
{
    public string Description { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId CreatedById { get; private set; }
    public string CreatedBy { get; private set; }
    public bool Process { get;  private set; }

    // relations
    public YtVideoTranscriptionId YtVideoTranscriptionId { get; set; }
    public YtVideoTranscription YtVideoTranscription { get; set; }

    public YtVideoId YtVideoId { get; set; }
    public YtVideo YtVideo { get; set; }

    private YtVideoDescription()
    {
    }

    private YtVideoDescription(string description)
    {
        Description = description;
    }

    public static YtVideoDescription Create(string description) => new(description);

    public void SetCreationData(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedBy = createdBy;
    }

    public YtVideoDescription AppendDescription(string description)
    {
        Description = description;
        return this;
    }
    
    public YtVideoDescription SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
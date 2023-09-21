using Domain.Abstractions;
using Domain.Auditable;
using Domain.EntityIds;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class YtVideoFileWav : Entity<YtVideoFileWavId>, ICreated
{
    public PathData PathData { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId CreatedById { get; private set; }
    public string CreatedBy { get; private set; }
    public string Language { get; private set; }
    public bool Process { get; private set; }

    // relations
    public YtVideoFileId YtVideoFileId { get; private set; }
    public YtVideoFile YtVideoFile { get; private set; }

    public YtVideoTranscription YtVideoTranscription { get; private set; }

    private YtVideoFileWav()
    {
    }

    private YtVideoFileWav(string mainPath)
    {
        PathData = new PathData(mainPath);
    }

    public static YtVideoFileWav Create(string mainPath) =>
        new(mainPath);

    public void SetCreationData(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedBy = createdBy;
    }

    public YtVideoFileWav AddTranscription(YtVideoTranscription ytVideoTranscription)
    {
        YtVideoTranscription = ytVideoTranscription;
        return this;
    }

    public YtVideoFileWav SetFileName(string fileName)
    {
        PathData.SetFileName(fileName, "wav");
        return this;
    }

    public YtVideoFileWav SetLanguage(string language)
    {
        Language = language;
        return this;
    }
    
    public YtVideoFileWav SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
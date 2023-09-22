using Domain.Auditable;
using Domain.Entities.Base;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class YtVideoFile : Entity<YtVideoFileId>, ICreated, IUpdated
{
    public PathData PathData { get; private set; }
    public string Quality { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId CreatedById { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public UserId? UpdatedById { get; private set; }
    public string UpdatedBy { get; private set; }
    public int Retries { get; private set; }
    public long Bytes { get; private set; }
    public bool Process { get; private set; }


    //relations
    public YtVideoId VideoId { get; private set; }
    public YtVideo Video { get; private set; }
    public YtVideoFileWav WavFile { get; set; }

    private YtVideoFile()
    {
    }

    private YtVideoFile(string pathValue,  VideoQualityEnum quality)
    {
        Id = new YtVideoFileId(Ulid.NewUlid().ToString());
        Quality = quality.Name;
        PathData = new PathData(pathValue);
    }

    public static YtVideoFile Create(string path, VideoQualityEnum quality) =>
        new(path, quality);

    public void SetCreationData(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedBy = createdBy;
    }

    public void SetUpdatedData(DateTimeOffset updatedAt, UserId updatedById, string updatedBy)
    {
        UpdatedAt = updatedAt;
        UpdatedById = updatedById;
        UpdatedBy = updatedBy;
    }

    public YtVideoFile IncreaseRetries()
    {
        Retries += 1;
        return this;
    }

    public YtVideoFile SetFileInfo(string fileName, string fileExtension, long bytes)
    {
        PathData.SetFileName(fileName, fileExtension);
        Bytes = bytes;
        return this;
    }

    public YtVideoFile AddWavFile(YtVideoFileWav videoFileWav)
    {
        WavFile = videoFileWav;
        return this;
    }

    public YtVideoFile SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
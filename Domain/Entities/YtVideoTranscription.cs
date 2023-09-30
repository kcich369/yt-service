﻿using Domain.Auditable;
using Domain.Entities.Base;
using Domain.EntityIds;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class YtVideoTranscription : Entity<YtVideoTranscriptionId>, ICreated
{
    public PathData PathData { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId CreatedById { get; private set; }
    public string CreatedBy { get; private set; }
    public bool Process { get; private set; }

    // relations
    public YtVideoFileWavId WavFileId { get; private set; }
    public YtVideoFileWav WavFile { get; private set; }
    public YtVideoDescription Description { get; private set; }
    public YtVideoTag Tag { get; private set; }

    private YtVideoTranscription()
    {
    }
    
    private YtVideoTranscription(string mainPath)
    {
        PathData = new PathData(mainPath);
    }

    public static YtVideoTranscription Create(string mainPath) =>
        new(mainPath);

    public void SetCreationData(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedBy = createdBy;
    }

    public YtVideoTranscription SetFileName(string fileName)
    {
        PathData.SetFileName(fileName, "txt");
        return this;
    }
    
    public YtVideoTranscription AddData(YtVideoDescription ytVideoDescription, YtVideoTag ytVideoTag)
    {
        Description = ytVideoDescription;
        Tag = ytVideoTag;
        return this;
    }
    
    public YtVideoTranscription SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
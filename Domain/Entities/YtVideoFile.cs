using Domain.Entities.Base;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class YtVideoFile : Entity<YtVideoFileId>
{
    public PathData PathData { get; private set; }
    public Quality Quality { get; private set; }
  
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
        Quality = new Quality(quality);
        PathData = new PathData(pathValue);
    }

    public static YtVideoFile Create(string path, VideoQualityEnum quality) =>
        new(path, quality);

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
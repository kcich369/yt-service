using Domain.Entities.Base;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class YtVideoFileWav : Entity<YtVideoFileWavId>
{
    public PathData PathData { get; private set; }
    public Language Language { get; private set; }
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

    public YtVideoFileWav SetLanguage(SupportedLanguagesEnum languageEnum)
    {
        Language = new Language(languageEnum);
        return this;
    }
    
    public YtVideoFileWav SetProcess(bool process = true)
    {
        Process = process;
        return this;
    }
}
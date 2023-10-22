using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public sealed class HangfireQueuesEnum : Enumeration
{
    // public static HangfireQueuesEnum CreatingChannel = new(1, nameof(CreatingChannel));
    public static HangfireQueuesEnum NewVideos = new(2, nameof(NewVideos).ToLower());
    public static HangfireQueuesEnum Downloading = new(3, nameof(Downloading).ToLower());
    public static HangfireQueuesEnum Converting = new(4, nameof(Converting).ToLower());
    public static HangfireQueuesEnum RecognisingLanguage = new(5, nameof(RecognisingLanguage).ToLower());
    public static HangfireQueuesEnum Transcribing = new(6, nameof(Transcribing).ToLower());
    public static HangfireQueuesEnum TranscriptionOperations = new(7, nameof(TranscriptionOperations).ToLower());
    
    private HangfireQueuesEnum(int id, string name) : base(id, name)
    {
    }
}
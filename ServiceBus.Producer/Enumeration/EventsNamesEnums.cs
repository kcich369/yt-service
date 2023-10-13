namespace ServiceBus.Producer.Enumeration;

public class EventsNamesEnums : Domain.Enumerations.Base.Enumeration
{
    public static readonly EventsNamesEnums ChannelCreated = new(1, nameof(Messages.ChannelCreated));
    public static readonly EventsNamesEnums NewVideoCreated = new(2, nameof(Messages.NewVideoCreated));
    public static readonly EventsNamesEnums VideoDownloaded = new(3, nameof(Messages.VideoDownloaded));
    public static readonly EventsNamesEnums VideoConverted = new(4, nameof(Messages.VideoConverted));
    public static readonly EventsNamesEnums LanguageRecognised = new(5, nameof(Messages.LanguageRecognised));
    public static readonly EventsNamesEnums VideoTranscribed = new(6, nameof(Messages.VideoTranscribed));
    public static readonly EventsNamesEnums VideoDataAdded = new(7, nameof(Messages.VideoDataAdded));

    private EventsNamesEnums(int id, string name) : base(id, name)
    {
    }
}
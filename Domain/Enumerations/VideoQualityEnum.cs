using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public sealed class VideoQualityEnum : Enumeration
{
    public static VideoQualityEnum Low = new(240, nameof(Low));
    public static VideoQualityEnum High = new(1080, nameof(High));
    public static VideoQualityEnum Mp3 = new(720, nameof(Mp3));

    private VideoQualityEnum(int id, string name) : base(id, name)
    {
    }
}
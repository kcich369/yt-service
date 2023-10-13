using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public sealed class VideoQualityEnum : Enumeration
{
    public static VideoQualityEnum Low = new(1, nameof(Low));
    public static VideoQualityEnum High = new(2, nameof(High));
    public static VideoQualityEnum Mp3 = new(3, nameof(Mp3));

    private VideoQualityEnum(int id, string name) : base(id, name)
    {
    }
}
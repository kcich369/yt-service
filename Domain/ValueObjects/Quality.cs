using Domain.Enumerations;

namespace Domain.ValueObjects;

public class Quality
{
    public string Value { get; private set; }

    private Quality()
    {
    }
    
    public Quality(VideoQualityEnum value)
    {
        Value = value.Name;
    }
    
    public static bool operator ==(Quality quality, VideoQualityEnum qualityEnum) => quality!.Value == qualityEnum!.Name;

    public static bool operator !=(Quality quality, VideoQualityEnum qualityEnum) => quality!.Value !=  qualityEnum!.Name;
}
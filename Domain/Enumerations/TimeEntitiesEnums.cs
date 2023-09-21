using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public sealed class TimeEntitiesEnums :Enumeration
{
    public static TimeEntitiesEnums Empty = new(0, nameof(Empty));
    public static TimeEntitiesEnums Minute = new(1, nameof(Minute));
    public static TimeEntitiesEnums Hour = new(2, nameof(Hour));
    public static TimeEntitiesEnums Day = new(3, nameof(Day));
    
    private TimeEntitiesEnums(int id, string name) : base(id, name)
    {
    }
}
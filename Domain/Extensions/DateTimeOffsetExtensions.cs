using Domain.Enumerations;

namespace Domain.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset AddValue(this DateTimeOffset date, int value, TimeEntitiesEnums timeEntitiesEnums)
    {
        if (timeEntitiesEnums == TimeEntitiesEnums.Minute)
            return date.AddMinutes(value);
        if (timeEntitiesEnums == TimeEntitiesEnums.Hour)
            return date.AddHours(value);
        return timeEntitiesEnums == TimeEntitiesEnums.Day
            ? date.AddDays(value)
            : date;
    }
}
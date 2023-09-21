using System.Text.RegularExpressions;
using Domain.Enumerations;

namespace Domain.Extensions;

public static class YtServiceErrorMessageExtensions
{
    private const string PremiersIn = "Premieres in";

    public static bool IsPremiersIn(this string errorMessage, out int value, out TimeEntitiesEnums timeEntitiesEnums)
    {
        if (!errorMessage.Contains(PremiersIn))
        {
            value = 0;
            timeEntitiesEnums = TimeEntitiesEnums.Empty;
            return false;
        }

        value = int.Parse(new Regex(@"\d+").Match(errorMessage.Split(PremiersIn)[1]).Value);
        timeEntitiesEnums = errorMessage.SetTimeEntity();
        return true;
    }

    private static TimeEntitiesEnums SetTimeEntity(this string errorMessage)
    {
        if (errorMessage.Contains(TimeEntitiesEnums.Minute.Name))
            return TimeEntitiesEnums.Minute;
        if (errorMessage.Contains(TimeEntitiesEnums.Hour.Name))
            return TimeEntitiesEnums.Hour;
        return errorMessage.Contains(TimeEntitiesEnums.Day.Name)
            ? TimeEntitiesEnums.Day
            : TimeEntitiesEnums.Empty;
    }
}
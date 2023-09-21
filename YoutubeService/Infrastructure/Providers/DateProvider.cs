using Domain.Providers;

namespace Infrastructure.Providers;

public class DateProvider : IDateProvider
{
    public DateTimeOffset DateTimeNow() => DateTimeOffset.Now;
}
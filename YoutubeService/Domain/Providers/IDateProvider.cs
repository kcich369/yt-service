namespace Domain.Providers;

public interface IDateProvider
{
    DateTimeOffset DateTimeNow();
}
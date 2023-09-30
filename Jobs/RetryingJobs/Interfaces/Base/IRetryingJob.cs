namespace Jobs.RetryingJobs.Interfaces.Base;

public interface IRetryingJob
{
    Task Execute();
}
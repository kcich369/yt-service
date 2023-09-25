namespace Domain.Helpers;

public interface IRedisLockHelper
{
    Task<T> GetData<T>(string key);
    Task<bool> SetData<T>(string key, T data, TimeSpan expiry);
    Task<bool> Exist(string key);
}
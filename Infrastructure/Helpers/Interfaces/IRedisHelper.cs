namespace Infrastructure.Helpers.Interfaces;

internal interface IRedisHelper
{
    Task<T> GetData<T>(string key);
    Task<bool> SetData<T>(string key, T data, TimeSpan expiry);
    Task<bool> Exist(string key);
    Task<bool> Remove(string key);
}
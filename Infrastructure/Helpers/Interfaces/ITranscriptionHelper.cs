namespace Infrastructure.Helpers.Interfaces;

public interface ITxtFileHelper
{
    Task<string> GetContents(string fullPath, CancellationToken token);
    Task<bool> Save(string fullPath, string contents, CancellationToken token);
}
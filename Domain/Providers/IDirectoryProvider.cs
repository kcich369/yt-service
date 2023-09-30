namespace Domain.Providers;

public interface IDirectoryProvider
{
    void CreateDirectoryIfNotExists(string path);
    bool FileExists(string path);
}
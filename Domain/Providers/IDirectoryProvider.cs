namespace Domain.Providers;

public interface IDirectoryProvider
{
    void CreateIfNotExists(string path);
}
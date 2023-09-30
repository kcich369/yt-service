using Domain.Providers;

namespace Infrastructure.Providers;

public sealed class DirectoryProvider : IDirectoryProvider
{
    public void CreateDirectoryIfNotExists(string path)
    {
        if (Directory.Exists(path))
            return;
        Directory.CreateDirectory(path!);
    }

    public bool FileExists(string path) => File.Exists(path);
}
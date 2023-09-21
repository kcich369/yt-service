using Domain.Providers;

namespace Infrastructure.Providers;

public sealed class DirectoryProvider : IDirectoryProvider
{
    public void CreateIfNotExists(string path)
    {
        if (Directory.Exists(path))
            return;
        Directory.CreateDirectory(path!);
    }
}
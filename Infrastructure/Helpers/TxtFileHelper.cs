using Domain.Providers;
using Infrastructure.Helpers.Interfaces;

namespace Infrastructure.Helpers;

internal sealed class TxtFileHelper : ITxtFileHelper
{
    private readonly IPathProvider _pathProvider;

    public TxtFileHelper(IPathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }

    public Task<string> GetContents(string fullPath, CancellationToken token) =>
        File.ReadAllTextAsync(fullPath, token);

    public async Task<bool> Save(string fullPath, string contents, CancellationToken token)
    {
        await File.AppendAllTextAsync(_pathProvider.GetRelativePath(fullPath), contents, token);
        return true;
    }
}
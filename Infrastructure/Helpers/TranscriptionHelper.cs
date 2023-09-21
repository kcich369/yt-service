using Domain.Helpers;

namespace Infrastructure.Helpers;

public sealed class TranscriptionHelper : ITranscriptionHelper
{
    public Task<string> GetTranscription(string fullPath, CancellationToken token) =>
        File.ReadAllTextAsync(fullPath, token);

    public async Task<string> SaveTranscription(string fullPath, string wavFileName, string transcription,
        CancellationToken token)
    {
        var outputPath = GetTranscriptionOutputPath(fullPath, wavFileName);
        await File.AppendAllTextAsync(outputPath, transcription, token);
        return outputPath;
    }

    private static string GetTranscriptionOutputPath(string path, string fileName)
    {
        return path;
    }
}
namespace Domain.Helpers;

public interface ITranscriptionHelper
{
    Task<string> GetTranscription(string fullPath, CancellationToken token);
    Task<string> SaveTranscription(string fullPath, string wavFileName, string transcription, CancellationToken token);
}
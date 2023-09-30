using Domain.Results;

namespace ExternalServices.Interfaces;

public interface ISpeechToTextService
{
    Task<IResult<string>> TranscribeFromWavFile(string path, string language, CancellationToken token);
    Task<IResult<string>> RecogniseLanguageFromWavFile(string path, CancellationToken token);
}
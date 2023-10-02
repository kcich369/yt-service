using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Domain.Results;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace ExternalServices.Services;

public sealed class LanguageRecognitionService : ILanguageRecognitionService
{
    private readonly ISpeechConfigFactory _speechConfigFactory;

    public LanguageRecognitionService(ISpeechConfigFactory speechConfigFactory)
    {
        _speechConfigFactory = speechConfigFactory;
    }

    public async Task<IResult<string>> FromWavFile(string path, CancellationToken token)
    {
        var languageRecognitionResult = await DetectLanguage(path).TryCatch();
        return languageRecognitionResult.IsError
            ? Result<string>.Error(languageRecognitionResult)
            : Result<string>.Success(languageRecognitionResult.Data);
    }

    private async Task<string> DetectLanguage(string path)
    {
        var autoDetectSourceLanguageConfig =
            AutoDetectSourceLanguageConfig.FromLanguages(Enumeration.GetAll<SupportedLanguagesEnum>()
                .Select(x => x.CultureValue).ToArray());

        using var audioConfig = AudioConfig.FromWavFileInput(path);
        using var recognizer = new SpeechRecognizer(
            _speechConfigFactory.Get(),
            autoDetectSourceLanguageConfig,
            audioConfig);

        var speechRecognitionResult = await recognizer.RecognizeOnceAsync();
        var autoDetectSourceLanguageResult =
            AutoDetectSourceLanguageResult.FromResult(speechRecognitionResult);
        return autoDetectSourceLanguageResult.Language;
    }
}
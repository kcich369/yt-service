using System.Text;
using Domain.Results;
using ExternalServices.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace ExternalServices.Services;

public sealed class SpeechToTextServiceService : ISpeechToTextService
{
    public async Task<IResult<string>> TranscribeFromWavFile(string path, string language, CancellationToken token)
    {
        var speechConfig = SpeechConfig.FromSubscription("", "");
        speechConfig.SpeechRecognitionLanguage = language;

        using var audioConfig = AudioConfig.FromWavFileInput(path);
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        var stopRecognition = new TaskCompletionSource<int>();

        var text = new StringBuilder();

        speechRecognizer.Recognized += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                text.Append(e.Result.Text + " ");
            }
        };

        await speechRecognizer.StartContinuousRecognitionAsync();

        Task.WaitAny(new Task[] { stopRecognition.Task }, token);
        return Result<string>.Success(text.ToString());
    }

    public async Task<IResult<string>> RecogniseLanguageFromWavFile(string path, CancellationToken token)
    {
        var speechConfig = SpeechConfig.FromSubscription("", "");

        //ToDO: to the config
        var autoDetectSourceLanguageConfig =
            AutoDetectSourceLanguageConfig.FromLanguages(
                new string[] { "en-US", "en-GB", "de-DE", "pl-PL" });

        using var audioConfig = AudioConfig.FromWavFileInput(path);
        using var recognizer = new SpeechRecognizer(
            speechConfig,
            autoDetectSourceLanguageConfig,
            audioConfig);

        var speechRecognitionResult = await recognizer.RecognizeOnceAsync();
        var autoDetectSourceLanguageResult =
            AutoDetectSourceLanguageResult.FromResult(speechRecognitionResult);
        return Result<string>.Success(autoDetectSourceLanguageResult.Language);
    }
}
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Configurations;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Domain.Results;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace ExternalServices.Services;

public sealed class TranscriptionService : ITranscriptionService
{
    private readonly ISpeechConfigFactory _speechConfigFactory;

    public TranscriptionService(ISpeechConfigFactory speechConfigFactory)
    {
        _speechConfigFactory = speechConfigFactory;
    }

    public async Task<IResult<string>> TranscribeFromWavFile(string path, string language, CancellationToken token)
    {
        var speechConfig = _speechConfigFactory.Get();
        speechConfig.SpeechRecognitionLanguage = language;

        using var audioConfig = AudioConfig.FromWavFileInput(path);
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
        var stopRecognition = new TaskCompletionSource<int>();
        var text = new StringBuilder();
        
        speechRecognizer.Recognized += (s, e) =>
        {
            var a = e.Result.OffsetInTicks;
            var start = new TimeSpan(e.Result.OffsetInTicks);
            var duration = e.Result.Duration;
            var end = start.Add(e.Result.Duration);
            if (e.Result.Reason != ResultReason.RecognizedSpeech) 
                return;
            text.Append($"{start}--{end} /n");
            text.Append(e.Result.Text + "/n");
        };

        await speechRecognizer.StartContinuousRecognitionAsync();
        Task.WaitAny(new Task[] { stopRecognition.Task }, token);

        return Result<string>.Success(text.ToString());
    }
}
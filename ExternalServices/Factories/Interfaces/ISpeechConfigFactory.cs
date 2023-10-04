using Microsoft.CognitiveServices.Speech;

namespace ExternalServices.Factories.Interfaces;

public interface ISpeechConfigFactory
{
    SpeechConfig Get();
}
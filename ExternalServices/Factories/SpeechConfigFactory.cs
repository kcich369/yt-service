using Domain.Configurations;
using ExternalServices.Factories.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace ExternalServices.Factories;

public sealed class SpeechConfigFactory : ISpeechConfigFactory
{
    private readonly AzureServiceConfiguration _configuration;
    private SpeechConfig _speechConfig;

    public SpeechConfigFactory(AzureServiceConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SpeechConfig Get()
    {
        return _speechConfig ??= SpeechConfig.FromSubscription(_configuration.SubscriptionKey, _configuration.Region);
    }
}
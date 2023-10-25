using ExternalServices.Factories;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using ExternalServices.Mappers;
using ExternalServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class DependencyInjection
{
    public static IServiceCollection RegisterExternalServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IYtClientFactory, YtClientFactory>();
        serviceCollection.AddScoped<IYtClientFactory, YtClientFactory>();
        serviceCollection.AddScoped<IChatGptFactory, ChatGptFactory>();
        serviceCollection.AddScoped<ISpeechConfigFactory, SpeechConfigFactory>();
        
        serviceCollection.AddScoped<IYtVideoDataMapper, YtVideoDataMapper>();
        serviceCollection.AddScoped<IYtChannelService, YtChannelService>();
        serviceCollection.AddScoped<IYtChannelVideosService, YtChannelVideosService>();
        serviceCollection.AddScoped<IDownloadYtChannelVideoService, DownloadYtChannelVideoService>();
        serviceCollection.AddScoped<IChatGptService, ChatGptService>();
        serviceCollection.AddScoped<ITranscriptionService, TranscriptionService>();
        serviceCollection.AddScoped<ILanguageRecognitionService, LanguageRecognitionService>();

        return serviceCollection;
    }
}
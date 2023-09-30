using ExternalServices.Factories;
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
        serviceCollection.AddScoped<IChatGptFactory, ChatGptFactory>();
        serviceCollection.AddScoped<IYtVideoDataMapper, YtVideoDataMapper>();
        serviceCollection.AddScoped<IYtService, YtService>();
        serviceCollection.AddScoped<IChatGptService, ChatGptService>();
        serviceCollection.AddScoped<ISpeechToTextService, SpeechToTextServiceService>();

        return serviceCollection;
    }
}
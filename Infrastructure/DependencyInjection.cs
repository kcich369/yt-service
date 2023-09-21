using Domain.Helpers;
using Domain.Providers;
using Domain.Services;
using Infrastructure.Helpers;
using Infrastructure.Mappers;
using Infrastructure.Providers;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDateProvider, DateProvider>();
        serviceCollection.AddSingleton<IPathProvider, PathProvider>();
        serviceCollection.AddSingleton<IDirectoryProvider, DirectoryProvider>();
        serviceCollection.AddSingleton<IConvertFileToWavHelper, ConvertFileToWavHelper>();
        serviceCollection.AddSingleton<ITranscriptionHelper, TranscriptionHelper>();
        serviceCollection.AddSingleton<IYtVideoMapper, YtVideoMapper>();
        serviceCollection.AddSingleton<ITranscriptionHelper, TranscriptionHelper>();
        serviceCollection.AddScoped<IAddChannelVideosService, AddChannelVideosService>();
        serviceCollection.AddScoped<IConvertVideoFileToWavService, ConvertVideoFileToWavService>();
        serviceCollection.AddScoped<ICreateYtChannelWithVideosService, CreateYtChannelService>();
        serviceCollection.AddScoped<IDownloadYtVideoFilesService, DownloadYtVideoFilesService>();
        serviceCollection.AddScoped<IRecogniseLanguageService, RecogniseLanguageService>();
        serviceCollection.AddScoped<ITranscribeWavFileService, TranscribeWavFileService>();
        serviceCollection.AddScoped<ITranscriptionDataService, TranscriptionDataService>();
        
        

        return serviceCollection;
    }
}
﻿using Domain.Configurations;
using Domain.Helpers;
using Domain.Providers;
using Domain.Services;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Infrastructure.Helpers.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Providers;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IDateProvider, DateProvider>();
        serviceCollection.AddSingleton<IPathProvider, PathProvider>();
        serviceCollection.AddSingleton<ITxtFileHelper, TxtFileHelper>();
        serviceCollection.AddSingleton<IYtVideoMapper, YtVideoMapper>();
        serviceCollection.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.ReturnConfigInstance<RedisConfiguration>().ConnectionPort));

        serviceCollection.AddScoped<IRedisHelper, RedisHelper>();
        serviceCollection.AddScoped<IRedisLockHelper, RedisLockHelper>();
        serviceCollection.AddScoped<IDirectoryProvider, DirectoryProvider>();
        serviceCollection.AddScoped<IConvertFileToWavHelper, ConvertFileToWavHelper>();
        serviceCollection.AddScoped<IMessageHelper, MessageHelper>();
        serviceCollection.AddScoped<IAddChannelVideosService, AddChannelVideosService>();
        serviceCollection.AddScoped<IConvertVideoFileToWavService, ConvertVideoFileToWavService>();
        serviceCollection.AddScoped<ICreateYtChannelWithVideosService, CreateYtChannelService>();
        serviceCollection.AddScoped<IDownloadYtVideoFilesService, DownloadYtVideoFilesService>();
        serviceCollection.AddScoped<IRecogniseLanguageService, RecogniseLanguageService>();
        serviceCollection.AddScoped<ITranscribeWavFileService, TranscribeWavFileService>();
        serviceCollection.AddScoped<ITranscriptionDataService, TranscriptionDataService>();
        serviceCollection.AddScoped<ICacheKeysProvider, CacheKeysProvider>();

        return serviceCollection;
    }
}
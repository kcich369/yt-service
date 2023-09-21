using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Interceptors;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection RegisterPersistence(this IServiceCollection serviceCollection,
       string connectionString)
    {
        serviceCollection.AddSingleton<CreatingAuditableEntitiesInterceptor>();
        serviceCollection.AddSingleton<UpdatingAuditableEntitiesInterceptor>();

        serviceCollection.AddDbContext<IAppDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(serviceProvider.GetRequiredService<CreatingAuditableEntitiesInterceptor>(),
                    serviceProvider.GetRequiredService<UpdatingAuditableEntitiesInterceptor>()
                );
        });
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        serviceCollection.AddScoped<IYtChannelRepository, YtChannelRepository>();
        serviceCollection.AddScoped<IYtVideoRepository, YtVideoRepository>();
        serviceCollection.AddScoped<IYtVideoTagsRepository, YtVideoTagsRepository>();
        serviceCollection.AddScoped<IYtVideoTranscriptionRepository, YtVideoTranscriptionRepository>();
        serviceCollection.AddScoped<IYtVideoDescriptionRepository, YtVideoDescriptionRepository>();
        serviceCollection.AddScoped<IYtVideoFileRepository, YtVideoFileRepository>();
        serviceCollection.AddScoped<IYtVideoFileWavRepository, YtVideoFileWavRepository>();
        
        return serviceCollection;
    }
}
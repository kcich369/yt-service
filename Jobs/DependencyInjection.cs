using Domain.Configurations;
using Hangfire;
using Jobs.RetryingJobs;
using Jobs.RetryingJobs.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs;

public static class DependencyInjection
{
    public static IServiceCollection RegisterJobs(this IServiceCollection serviceCollection,
        string hangfireConnectionString, JobsConfiguration config)
    {
        serviceCollection.AddScoped<IChannelCreatedRetryingJob, ChannelCreatedRetryingJob>();
        
        serviceCollection.AddScoped<ILanguageRecognisedRetryingJob, LanguageRecognisedRetryingJob>();
        serviceCollection.AddScoped<INewVideoCreatedRetryingJob, NewVideoCreatedRetryingJob>();
        serviceCollection.AddScoped<IVideoConvertedRetryingJob, VideoConvertedRetryingJob>();
        
        serviceCollection.AddScoped<IVideoDownloadedRetryingJob, VideoDownloadedRetryingJob>();
        serviceCollection.AddScoped<IVideoTranscribedRetryingJob, VideoTranscribedRetryingJob>();
        
        if (config.DisableAll)
            return serviceCollection;
        serviceCollection.AddHangfire(conf => conf
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(hangfireConnectionString));
        serviceCollection.AddHangfireServer();
        
        return serviceCollection;
    }

    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder builder, JobsConfiguration config)
    {
        if (config.DisableAll)
            return builder;
        builder.UseHangfireDashboard(config.HangfireDashboardEndpoint);

        return builder;
    }

    public static IServiceCollection RegisterHangfireJobs(this IServiceCollection serviceCollection,
        JobsConfiguration config)
    {
        if (config.DisableAll)
            return serviceCollection;
        RecurringJob.AddOrUpdate<IChannelCreatedRetryingJob>(nameof(ChannelCreatedRetryingJob),
            (service) => service.Execute(),
            config.JobInterval);
        RecurringJob.AddOrUpdate<ILanguageRecognisedRetryingJob>(nameof(LanguageRecognisedRetryingJob),
            (service) => service.Execute(),
            config.JobInterval);
        RecurringJob.AddOrUpdate<INewVideoCreatedRetryingJob>(nameof(NewVideoCreatedRetryingJob),
            (service) => service.Execute(),
            config.JobInterval);
        RecurringJob.AddOrUpdate<IVideoConvertedRetryingJob>(nameof(VideoConvertedRetryingJob),
            (service) => service.Execute(),
            config.JobInterval);
        RecurringJob.AddOrUpdate<IVideoDownloadedRetryingJob>(nameof(VideoDownloadedRetryingJob),
            (service) => service.Execute(),
            config.JobInterval);
        RecurringJob.AddOrUpdate<IVideoTranscribedRetryingJob>(nameof(VideoTranscribedRetryingJob),
            (service) => service.Execute(),
            config.JobInterval);
        return serviceCollection;
    }
}
using Domain.Configurations;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Infrastructure;

public static class ConfigureLogging
{
    private const string PropertyEnvironment = "Environment";
    private const string AspEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";
    private const string AppSettings = "appsettings";

    public static IHostBuilder UseSerilog(this IHostBuilder builder, IConfiguration configuration)
    {
        var elasticConfiguration = configuration.ReturnConfigInstance<ElasticConfiguration>();
        if (elasticConfiguration.Disabled)
            return builder;
        var environment = Environment.GetEnvironmentVariable(AspEnvironmentVariable);

        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile($"{AppSettings}.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{AppSettings}.{environment}.json", optional: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSinc(elasticConfiguration, environment!))
            .Enrich.WithProperty(PropertyEnvironment, environment!)
            .ReadFrom.Configuration(configurationBuilder)
            .CreateLogger();

        return builder.UseSerilog();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSinc(ElasticConfiguration configuration, string environment)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration.Url))
        {
            AutoRegisterTemplate = true,
            IndexFormat =
                $"yt-service-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
            NumberOfReplicas = 1,
            NumberOfShards = 2
        };
    }
}
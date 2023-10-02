using System.Reflection;
using System.Text.Json.Serialization;
using Application;
using Domain.Configurations;
using ExternalServices;
using Hangfire;
using Infrastructure;
using Infrastructure.Extensions;
using Jobs;
using Persistence;
using Persistence.Extensions;
using Presentation;
using Presentation.Extensions;
using Serilog;
using ServiceBus.Consumer;
using ServiceBus.Producer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.Abstractions.ApiController).Assembly)
    .AddJsonOptions(option =>
        option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var jobConf = builder.Configuration.ReturnConfigInstance<JobsConfiguration>();

builder.Services
    .RegisterAllConfigurations(builder.Configuration)
    .RegisterServiceBusProducer(builder.Configuration.ReturnConfigInstance<AzureServiceConfiguration>())
    .RegisterApplication()
    .RegisterPersistence(builder.GetDatabaseConnectionString())
    .RegisterInfrastructure(builder.Configuration)
    .RegisterPresentation()
    .RegisterExternalServices()
    .RegisterJobs(builder.GetHangfireConnectionString(), jobConf)
    .RegisterServiceBusConsumer(jobConf);

builder.Host
    .UseSerilog(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard(jobConf);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await builder.Services
    .RegisterHangfireJobs(jobConf)
    .MigrateDatabase();

app.Run();


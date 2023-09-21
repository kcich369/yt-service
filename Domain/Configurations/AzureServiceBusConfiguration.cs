using Domain.Configurations.Base;

namespace Domain.Configurations;

public class AzureServiceBusConfiguration : IConfiguration
{
    public string ConnectionString { get; set; }
    public string TopicName { get; set; }
}
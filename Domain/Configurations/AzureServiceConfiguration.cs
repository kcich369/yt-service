using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class AzureServiceConfiguration : IConfiguration
{
    public string ServiceBusConnectionString { get; set; }
    public string TopicName { get; set; }
    public string SubscriptionKey { get; set; }
    public string Region { get; set; }
}
using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class AzureServiceConfiguration: IConfiguration
{
    public string Key { get; set; }
    public string Region { get; set; }
    public Ulid Value { get; set;}
}
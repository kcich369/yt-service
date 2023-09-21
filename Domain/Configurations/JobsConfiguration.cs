using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class JobsConfiguration: IConfiguration
{
    public bool DisableAll { get; set; }
    public bool DisableAllQueueConsumers { get; set; }
    public string JobInterval { get; set; }
    public string HangfireDashboardEndpoint { get; set; }
}
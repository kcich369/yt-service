using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class YtServiceConfiguration : IConfiguration
{
    public string YtUrl { get; set; }
    public string YtUrlUser { get; set; }
}
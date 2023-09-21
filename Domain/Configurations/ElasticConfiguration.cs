using Domain.Configurations.Base;

namespace Domain.Configurations;

public class ElasticConfiguration : IConfiguration
{
    public string Url { get; set; }
    public bool Disabled { get; set; }
}
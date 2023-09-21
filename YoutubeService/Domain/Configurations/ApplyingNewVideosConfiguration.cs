using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class ApplyingNewVideosConfiguration : IConfiguration
{
    public int Amount { get; set; }
}
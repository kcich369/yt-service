using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class VideoFileSavingConfiguration : IConfiguration
{
    public int AddHours { get; set; }
    public int Amount { get; set; }
}
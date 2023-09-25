using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class ChatGptConfiguration : IConfiguration
{
    public string ApiKey { get; set; }
    public string OrganizationId { get; set; }
    public string Role { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
    public int MaxTokens { get; set; }
}
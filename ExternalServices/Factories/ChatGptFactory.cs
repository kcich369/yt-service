using Domain.Configurations;
using ExternalServices.Factories.Interfaces;
using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;

namespace ExternalServices.Factories;

public sealed class ChatGptFactory : IChatGptFactory
{
    private readonly ChatGptConfiguration _configuration;
    private OpenAIClient? _openAiClient;

    public ChatGptFactory(ChatGptConfiguration configuration)
    {
        _configuration = configuration;
    }

    public OpenAIClient Instance() =>
        _openAiClient ??= new OpenAIClient(new OpenAIConfigurations
        {
            ApiKey = _configuration.ApiKey,
            OrganizationId = _configuration.OrganizationId
        });
}
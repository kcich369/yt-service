using Standard.AI.OpenAI.Clients.OpenAIs;

namespace ExternalServices.Factories;

public interface IChatGptFactory
{
    OpenAIClient Instance();
}
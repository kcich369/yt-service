using Standard.AI.OpenAI.Clients.OpenAIs;

namespace ExternalServices.Factories.Interfaces;

public interface IChatGptFactory
{
    OpenAIClient Instance();
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Configurations;
using Domain.Results;
using ExternalServices.Factories;
using ExternalServices.Factories.Interfaces;
using ExternalServices.Interfaces;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ExternalServices.Services;

public sealed class ChatGptService : IChatGptService
{
    private readonly IChatGptFactory _chatGptFactory;
    private readonly ChatGptConfiguration _chatGptConfiguration;
    private readonly List<ChatCompletionMessage> _messages = new List<ChatCompletionMessage>();

    public ChatGptService(IChatGptFactory chatGptFactory, ChatGptConfiguration chatGptConfiguration)
    {
        _chatGptFactory = chatGptFactory;
        _chatGptConfiguration = chatGptConfiguration;
    }

    public async Task<IResult<string>> Ask(string message, CancellationToken token) =>
        await SendChatMessageToString(message).TryCatch();

    private async Task<string> SendChatMessageToString(string message)
    {
        var text = new StringBuilder();

        foreach (var item in await SendChatMessage(message))
        {
            text.Append(item.Content);
        }

        return text.ToString();
    }

    private async Task<ChatCompletionMessage[]> SendChatMessage(string message)
    {
        StackMessages(new ChatCompletionMessage() { Content = message, Role = _chatGptConfiguration.Role });

        var chatCompletion = new ChatCompletion
        {
            Request = new ChatCompletionRequest
            {
                Model = _chatGptConfiguration.Model,
                Messages = _messages.ToArray(),
                Temperature = _chatGptConfiguration.Temperature,
                MaxTokens = _chatGptConfiguration.MaxTokens
            }
        };

        var result = await _chatGptFactory.Instance()
            .ChatCompletions
            .SendChatCompletionAsync(chatCompletion);
        var messages = ToCompletionMessage(result.Response.Choices);
        StackMessages(messages);

        return messages;
    }

    private void StackMessages(params ChatCompletionMessage[] message) => _messages.AddRange(message);

    private static ChatCompletionMessage[] ToCompletionMessage(IEnumerable<ChatCompletionChoice> choices)
        => choices.Select(x => x.Message).ToArray();
}
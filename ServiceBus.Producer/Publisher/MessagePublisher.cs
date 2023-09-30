using System.Text;
using System.Text.Json;
using Domain.Messages;
using Microsoft.Azure.ServiceBus;

namespace ServiceBus.Producer.Publisher;

public sealed class MessagePublisher : IMessagePublisher
{
    private readonly ITopicClient _topicClient;

    public MessagePublisher(ITopicClient topicClient)
    {
        _topicClient = topicClient;
    }

    public async Task Send<T>(T message) where T : IMessage
    {
        var msg = new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
        msg.UserProperties["typeName"] = typeof(T).Name;
        await _topicClient.SendAsync(msg);
    }

    public async Task Send<T>(IEnumerable<T> messages) where T : IMessage
    {
        messages = messages.ToList();
        if (!messages.Any())
            return;
        foreach (var message in messages)
        {
            await Send(message);
        }
    }
}
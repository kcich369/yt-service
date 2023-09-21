using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Messages;

public sealed record ChannelCreated(string ChannelId) : IMessage;
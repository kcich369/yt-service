using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Messages;

public sealed record NewVideoCreated(string YtVideoId) : IMessage;
using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Messages;

public sealed record VideoTranscribed(string YtVideoTranscriptionId) : IMessage;
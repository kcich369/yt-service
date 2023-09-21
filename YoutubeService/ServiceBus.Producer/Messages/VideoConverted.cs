using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Messages;

public sealed record VideoConverted(string WavFileId) : IMessage;

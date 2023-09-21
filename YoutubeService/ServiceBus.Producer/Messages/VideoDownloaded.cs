using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Messages;

public sealed record VideoDownloaded(string YtVideoFileId) : IMessage;
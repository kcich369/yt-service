using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Messages;

public sealed record LanguageRecognised(string YtVideoFileWav) : IMessage;

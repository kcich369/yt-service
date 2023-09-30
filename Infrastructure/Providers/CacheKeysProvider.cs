using Domain.EntityIds.Base;
using Domain.Enumerations.Base;
using Domain.Messages;
using Domain.Providers;
using ServiceBus.Producer.Enumeration;

namespace Infrastructure.Providers;

public sealed class CacheKeysProvider : ICacheKeysProvider
{
    public string GetPreviousMessageKey<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId
    {
        if (message.PreviousId is null)
            return string.Empty;
        var @event = Enumeration.GetByName<EventsNamesEnums>(message.GetType().Name);
        if (@event is null)
            throw new Exception("Wrong message type");
        var previousEvent = Enumeration.GetById<EventsNamesEnums>(@event.Id - 1);
        if (previousEvent is null)
            throw new Exception("Wrong previous message type");
        return $"{previousEvent.Name}-{message.PreviousId}";
    }

    public string GetKey<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId
    {
        var @event = Enumeration.GetByName<EventsNamesEnums>(message.GetType().Name);
        if (@event is null)
            throw new Exception("Wrong message type");
        return $"{@event.Name}-{message.Id}";
    }
}
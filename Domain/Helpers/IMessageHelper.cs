using Domain.EntityIds.Base;
using Domain.Messages;

namespace Domain.Helpers;

public interface IMessageHelper
{
    Task<bool> Delivered<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId;

    Task<bool> PreviousOperationIsFinished<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId;
}
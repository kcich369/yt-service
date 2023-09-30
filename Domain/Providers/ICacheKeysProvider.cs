using Domain.EntityIds.Base;
using Domain.Messages;

namespace Domain.Providers;

public interface ICacheKeysProvider
{
    string GetPreviousMessageKey<TId, TPreviousId>(MessageBase<TId, TPreviousId> previousId)
        where TId : EntityId where TPreviousId : EntityId;

    string GetKey<TId, TPreviousId>(MessageBase<TId, TPreviousId> previousId)
        where TId : EntityId where TPreviousId : EntityId;
}
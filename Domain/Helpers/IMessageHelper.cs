using Domain.EntityIds.Base;
using Domain.Messages;

namespace Domain.Helpers;

public interface IMessageHelper
{
    Task<bool> MessageIsProcessing<T>(MessageBase<T> message) where T : EntityId;
}
using Domain.Entities;

namespace Domain.Repositories;

public interface IYtVideoDescriptionRepository
{
    Task Add(YtVideoDescription ytVideoDescription, CancellationToken token);
}
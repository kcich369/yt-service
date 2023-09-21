using Domain.Entities;

namespace Domain.Repositories;

public interface IYtVideoTagsRepository
{
    Task Add(YtVideoTag ytVideoTag, CancellationToken token);
}
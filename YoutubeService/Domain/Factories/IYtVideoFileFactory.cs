using Domain.Entities;

namespace Domain.Factories;

public interface IYtVideoFileFactory
{
    public IList<YtVideoFile> NewYtVideoFiles( string channelDirectory, string videoDirectory);
}
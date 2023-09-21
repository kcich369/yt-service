using Domain.Entities;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Domain.Factories;

namespace Infrastructure.Factories;

public sealed class YtVideoFileFactory : IYtVideoFileFactory
{
    public IList<YtVideoFile> NewYtVideoFiles(string channelDirectory, string videoDirectory) =>
        Enumeration.GetAll<VideoQualityEnum>().Select(quality =>
                YtVideoFile.Create(string.Empty, quality, channelDirectory, videoDirectory))
            .ToList();
}
using Domain.Entities;
using Domain.EntityIds;
using ExternalServices.Dto;

namespace Infrastructure.Mappers;

public interface IYtVideoMapper
{
    IEnumerable<YtVideo> Map(IEnumerable<YtVideoData> ytVideoData, YtChannelId channelId, string channelName);
}
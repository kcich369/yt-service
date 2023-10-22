using Domain.Enumerations;

namespace ExternalServices.Dto;

public sealed record VideoData(string Url, VideoQualityEnum Quality, string MainPath, string YtId);
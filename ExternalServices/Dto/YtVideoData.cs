using System;

namespace ExternalServices.Dto;

public sealed record YtVideoData(string Name, string YtId, string Url, TimeSpan? Duration);
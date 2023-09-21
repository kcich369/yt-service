namespace ExternalServices.Dto;

public sealed record VideoData(string Url, string Quality, string ChannelDirectoryName, string VideoDirectoryName,
    string? LanguageCulture = null);
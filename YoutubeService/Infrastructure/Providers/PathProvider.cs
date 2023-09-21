﻿using Domain.Configurations;
using Domain.Providers;

namespace Infrastructure.Providers;

public sealed class PathProvider : IPathProvider
{
    private readonly FilesDataConfiguration _filesDataConfiguration;

    public PathProvider(FilesDataConfiguration filesDataConfiguration)
    {
        _filesDataConfiguration = filesDataConfiguration;
    }

    public string GetChannelPath(string channelName) =>
        $"{_filesDataConfiguration.MainPath}{channelName}";

    public string GetVideoDirectoryPath(string channelPath, string ytVideDirectoryName) =>
        $"{GetChannelPath(channelPath)}\\{ytVideDirectoryName}";

    public string GetFileName(string ytVideDirectoryName, string quality, string extension) =>
        $"{ytVideDirectoryName}_{quality}.{extension}";

    public string GetVideoFilePath(string channelName, string ytVideDirectoryName, string quality, string extension) =>
        $"{GetVideoDirectoryPath(channelName, ytVideDirectoryName)}\\{ytVideDirectoryName}_{quality}.{extension}";

    public string GetVideoTranscriptionDirectoryPath(string channelName, string ytVideDirectoryName)=>
        $"{GetVideoDirectoryPath(channelName, ytVideDirectoryName)}\\{_filesDataConfiguration.Transcriptions}";

    public string GetRelativePath(string path) => $"{_filesDataConfiguration.Resource}{path}";
}
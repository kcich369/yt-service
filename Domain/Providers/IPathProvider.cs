namespace Domain.Providers;

public interface IPathProvider
{
    string GetChannelPath(string channelName);
    string GetVideoDirectoryPath(string channelPath, string ytVideDirectoryName);
    string GetFileName(string ytVideoYtId, string quality, string extension);
    string GetVideoFilePath(string mainPath, string fileName, string extension);
    string GetVideoTranscriptionDirectoryPath(string channelName, string ytVideDirectoryName);
    public string GetRelativePath(string path);
}
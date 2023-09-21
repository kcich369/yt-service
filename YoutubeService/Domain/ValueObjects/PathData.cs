namespace Domain.ValueObjects;

public class PathData
{
    public string MainPath { get; }
    public string DirectoryName { get; }
    public string FileName { get; private set; }
    public string FullValue { get; private set; }
    
    public PathData(string mainPath, string directoryName)
    {
        MainPath = mainPath;
        DirectoryName = directoryName;
        FullValue = this.ToString();
    }

    public sealed override string ToString() => $@"{MainPath}\{DirectoryName}{FileName}";

    public PathData SetFileName(string fileName)
    {
        FileName = fileName;
        return this;
    }
}
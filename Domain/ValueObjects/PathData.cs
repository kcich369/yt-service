namespace Domain.ValueObjects;

public class PathData
{
    public string MainPath { get; }
    public string FileName { get; private set; }
    public string FileExtension { get; private set; }
    public string FullValue { get; private set; }

    public PathData(string mainPath)
    {
        MainPath = mainPath;
        FullValue = null;
    }

    public sealed override string ToString() => $@"{MainPath}\{FileName}.{FileExtension}";

    public PathData SetFileName(string fileName, string fileExtension)
    {
        FileName = fileName;
        FileExtension = fileExtension;
        return this;
    }
}
using Domain.ValueObjects;

namespace Infrastructure.Dtos;

public class PathDataDto
{
    public string MainPath { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string FullValue { get; set; }
    
    public PathDataDto(string mainPath, string fileName, string fileExtension)
    {
        MainPath = mainPath;
        FileName = fileName;
        FileExtension = fileExtension;
    }

    public PathDataDto(PathData pathData)
    {
        MainPath = pathData.MainPath;
        FileName = pathData.FileName;
        FileExtension = pathData.FileExtension;
        FullValue = pathData.FullValue;
    }

    public PathDataDto ConvertTo(string newFileExtension)
    {
        FullValue = FullValue.Replace(FileExtension, newFileExtension);
        FileExtension = newFileExtension;
        return this;
    }
}
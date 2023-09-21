using Domain.Configurations.Base;

namespace Domain.Configurations;

public sealed class FilesDataConfiguration : IConfiguration
{
    public string MainPath { get; set; }
    public string Resource { get; set; }
    public string Transcriptions { get; set; }
}
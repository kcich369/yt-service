using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public class FileExtensionsEnum: Enumeration
{
    public static FileExtensionsEnum Mp3 = new(1, nameof(Mp3).ToLower());
    public static FileExtensionsEnum Wav = new(2, nameof(Wav).ToLower());
    public static FileExtensionsEnum Txt = new(3, nameof(Txt).ToLower());
    protected FileExtensionsEnum(int id, string name) : base(id, name)
    {
    }
}
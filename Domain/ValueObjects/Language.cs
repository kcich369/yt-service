using Domain.Enumerations;

namespace Domain.ValueObjects;

public class Language
{
    public string Name { get; private set; }
    public string CultureValue { get; private set; }

    private Language()
    {
    }
    
    public Language(SupportedLanguagesEnum value)
    {
        Name = value.Name;
        CultureValue = value.CultureValue;
    }
    
    public static bool operator ==(Language language, SupportedLanguagesEnum languageEnum) => language!.Name == languageEnum!.Name;

    public static bool operator !=(Language language, SupportedLanguagesEnum languageEnum) => language!.Name !=  languageEnum!.Name;
}
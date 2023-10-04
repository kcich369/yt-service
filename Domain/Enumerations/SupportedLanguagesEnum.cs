using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public class SupportedLanguagesEnum : Enumeration
{
    public string CultureValue { get; private set; }
    public string LanguageName { get; private set; }
    
    public static SupportedLanguagesEnum Unsupported = new(0, nameof(Unsupported), nameof(Unsupported),nameof(Unsupported));
    public static SupportedLanguagesEnum Pl = new(1, nameof(Pl), "pl-Pl","Polish");
    public static SupportedLanguagesEnum Us = new(1, nameof(Us), "en-Us","American English");
    public static SupportedLanguagesEnum Gb = new(1, nameof(Gb), "en-Gb","English");
    public static SupportedLanguagesEnum De = new(1, nameof(De), "de-De","German");
    public static SupportedLanguagesEnum Es = new(1, nameof(Es), "es-ES","Spanish");
    protected SupportedLanguagesEnum(int id, string name, string cultureValue, string languageName) : base(id, name)
    {
        CultureValue = cultureValue;
        LanguageName = languageName;
    }
}
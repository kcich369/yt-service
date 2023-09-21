using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class ConfigurationExtension
{
    private const string Configuration = "Configuration";
    public static T ReturnConfigInstance<T>(this IConfiguration configuration)
    {
        var instance = Activator.CreateInstance(typeof(T));
        configuration.GetSection(typeof(T).Name.GetConfigName()).Bind(instance);
        return (T)instance;
    }

    public static string GetConfigName(this string configTypeName) =>
        configTypeName.Replace(Configuration, string.Empty);
}
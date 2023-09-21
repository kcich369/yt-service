using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Extensions;

public static class RegisterConfigurationExtension
{
    public static IServiceCollection RegisterConfigurations(this IServiceCollection services, IConfiguration config)
    {
        foreach (var configurationType in typeof(Domain.Configurations.Base.IConfiguration).Assembly.GetTypes()
                     .Where(t => typeof(Domain.Configurations.Base.IConfiguration).IsAssignableFrom(t) && t.IsClass))
        {
            var instance = Activator.CreateInstance(configurationType);
            config.GetSection(configurationType.Name.GetConfigName()).Bind(instance);
            services.AddSingleton(configurationType, instance!);
        }

        return services;
    }
}
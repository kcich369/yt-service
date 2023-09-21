using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Extensions;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection RegisterPresentation(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    public static IServiceCollection RegisterAllConfigurations(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection.RegisterConfigurations(configuration);
    }
}
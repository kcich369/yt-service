using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(
            config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        return serviceCollection;
    }
}
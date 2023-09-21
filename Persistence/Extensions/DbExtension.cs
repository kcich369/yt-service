using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Persistence.Extensions;

public static class DbExtension
{
    public static async Task MigrateDatabase(this IServiceCollection serviceCollection)
    {
        using var scope = serviceCollection.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
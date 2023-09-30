using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class ConnectionStringExtensions
{
    public static string GetDatabaseConnectionString(this IConfiguration configuration) =>
        configuration.GetConnectionString("YtServiceDatabase");

    public static string GetHangfireConnectionString(this IConfiguration configuration) =>
        configuration.GetConnectionString("HangfireConnection");
    
    public static string GetDatabaseConnectionString(this WebApplicationBuilder builder) =>
        builder.Configuration.GetConnectionString("YtServiceDatabase");

    public static string GetHangfireConnectionString(this WebApplicationBuilder builder) =>
        builder.Configuration.GetConnectionString("HangfireConnection");
}
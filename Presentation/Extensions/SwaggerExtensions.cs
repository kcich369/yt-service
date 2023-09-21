using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services) =>
        services.AddSwaggerGen(options => Options(options));

    private static SwaggerGenOptions Options(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Yt Service Api - V1",
                Version = "v1"
            }
        );

        options.EnableAnnotations();
        return options;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace swaggerToCode2.providers;

public static class OutputFileProviderExtensions
{
    public static IServiceCollection AddOutputFileProvider(this IServiceCollection services)
    {
        services.AddSingleton<OutputFileProvider, OutputFileProviderImpl>();
        return services;
    }

}
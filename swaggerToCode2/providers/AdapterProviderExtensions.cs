using Microsoft.Extensions.DependencyInjection;
using swaggerToCode2.providers;

namespace SwaggerToCode
{
    public static class AdapterProviderExtensions
    {
        public static IServiceCollection AddAdapterProvider(this IServiceCollection services)
        {
            services.AddSingleton<AdapterProvider, AdapterProviderImpl>();
            return services;
        }
    }
}
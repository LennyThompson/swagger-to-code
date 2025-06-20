using Microsoft.Extensions.DependencyInjection;
using swaggerToCode2.providers;

namespace SwaggerToCode
{
    public static class TypeAdapterProviderExtensions
    {
        public static IServiceCollection AddTypeAdapterProvider(this IServiceCollection services)
        {
            services.AddSingleton<TypeAdapterProvider, TypeAdapterProviderImpl>();
            return services;
        }
    }
}
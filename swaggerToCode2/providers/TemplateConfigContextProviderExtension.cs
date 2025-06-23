using Microsoft.Extensions.DependencyInjection;
using swaggerToCode2.providers;

namespace SwaggerToCode
{
    public static class TemplateConfigContextProviderExtensions
    {
        public static IServiceCollection AddTemplateConfigContextProvider(this IServiceCollection services)
        {
            services.AddSingleton<TemplateConfigContextProvider, TemplateConfigContextProviderImpl>();
            return services;
        }
    }
}
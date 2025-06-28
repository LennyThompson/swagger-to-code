using Microsoft.Extensions.DependencyInjection;
using SwaggerToCode.Services;

namespace SwaggerToCode
{
    public static class TemplateManagerServiceCollectionExtensions
    {
        public static IServiceCollection AddTemplateManagerService(this IServiceCollection services)
        {
            services.AddSingleton<ITemplateManagerService, TemplateManagerService>();
            
            return services;
        }
    }
}
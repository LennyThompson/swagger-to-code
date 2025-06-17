using Microsoft.Extensions.DependencyInjection;
using SwaggerToCode.Services;

namespace SwaggerToCode
{
    public static class TemplateManagerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the OpenAPI document provider service to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddTemplateManagerService(this IServiceCollection services)
        {
            // Register the OpenApiDocumentProvider as a singleton
            services.AddSingleton<ITemplateManagerService, TemplateManagerService>();
            
            return services;
        }
    }
}
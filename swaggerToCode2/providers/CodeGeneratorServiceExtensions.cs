using Microsoft.Extensions.DependencyInjection;

namespace SwaggerToCode
{
    public static class CodeGeneratorServiceExtensions
    {
        /// <summary>
        /// Adds the CodeGeneratorService implementation as a singleton to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCodeGeneratorService(this IServiceCollection services)
        {
            // Register the CodeGeneratorServiceImpl as a singleton
            services.AddSingleton<CodeGeneratorService, CodeGeneratorServiceImpl>();
            
            return services;
        }
    }
}
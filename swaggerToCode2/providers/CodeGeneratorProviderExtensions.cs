using Microsoft.Extensions.DependencyInjection;
using swaggerToCode2.providers;

namespace SwaggerToCode
{
    public static class CodeGeneratorProviderExtensions
    {
        public static IServiceCollection AddCodeGeneratorProvider(this IServiceCollection services)
        {
            services.AddSingleton<CodeGeneratorProvider, CodeGeneratorProviderImpl>();
            return services;
        }
    }
}
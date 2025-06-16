using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;
using SwaggerToCode.Services;

namespace SwaggerToCode
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenerateConfigReader(
            this IServiceCollection services, 
            string configFilePath)
        {
            services.AddSingleton<IConfigurationReader>(sp => 
                new GenerateConfigReader(
                    configFilePath, 
                    sp.GetService<ILogger<GenerateConfigReader>>()));
            
            return services;
        }
    }
}
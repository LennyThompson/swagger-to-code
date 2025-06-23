using Microsoft.Extensions.DependencyInjection;
using swaggerToCode.code_generators;
using swaggerToCode2.providers;

namespace SwaggerToCode
{
    public static class CodeGeneratorExtensions
    {
        public static IServiceCollection AddCodeGenerators(this IServiceCollection services)
        {
            services.AddSingleton<CodeGenerator, OpenApiDocumentCodeGenerator>();
            services.AddSingleton<CodeGenerator, PathCodeGenerator>();
            services.AddSingleton<CodeGenerator, SchemaObjectCodeGenerator>();
            services.AddSingleton<Func<string, CodeGenerator?>>(provider => key =>
            {
                return key switch
                {
                    "open-api-doc" => provider.GetRequiredService<OpenApiDocumentCodeGenerator>(),
                    "path-item" => provider.GetRequiredService<PathCodeGenerator>(),
                    "schema-obj" => provider.GetRequiredService<SchemaObjectCodeGenerator>(),
                    _ => null
                };
            });

            return services;
        }
    }
}
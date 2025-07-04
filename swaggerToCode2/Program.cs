using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenApi.Models;
using SwaggerToCode.Models;
using SwaggerToCode.Services;
using System.IO;
using System;
using System.Threading.Tasks;
using SwaggerToCode;
using swaggerToCode2.providers;

namespace swaggerToCode;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Setup base DI container
        var configOption = new Option<FileInfo?>(
            name: "--config",
            description: "The generate-config.json file to use",
            getDefaultValue: () => new FileInfo("generate-config.json"));

        var rootCommand = new RootCommand("Swagger/OpenAPI to Code Generator")
        {
            configOption
        };

        var outputDirOption = new Option<string>(
            name: "--output",
            description: "The output directory",
            getDefaultValue: () => "output");

        rootCommand.Add(outputDirOption);

        rootCommand.SetHandler(async (FileInfo configFile, string outputDir) =>
        {
            if (configFile == null || !configFile.Exists)
            {
                Console.Error.WriteLine($"Configuration file not found: {configFile?.FullName ?? "null"}");
                return;
            }

            Console.WriteLine($"Using configuration file: {configFile.FullName}");

            // Create a scoped service collection
            var scopedProvider = ConfigureServices(configFile.FullName);

            // Get services
            var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
            var configReader = scopedProvider.GetRequiredService<IConfigurationReader>();
            var openApiProvider = scopedProvider.GetRequiredService<IOpenApiDocumentProvider>();
            CodeGeneratorService generatorService = scopedProvider.GetRequiredService<CodeGeneratorService>();
            
            try
            {
                // Get the configuration
                var generateConfig = configReader.Configuration;
                logger.LogInformation($"Loaded configuration from: {configFile.FullName}");
                logger.LogInformation($"Configuration contains {generateConfig.Templates.Count} templates");

                if (generateConfig.Swagger == null || generateConfig.Swagger.Count == 0)
                {
                    logger.LogWarning("No swagger files specified in the configuration");
                    return;
                }

                // Load all OpenAPI documents from the swagger files in the configuration
                await openApiProvider.LoadDocumentsAsync();

                logger.LogInformation($"Loaded {openApiProvider.Documents.Count} OpenAPI documents");

                // Process the documents with the templates
                generatorService.ProcessOpenApiDocuments(outputDir);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the configuration");
            }

        }, configOption, outputDirOption);

        return await rootCommand.InvokeAsync(args);
    }
    
    private static string GetRootPathForTemplate(TemplateConfig template, GenerateConfig config)
    {
        // Find the root path configuration for this template
        var rootPathConfig = config.RootPaths.FirstOrDefault(rp => rp.Name == template.PathRoot);
        
        if (rootPathConfig == null)
        {
            // Return an empty string if no matching root path is found
            return string.Empty;
        }
        
        return rootPathConfig.Path;
    }
    
    private static ServiceProvider ConfigureServices(string strConfigFilePath)
    {
        var services = new ServiceCollection();

        // Add logging
        services.AddLogging(builder =>
        {
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
            });
            builder.SetMinimumLevel(LogLevel.Information);
        });

            // Add the configuration reader service
            services.AddGenerateConfigReader(strConfigFilePath);

            // Add the OpenAPI conde generation provider services
            services.AddOpenApiDocumentProvider()
                .AddTemplateManagerService()
                .AddTemplateConfigContextProvider()
                .AddCodeGeneratorService()
                .AddTypeAdapterProvider()
                .AddOutputFileProvider()
                .AddCodeGenerators()
                .AddAdapterProvider();

            // Build the service provider
            return services.BuildServiceProvider();
    }
}
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

namespace swaggerToCode;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Setup base DI container
        var serviceProvider = ConfigureServices();

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

        rootCommand.SetHandler(async (configFile, outputDir) =>
        {
            if (configFile == null || !configFile.Exists)
            {
                Console.Error.WriteLine($"Configuration file not found: {configFile?.FullName ?? "null"}");
                return;
            }

            Console.WriteLine($"Using configuration file: {configFile.FullName}");

            // Create a scoped service collection
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

            // Add the configuration rea
            // der service
            services.AddGenerateConfigReader(configFile.FullName);

            // Add the OpenAPI document provider service
            services.AddOpenApiDocumentProvider();

            // Build the service provider
            var scopedProvider = services.BuildServiceProvider();

            // Get services
            var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
            var configReader = scopedProvider.GetRequiredService<IConfigurationReader>();
            var openApiProvider = scopedProvider.GetRequiredService<IOpenApiDocumentProvider>();

            try
            {
                // Get the configuration
                var generateConfig = configReader.GetConfiguration();
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
                await ProcessOpenApiDocuments(openApiProvider, generateConfig, outputDir, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the configuration");
            }

        }, configOption, outputDirOption);

        return await rootCommand.InvokeAsync(args);
    }
    
    private static async Task ProcessOpenApiDocuments(
        IOpenApiDocumentProvider openApiProvider, 
        GenerateConfig config, 
        string outputDirectory,
        ILogger logger)
    {
        // Create output directory if it doesn't exist
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
            logger.LogInformation($"Created output directory: {outputDirectory}");
        }

        // Process each OpenAPI document
        for (int i = 0; i < openApiProvider.Documents.Count; i++)
        {
            var document = openApiProvider.Documents[i];
            var swaggerFileName = config.Swagger[i];
            
            logger.LogInformation($"Processing document {i+1}/{openApiProvider.Documents.Count}: {swaggerFileName}");
            logger.LogInformation($"Document contains {document.Paths.Count} paths and {document.Components.Schemas.Count} schemas");
            
            // Process each template for this document
            foreach (var template in config.Templates)
            {
                if (!template.Use)
                {
                    logger.LogInformation($"Skipping disabled template: {template.Template}");
                    continue;
                }
                
                logger.LogInformation($"Applying template: {template.Template}");
                
                // Get the root path for this template
                string rootPath = GetRootPathForTemplate(template, config);
                string templateOutputPath = Path.Combine(outputDirectory, rootPath, template.Path);
                
                // Create the output directory for this template if it doesn't exist
                if (!Directory.Exists(templateOutputPath))
                {
                    Directory.CreateDirectory(templateOutputPath);
                    logger.LogInformation($"Created template output directory: {templateOutputPath}");
                }

                // TODO: Apply the template to the document
                // This is where you would implement your template processing logic
                // For now, we'll just log that we would process it
                logger.LogInformation($"Would generate {template.GenerateType} code using template {template.Template} in {templateOutputPath}");
                
                // Example of how you might handle different template targets
                switch (template.Target.ToLowerInvariant())
                {
                    case "each":
                        // Process each schema in the document
                        foreach (var schema in document.Components.Schemas)
                        {
                            logger.LogInformation($"Would process schema: {schema.Key}");
                            
                            // Example file naming
                            string outputFileName1 = $"{schema.Key}{template.FileExtension}";
                            string outputFilePath1 = Path.Combine(templateOutputPath, outputFileName1);
                            
                            logger.LogInformation($"Would write to: {outputFilePath1}");
                        }
                        break;
                        
                    case "all":
                        // Process all schemas in one go
                        string outputFileName = $"AllSchemas{template.FileExtension}";
                        string outputFilePath = Path.Combine(templateOutputPath, outputFileName);
                        
                        logger.LogInformation($"Would write all schemas to: {outputFilePath}");
                        break;
                        
                    default:
                        logger.LogWarning($"Unknown template target: {template.Target}");
                        break;
                }
            }
        }
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
    
    private static ServiceProvider ConfigureServices()
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

        return services.BuildServiceProvider();
    }
}
using Microsoft.Extensions.Logging;
using swaggerToCode.code_generators;
using SwaggerToCode.Models;
using SwaggerToCode.Services;
using swaggerToCode2.providers;

namespace SwaggerToCode
{
    // Example usage in a generator class
    public class CodeGeneratorServiceImpl : CodeGeneratorService
    {
        private readonly Func<string, CodeGenerator> _codeGeneratorFactory;

        private readonly IOpenApiDocumentProvider _documentProvider;
        private readonly IConfigurationReader _configService;
        private readonly ITemplateManagerService _templateManager;
        private readonly TemplateConfigContextProvider _templateConfigContextProvider;
        private readonly AdapterProvider _adapterProvider;
        private readonly ILogger<CodeGeneratorServiceImpl> _logger;

        public CodeGeneratorServiceImpl
        (
            IOpenApiDocumentProvider documentProvider,
            IConfigurationReader configService,
            ITemplateManagerService templateManager,
            TemplateConfigContextProvider templateConfigContextProvider,
            Func<string, CodeGenerator> codeGeneratorFactory,
            AdapterProvider adapterProvider,
            ILogger<CodeGeneratorServiceImpl> logger
        )
        {
            _codeGeneratorFactory = codeGeneratorFactory;
            _documentProvider = documentProvider;
            _configService = configService;
            _templateManager = templateManager;
            _templateConfigContextProvider = templateConfigContextProvider;
            _adapterProvider = adapterProvider;
            _logger = logger;
        }

        public bool ProcessOpenApiDocuments(string outputDirectory)
        {
            // Create output directory if it doesn't exist
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
                _logger.LogInformation($"Created output directory: {outputDirectory}");
            }


            // Process each OpenAPI document
            foreach (var document in _documentProvider.Documents)
            {
                _logger.LogInformation($"Processing document {document.SwaggerFile}");
                _logger.LogInformation(
                    $"Document contains {document.Paths.Count} paths and {document.Components.Schemas.Count} schemas");

                foreach (var templateConfig in _configService.Configuration.TemplateConfigs.Where(template => template.Use))
                {
                    _templateConfigContextProvider.SetContext(templateConfig, document);
                    GenerateCode();
                }

            }

            return true;
        }

        private void GenerateCode()
        {
            foreach (var generator in _templateConfigContextProvider.CurrentTemplateConfig.Generators)
            {
                var codeGenerator = _codeGeneratorFactory(generator);
                if (codeGenerator != null)
                {
                    codeGenerator.GenerateAll();
                }
                else
                {
                    _logger.LogError($"Could not match {generator} to any known code generator");
                }
            }
        }
    }
}
using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;
using SwaggerToCode.Services;
using swaggerToCode2.providers;

namespace swaggerToCode.code_generators;

public class SchemaObjectCodeGenerator : CodeGeneratorImpl
{
    private readonly AdapterProvider _adapterProvider;
    public SchemaObjectCodeGenerator
    (
        TemplateConfigContextProvider templateConfigContextProvider, 
        ITemplateManagerService templateManager,
        AdapterProvider adapterProvider,
        OutputFileProvider outputFileProvider,
        ILogger<CodeGeneratorImpl> logger
    )
    : base("schema-obj", templateConfigContextProvider, templateManager, outputFileProvider, logger)
    {
        _adapterProvider = adapterProvider;
    }

    public override bool GenerateAll()
    {
        foreach (var generateSchemaObj in _templateConfigContextProvider.CurrentOpenApiDocument.Components.Schemas.Select(model =>
                     new OpenApiGenerateTarget(model.Key, _templateConfigContextProvider.CurrentOpenApiDocument,
                         _adapterProvider.CreateSchemaObjectAdapter(model.Value))))
        {
            Generate(generateSchemaObj);
        }

        return true;
    }
}
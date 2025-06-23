using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;
using SwaggerToCode.Services;
using swaggerToCode2.providers;

namespace swaggerToCode.code_generators;

public class PathCodeGenerator : CodeGeneratorImpl
{
    private readonly AdapterProvider _adapterProvider;
    public PathCodeGenerator
    (
        TemplateConfigContextProvider templateConfigContextProvider, 
        AdapterProvider adapterProvider,
        OutputFileProvider outputFileProvider,
        ILogger<CodeGeneratorImpl> logger
    )
    : base("path-item", templateConfigContextProvider, outputFileProvider, logger)
    {
        _adapterProvider = adapterProvider;
    }

    public override bool GenerateAll()
    {
        foreach (var generateSchemaObj in _templateConfigContextProvider.CurrentOpenApiDocument.Paths.Select(path =>
                     new OpenApiGenerateTarget(path.Key, _templateConfigContextProvider.CurrentOpenApiDocument,
                         _adapterProvider.CreatePathItemObjectAdapter(path.Value))))
        {
            Generate(generateSchemaObj);
        }

        return true;
    }
}
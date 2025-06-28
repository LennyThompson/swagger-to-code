using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;
using SwaggerToCode.Services;
using swaggerToCode2.providers;

namespace swaggerToCode.code_generators;

public class OpenApiDocumentCodeGenerator : CodeGeneratorImpl
{
    private readonly AdapterProvider _adapterProvider;
    public OpenApiDocumentCodeGenerator
    (
        TemplateConfigContextProvider templateConfigContextProvider, 
        ITemplateManagerService templateManager,
        AdapterProvider adapterProvider,
        OutputFileProvider outputFileProvider,
        ILogger<CodeGeneratorImpl> logger
    )
    : base("open-api-doc", templateConfigContextProvider, templateManager, outputFileProvider, logger)
    {
        _adapterProvider = adapterProvider;
    }

    public override bool GenerateAll()
    {
        var document = _templateConfigContextProvider.CurrentOpenApiDocument;
        GenerateTarget targetOverall =
            new OpenApiGenerateTarget("test", _adapterProvider.CreateOpenApiDocumentAdapter(document));
        return Generate(targetOverall);
    }
}
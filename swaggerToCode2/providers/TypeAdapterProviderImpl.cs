using Microsoft.Extensions.Logging;
using OpenApi.Models;
using SwaggerToCode.Adapters;
using SwaggerToCode.Models;
using SwaggerToCode.Services;

namespace swaggerToCode2.providers;

public class TypeAdapterProviderImpl : TypeAdapterProvider
{
    private readonly IConfigurationReader _configService;
    private readonly ILogger<TypeAdapterProviderImpl> _logger;

    public TypeAdapterProviderImpl
    (
        IConfigurationReader configService, 
        ILogger<TypeAdapterProviderImpl> logger
    )
    {
        _configService = configService;
        _logger = logger;
    }

    public TypeAdapter GetTypeAdapter(TemplateConfig templateConfigFor, string strName, SchemaObject schemaObj)
    {
        switch (templateConfigFor.Target.ToLower())
        {
            case "cpp":
            case "c++":
                return new CppTypeAdapter
                (
                    strName, 
                    schemaObj.Type, 
                    schemaObj.Format,
                    (string strName) => { return new SnakeToCamelCaseAdapter(strName); }, 
                    schemaObj
                );
            default:
                break;
        }
        throw new NotImplementedException();
    }
}
using Microsoft.Extensions.Logging;
using OpenApi.Models;
using SwaggerToCode.Adapters;

namespace swaggerToCode2.providers;

public class AdapterProviderImpl : AdapterProvider
{
    private readonly TypeAdapterProvider _typeAdapterProvider;
    private readonly TemplateConfigContextProvider _templateContextProvider;
    private readonly ILogger<AdapterProviderImpl> _logger;
    
    public AdapterProviderImpl
    (
        TypeAdapterProvider typeAdapterProvider, 
        TemplateConfigContextProvider templateContextProvider, 
        ILogger<AdapterProviderImpl> logger
    )
    {
        _typeAdapterProvider = typeAdapterProvider;
        _templateContextProvider = templateContextProvider;
        _logger = logger;
    }
    
    
    public IOpenApiDocument CreateOpenApiDocumentAdapter(IOpenApiDocument document)
    {
        return new OpenApiDocumentAdapter(document, this);
    }

    public IInfoObject CreateInfoObjectAdapter(IInfoObject info)
    {
        return new InfoObjectAdapter(info, this);
    }

    public IContactObject CreateContactObjectAdapter(IContactObject contact)
    {
        return new ContactObjectAdapter(contact, this);
    }

    public ILicenseObject CreateLicenseObjectAdapter(ILicenseObject license)
    {
        return new LicenseObjectAdapter(license, this);
    }

    public IServerObject CreateServerObjectAdapter(IServerObject server)
    {
        return new ServerObjectAdapter(server, this);
    }

    public IServerVariableObject CreateServerVariableObjectAdapter(IServerVariableObject serverVariable)
    {
        return new ServerVariableObjectAdapter(serverVariable, this);
    }

    public IComponentsObject CreateComponentsObjectAdapter(IComponentsObject components)
    {
        return new ComponentsObjectAdapter(components, this);
    }

    public ISchemaObject CreateSchemaObjectAdapter(ISchemaObject schema)
    {
        return new SchemaObjectAdapter(schema, this, _typeAdapterProvider, _templateContextProvider.CurrentTemplateConfig);
    }

    public ISchemaObjectField CreateSchemaObjectFieldAdapter(ISchemaObjectField field)
    {
        return new SchemaObjectFieldAdapter(field, this, _typeAdapterProvider, _templateContextProvider.CurrentTemplateConfig);
    }

    public IDiscriminatorObject CreateDiscriminatorObjectAdapter(IDiscriminatorObject discriminator)
    {
        return new DiscriminatorObjectAdapter(discriminator, this);
    }

    public IPathItemObject CreatePathItemObjectAdapter(IPathItemObject pathItem)
    {
        return new PathItemObjectAdapter(pathItem, this);
    }

    public IOperationObject CreateOperationObjectAdapter(IOperationObject operation)
    {
        return new OperationObjectAdapter(operation, this);
    }

    public IParameterObject CreateParameterObjectAdapter(IParameterObject parameter)
    {
        return new ParameterObjectAdapter(parameter, this);
    }

    public IRequestBodyObject CreateRequestBodyObjectAdapter(IRequestBodyObject requestBody)
    {
        return new RequestBodyObjectAdapter(requestBody, this);
    }

    public IResponseObject CreateResponseObjectAdapter(IResponseObject response)
    {
        return new ResponseObjectAdapter(response, this);
    }

    public IMediaTypeObject CreateMediaTypeObjectAdapter(IMediaTypeObject mediaType)
    {
        return new MediaTypeObjectAdapter(mediaType, this);
    }

    public IHeaderObject CreateHeaderObjectAdapter(IHeaderObject header)
    {
        return new HeaderObjectAdapter(header, this);
    }

    public IExampleObject CreateExampleObjectAdapter(IExampleObject example)
    {
        return new ExampleObjectAdapter(example, this);
    }

    public ISecuritySchemeObject CreateSecuritySchemeObjectAdapter(ISecuritySchemeObject securityScheme)
    {
        return new SecuritySchemeObjectAdapter(securityScheme, this);
    }

    public IOAuthFlowsObject CreateOAuthFlowsObjectAdapter(IOAuthFlowsObject oAuthFlows)
    {
        return new OAuthFlowsObjectAdapter(oAuthFlows, this);
    }

    public IOAuthFlowObject CreateOAuthFlowObjectAdapter(IOAuthFlowObject oAuthFlow)
    {
        return new OAuthFlowObjectAdapter(oAuthFlow, this);
    }

    public ITagObject CreateTagObjectAdapter(ITagObject tag)
    {
        return new TagObjectAdapter(tag, this);
    }

    public IExternalDocumentationObject CreateExternalDocumentationObjectAdapter(IExternalDocumentationObject externalDocs)
    {
        return new ExternalDocumentationObjectAdapter(externalDocs, this);
    }
}
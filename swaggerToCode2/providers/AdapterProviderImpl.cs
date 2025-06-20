using OpenApi.Models;
using SwaggerToCode.Adapters;

namespace swaggerToCode2.providers;

public class AdapterProviderImpl : AdapterProvider
{
    public IOpenApiDocument CreateOpenApiDocumentAdapter(OpenApiDocument document)
    {
        return new OpenApiDocumentAdapter(document);
    }

    public IInfoObject CreateInfoObjectAdapter(InfoObject info)
    {
        return new InfoObjectAdapter(info);
    }

    public IContactObject CreateContactObjectAdapter(ContactObject contact)
    {
        return new ContactObjectAdapter(contact);
    }

    public ILicenseObject CreateLicenseObjectAdapter(LicenseObject license)
    {
        return new LicenseObjectAdapter(license);
    }

    public IServerObject CreateServerObjectAdapter(ServerObject server)
    {
        return new ServerObjectAdapter(server);
    }

    public IServerVariableObject CreateServerVariableObjectAdapter(ServerVariableObject serverVariable)
    {
        return new ServerVariableObjectAdapter(serverVariable);
    }

    public IComponentsObject CreateComponentsObjectAdapter(ComponentsObject components)
    {
        return new ComponentsObjectAdapter(components);
    }

    public ISchemaObject CreateSchemaObjectAdapter(SchemaObject schema)
    {
        return new SchemaObjectAdapter(schema);
    }

    public ISchemaObjectField CreateSchemaObjectFieldAdapter(SchemaObjectField field)
    {
        return new SchemaObjectFieldAdapter(field);
    }

    public IDiscriminatorObject CreateDiscriminatorObjectAdapter(DiscriminatorObject discriminator)
    {
        return new DiscriminatorObjectAdapter(discriminator);
    }

    public IPathItemObject CreatePathItemObjectAdapter(PathItemObject pathItem)
    {
        return new PathItemObjectAdapter(pathItem);
    }

    public IOperationObject CreateOperationObjectAdapter(OperationObject operation)
    {
        return new OperationObjectAdapter(operation);
    }

    public IParameterObject CreateParameterObjectAdapter(ParameterObject parameter)
    {
        return new ParameterObjectAdapter(parameter);
    }

    public IRequestBodyObject CreateRequestBodyObjectAdapter(RequestBodyObject requestBody)
    {
        return new RequestBodyObjectAdapter(requestBody);
    }

    public IResponseObject CreateResponseObjectAdapter(ResponseObject response)
    {
        return new ResponseObjectAdapter(response);
    }

    public IMediaTypeObject CreateMediaTypeObjectAdapter(MediaTypeObject mediaType)
    {
        return new MediaTypeObjectAdapter(mediaType);
    }

    public IHeaderObject CreateHeaderObjectAdapter(HeaderObject header)
    {
        return new HeaderObjectAdapter(header);
    }

    public IExampleObject CreateExampleObjectAdapter(ExampleObject example)
    {
        return new ExampleObjectAdapter(example);
    }

    public ISecuritySchemeObject CreateSecuritySchemeObjectAdapter(SecuritySchemeObject securityScheme)
    {
        return new SecuritySchemeObjectAdapter(securityScheme);
    }

    public IOAuthFlowsObject CreateOAuthFlowsObjectAdapter(OAuthFlowsObject oAuthFlows)
    {
        return new OAuthFlowsObjectAdapter(oAuthFlows);
    }

    public IOAuthFlowObject CreateOAuthFlowObjectAdapter(OAuthFlowObject oAuthFlow)
    {
        return new OAuthFlowObjectAdapter(oAuthFlow);
    }

    public ITagObject CreateTagObjectAdapter(TagObject tag)
    {
        return new TagObjectAdapter(tag);
    }

    public IExternalDocumentationObject CreateExternalDocumentationObjectAdapter(ExternalDocumentationObject externalDocs)
    {
        return new ExternalDocumentationObjectAdapter(externalDocs);
    }
}
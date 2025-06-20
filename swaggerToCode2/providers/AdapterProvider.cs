using OpenApi.Models;

namespace swaggerToCode2.providers;

public interface AdapterProvider
{
    IOpenApiDocument CreateOpenApiDocumentAdapter(OpenApiDocument document);
    IInfoObject CreateInfoObjectAdapter(InfoObject info);
    IContactObject CreateContactObjectAdapter(ContactObject contact);
    ILicenseObject CreateLicenseObjectAdapter(LicenseObject license);
    IServerObject CreateServerObjectAdapter(ServerObject server);
    IServerVariableObject CreateServerVariableObjectAdapter(ServerVariableObject serverVariable);
    IComponentsObject CreateComponentsObjectAdapter(ComponentsObject components);
    ISchemaObject CreateSchemaObjectAdapter(SchemaObject schema);
    ISchemaObjectField CreateSchemaObjectFieldAdapter(SchemaObjectField field);
    IDiscriminatorObject CreateDiscriminatorObjectAdapter(DiscriminatorObject discriminator);
    IPathItemObject CreatePathItemObjectAdapter(PathItemObject pathItem);
    IOperationObject CreateOperationObjectAdapter(OperationObject operation);
    IParameterObject CreateParameterObjectAdapter(ParameterObject parameter);
    IRequestBodyObject CreateRequestBodyObjectAdapter(RequestBodyObject requestBody);
    IResponseObject CreateResponseObjectAdapter(ResponseObject response);
    IMediaTypeObject CreateMediaTypeObjectAdapter(MediaTypeObject mediaType);
    IHeaderObject CreateHeaderObjectAdapter(HeaderObject header);
    IExampleObject CreateExampleObjectAdapter(ExampleObject example);
    ISecuritySchemeObject CreateSecuritySchemeObjectAdapter(SecuritySchemeObject securityScheme);
    IOAuthFlowsObject CreateOAuthFlowsObjectAdapter(OAuthFlowsObject oAuthFlows);
    IOAuthFlowObject CreateOAuthFlowObjectAdapter(OAuthFlowObject oAuthFlow);
    ITagObject CreateTagObjectAdapter(TagObject tag);
    IExternalDocumentationObject CreateExternalDocumentationObjectAdapter(ExternalDocumentationObject externalDocs);

    // TInterface CreateAdapter<TModel, TInterface>(TModel model) where TInterface : class;
    
}
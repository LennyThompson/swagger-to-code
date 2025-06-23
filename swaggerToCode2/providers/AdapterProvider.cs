using OpenApi.Models;

namespace swaggerToCode2.providers;

public interface AdapterProvider
{
    IOpenApiDocument CreateOpenApiDocumentAdapter(IOpenApiDocument document);
    IInfoObject CreateInfoObjectAdapter(IInfoObject info);
    IContactObject CreateContactObjectAdapter(IContactObject contact);
    ILicenseObject CreateLicenseObjectAdapter(ILicenseObject license);
    IServerObject CreateServerObjectAdapter(IServerObject server);
    IServerVariableObject CreateServerVariableObjectAdapter(IServerVariableObject serverVariable);
    IComponentsObject CreateComponentsObjectAdapter(IComponentsObject components);
    ISchemaObject CreateSchemaObjectAdapter(ISchemaObject schema);
    ISchemaObjectField CreateSchemaObjectFieldAdapter(ISchemaObjectField field);
    IDiscriminatorObject CreateDiscriminatorObjectAdapter(IDiscriminatorObject discriminator);
    IPathItemObject CreatePathItemObjectAdapter(IPathItemObject pathItem);
    IOperationObject CreateOperationObjectAdapter(IOperationObject operation);
    IParameterObject CreateParameterObjectAdapter(IParameterObject parameter);
    IRequestBodyObject CreateRequestBodyObjectAdapter(IRequestBodyObject requestBody);
    IResponseObject CreateResponseObjectAdapter(IResponseObject response);
    IMediaTypeObject CreateMediaTypeObjectAdapter(IMediaTypeObject mediaType);
    IHeaderObject CreateHeaderObjectAdapter(IHeaderObject header);
    IExampleObject CreateExampleObjectAdapter(IExampleObject example);
    ISecuritySchemeObject CreateSecuritySchemeObjectAdapter(ISecuritySchemeObject securityScheme);
    IOAuthFlowsObject CreateOAuthFlowsObjectAdapter(IOAuthFlowsObject oAuthFlows);
    IOAuthFlowObject CreateOAuthFlowObjectAdapter(IOAuthFlowObject oAuthFlow);
    ITagObject CreateTagObjectAdapter(ITagObject tag);
    IExternalDocumentationObject CreateExternalDocumentationObjectAdapter(IExternalDocumentationObject externalDocs);

    // TInterface CreateAdapter<TModel, TInterface>(TModel model) where TInterface : class;
    
}
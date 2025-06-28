using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace OpenApi.Models
{
    public interface IOpenApiDocument
    {
        string OpenApi { get; set; }
        IInfoObject Info { get; set; }
        List<IServerObject> Servers { get; set; }
        Dictionary<string, IPathItemObject> Paths { get; set; }
        IComponentsObject Components { get; set; }
        List<Dictionary<string, List<string>>> Security { get; set; }
        List<ITagObject> Tags { get; set; }
        IExternalDocumentationObject ExternalDocs { get; set; }
        string SwaggerFile { get; set; }
        bool UpdateSchemaReferences();
    }

    public interface IInfoObject
    {
        string Title { get; set; }
        string Description { get; set; }
        string Version { get; set; }
        string TermsOfService { get; set; }
        ContactObject Contact { get; set; }
        LicenseObject License { get; set; }
    }

    public interface IContactObject
    {
        string Name { get; set; }
        string Url { get; set; }
        string Email { get; set; }
    }

    public interface ILicenseObject
    {
        string Name { get; set; }
        string Url { get; set; }
    }

    public interface IServerObject
    {
        string Url { get; set; }
        string Description { get; set; }
        Dictionary<string, ServerVariableObject> Variables { get; set; }
    }

    public interface IServerVariableObject
    {
        List<string> Enum { get; set; }
        string Default { get; set; }
        string Description { get; set; }
    }

    public interface IComponentsObject
    {
        Dictionary<string, SchemaObject> Schemas { get; set; }
        Dictionary<string, ResponseObject> Responses { get; set; }
        Dictionary<string, ParameterObject> Parameters { get; set; }
        Dictionary<string, RequestBodyObject> RequestBodies { get; set; }
        Dictionary<string, SecuritySchemeObject> SecuritySchemes { get; set; }
    }

    public interface ISchemaObject
    {
        string Ref { get; set; }
        string Type { get; set; }
        string Format { get; set; }
        Dictionary<string, SchemaObject> Properties { get; set; }
        SchemaObject Items { get; set; }
        List<string> Required { get; set; }
        List<string> Enum { get; set; }
        List<SchemaObject> AllOf { get; set; }
        List<SchemaObject> OneOf { get; set; }
        List<SchemaObject> AnyOf { get; set; }
        string Description { get; set; }
        object Default { get; set; }
        object AdditionalProperties { get; set; }
        DiscriminatorObject Discriminator { get; set; }
        Dictionary<string, object> VendorExtensions { get; }
        object this[string key] { get; set; }
        string Name { get; set; }
        bool IsReference { get; }
        bool IsSimpleType { get; }
        bool IsArrayType { get; }
        SchemaObject? ReferenceSchemaObject { get; set; }
        List<ISchemaObjectField> Fields { get; }
    }

    public interface ISchemaObjectField
    {
        ISchemaObject Parent { get; }
        string Name { get; }
        ISchemaObject Field { get; }
    }

    public interface IDiscriminatorObject
    {
        string PropertyName { get; set; }
        Dictionary<string, string> Mapping { get; set; }
    }

    public interface IPathItemObject
    {
        string Summary { get; set; }
        string Description { get; set; }
        OperationObject Get { get; set; }
        OperationObject Put { get; set; }
        OperationObject Post { get; set; }
        OperationObject Delete { get; set; }
        List<ParameterObject> Parameters { get; set; }
    }

    public interface IOperationObject
    {
        List<string> Tags { get; set; }
        string Summary { get; set; }
        string Description { get; set; }
        string OperationId { get; set; }
        List<ParameterObject> Parameters { get; set; }
        RequestBodyObject RequestBody { get; set; }
        Dictionary<string, ResponseObject> Responses { get; set; }
        List<Dictionary<string, List<string>>> Security { get; set; }
    }

    public interface IParameterObject
    {
        string Name { get; set; }
        string In { get; set; }
        string Description { get; set; }
        bool Required { get; set; }
        SchemaObject Schema { get; set; }
    }

    public interface IRequestBodyObject
    {
        string Description { get; set; }
        bool Required { get; set; }
        Dictionary<string, MediaTypeObject> Content { get; set; }
    }

    public interface IResponseObject
    {
        string Description { get; set; }
        Dictionary<string, MediaTypeObject> Content { get; set; }
        Dictionary<string, HeaderObject> Headers { get; set; }
    }

    public interface IMediaTypeObject
    {
        SchemaObject Schema { get; set; }
        object Example { get; set; }
        Dictionary<string, ExampleObject> Examples { get; set; }
    }

    public interface IHeaderObject
    {
        string Description { get; set; }
        SchemaObject Schema { get; set; }
    }

    public interface IExampleObject
    {
        string Summary { get; set; }
        string Description { get; set; }
        object Value { get; set; }
        string ExternalValue { get; set; }
    }

    public interface ISecuritySchemeObject
    {
        string Type { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        string In { get; set; }
        string Scheme { get; set; }
        string BearerFormat { get; set; }
        OAuthFlowsObject Flows { get; set; }
    }

    public interface IOAuthFlowsObject
    {
        OAuthFlowObject Implicit { get; set; }
        OAuthFlowObject Password { get; set; }
        OAuthFlowObject ClientCredentials { get; set; }
        OAuthFlowObject AuthorizationCode { get; set; }
    }

    public interface IOAuthFlowObject
    {
        string AuthorizationUrl { get; set; }
        string TokenUrl { get; set; }
        string RefreshUrl { get; set; }
        Dictionary<string, string> Scopes { get; set; }
    }

    public interface ITagObject
    {
        string Name { get; set; }
        string Description { get; set; }
        ExternalDocumentationObject ExternalDocs { get; set; }
    }

    public interface IExternalDocumentationObject
    {
        string Description { get; set; }
        string Url { get; set; }
    }
}
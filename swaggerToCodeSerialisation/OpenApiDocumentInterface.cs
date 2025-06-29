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

    public interface ISchemaObjectFinder
    {
        ISchemaObject? FindSchemaByReference(string strRef);
    }
    public interface IInfoObject
    {
        string Title { get; set; }
        string Description { get; set; }
        string Version { get; set; }
        string TermsOfService { get; set; }
        IContactObject Contact { get; set; }
        ILicenseObject License { get; set; }
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
        Dictionary<string, IServerVariableObject> Variables { get; set; }
    }

    public interface IServerVariableObject
    {
        List<string> Enum { get; set; }
        string Default { get; set; }
        string Description { get; set; }
    }

    public interface IComponentsObject
    {
        Dictionary<string, ISchemaObject> Schemas { get; set; }
        Dictionary<string, IResponseObject> Responses { get; set; }
        Dictionary<string, IParameterObject> Parameters { get; set; }
        Dictionary<string, IRequestBodyObject> RequestBodies { get; set; }
        Dictionary<string, ISecuritySchemeObject> SecuritySchemes { get; set; }
    }

    public interface ISchemaObject
    {
        string Ref { get; set; }
        string Type { get; set; }
        string Format { get; set; }
        Dictionary<string, ISchemaObject>? Properties { get; set; }
        ISchemaObject? Items { get; set; }
        List<string> Required { get; set; }
        List<string> Enum { get; set; }
        List<ISchemaObject>? AllOf { get; set; }
        List<ISchemaObject>? OneOf { get; set; }
        List<ISchemaObject>? AnyOf { get; set; }
        string Description { get; set; }
        object Default { get; set; }
        object AdditionalProperties { get; set; }
        DiscriminatorObject Discriminator { get; set; }
        Dictionary<string, object>? VendorExtensions { get; }
        public object? Example { get; set; }
        public Dictionary<string, IExampleObject>? Examples { get; set; }
        object this[string key] { get; set; }
        string Name { get; set; }
        bool IsReference { get; }
        bool IsSimpleType { get; }
        bool IsArrayType { get; }
        bool IsObjectType { get; }
        ISchemaObject? ReferenceSchemaObject { get; set; }
        List<ISchemaObjectField>? Fields { get; }
        bool UpdateSchemaReferences(ISchemaObjectFinder finder);
        List<ISchemaObject> References { get; }
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
        IOperationObject Get { get; set; }
        IOperationObject Put { get; set; }
        IOperationObject Post { get; set; }
        IOperationObject Delete { get; set; }
        List<IParameterObject> Parameters { get; set; }
    }

    public interface IOperationObject
    {
        List<string> Tags { get; set; }
        string Summary { get; set; }
        string Description { get; set; }
        string OperationId { get; set; }
        List<IParameterObject> Parameters { get; set; }
        IRequestBodyObject RequestBody { get; set; }
        Dictionary<string, IResponseObject> Responses { get; set; }
        List<Dictionary<string, List<string>>> Security { get; set; }
    }

    public interface IParameterObject
    {
        string Name { get; set; }
        string In { get; set; }
        string Description { get; set; }
        bool Required { get; set; }
        ISchemaObject Schema { get; set; }
        public object? Example { get; set; }
        public Dictionary<string, IExampleObject>? Examples { get; set; }
    }

    public interface IRequestBodyObject
    {
        string Description { get; set; }
        bool Required { get; set; }
        Dictionary<string, IMediaTypeObject> Content { get; set; }
    }

    public interface IResponseObject
    {
        string Description { get; set; }
        Dictionary<string, IMediaTypeObject> Content { get; set; }
        Dictionary<string, IHeaderObject> Headers { get; set; }
    }

    public interface IMediaTypeObject
    {
        ISchemaObject Schema { get; set; }
        object Example { get; set; }
        Dictionary<string, IExampleObject> Examples { get; set; }
    }

    public interface IHeaderObject
    {
        string Description { get; set; }
        ISchemaObject Schema { get; set; }
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
        IOAuthFlowsObject Flows { get; set; }
    }

    public interface IOAuthFlowsObject
    {
        IOAuthFlowObject Implicit { get; set; }
        IOAuthFlowObject Password { get; set; }
        IOAuthFlowObject ClientCredentials { get; set; }
        IOAuthFlowObject AuthorizationCode { get; set; }
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
        IExternalDocumentationObject ExternalDocs { get; set; }
    }

    public interface IExternalDocumentationObject
    {
        string Description { get; set; }
        string Url { get; set; }
    }
}
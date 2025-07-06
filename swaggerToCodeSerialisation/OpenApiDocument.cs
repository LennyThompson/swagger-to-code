using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace OpenApi.Models
{
    public class OpenApiDocument : IOpenApiDocument
    {
        [YamlMember(Alias = "openapi")]
        public string OpenApi { get; set; }

        [YamlMember(Alias = "info")]
        public IInfoObject Info { get; set; }

        [YamlMember(Alias = "servers")]
        public List<IServerObject> Servers { get; set; }

        [YamlMember(Alias = "paths")]
        public Dictionary<string, IPathItemObject> Paths { get; set; }

        [YamlMember(Alias = "components")]
        public IComponentsObject Components { get; set; }

        [YamlMember(Alias = "security")]
        public List<Dictionary<string, List<string>>> Security { get; set; }

        [YamlMember(Alias = "tags")]
        public List<ITagObject> Tags { get; set; }

        [YamlMember(Alias = "externalDocs")]
        public IExternalDocumentationObject ExternalDocs { get; set; }

        [YamlIgnore]
        public string SwaggerFile { get; set; }

    }

    public class InfoObject : IInfoObject
    {
        [YamlMember(Alias = "title")]
        public string Title { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "version")]
        public string Version { get; set; }

        [YamlMember(Alias = "termsOfService")]
        public string TermsOfService { get; set; }

        [YamlMember(Alias = "contact")]
        public IContactObject Contact { get; set; }

        [YamlMember(Alias = "license")]
        public ILicenseObject License { get; set; }
    }

    public class ContactObject : IContactObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "url")]
        public string Url { get; set; }

        [YamlMember(Alias = "email")]
        public string Email { get; set; }
    }

    public class LicenseObject : ILicenseObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "url")]
        public string Url { get; set; }
    }

    public class ServerObject : IServerObject
    {
        [YamlMember(Alias = "url")]
        public string Url { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "variables")]
        public Dictionary<string, IServerVariableObject> Variables { get; set; }
    }

    public class ServerVariableObject : IServerVariableObject
    {
        [YamlMember(Alias = "enum")]
        public List<string> Enum { get; set; }

        [YamlMember(Alias = "default")]
        public string Default { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }
    }

    public class ComponentsObject : IComponentsObject
    {
        [YamlMember(Alias = "schemas")]
        public Dictionary<string, ISchemaObject> Schemas { get; set; }

        [YamlMember(Alias = "responses")]
        public Dictionary<string, IResponseObject> Responses { get; set; }

        [YamlMember(Alias = "parameters")]
        public Dictionary<string, IParameterObject> Parameters { get; set; }

        [YamlMember(Alias = "requestBodies")]
        public Dictionary<string, IRequestBodyObject> RequestBodies { get; set; }

        [YamlMember(Alias = "securitySchemes")]
        public Dictionary<string, ISecuritySchemeObject> SecuritySchemes { get; set; }
    }

    public class DiscriminatorObject : IDiscriminatorObject
    {
        [YamlMember(Alias = "propertyName")]
        public string PropertyName { get; set; }

        [YamlMember(Alias = "mapping")]
        public Dictionary<string, string> Mapping { get; set; }
    }
    
    public class MediaTypeObject : IMediaTypeObject
    {
        [YamlMember(Alias = "schema")]
        public ISchemaObject Schema { get; set; }

        [YamlMember(Alias = "example")]
        public object Example { get; set; }

        [YamlMember(Alias = "examples")]
        public Dictionary<string, IExampleObject> Examples { get; set; }
    }

    public class HeaderObject : IHeaderObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "schema")]
        public ISchemaObject Schema { get; set; }
    }

    public class ExampleObject : IExampleObject
    {
        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "value")]
        public object Value { get; set; }

        [YamlMember(Alias = "externalValue")]
        public string ExternalValue { get; set; }
    }

    public class SecuritySchemeObject : ISecuritySchemeObject
    {
        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "in")]
        public string In { get; set; }

        [YamlMember(Alias = "scheme")]
        public string Scheme { get; set; }

        [YamlMember(Alias = "bearerFormat")]
        public string BearerFormat { get; set; }

        [YamlMember(Alias = "flows")]
        public IOAuthFlowsObject Flows { get; set; }
    }

    public class OAuthFlowsObject : IOAuthFlowsObject
    {
        [YamlMember(Alias = "implicit")]
        public IOAuthFlowObject Implicit { get; set; }

        [YamlMember(Alias = "password")]
        public IOAuthFlowObject Password { get; set; }

        [YamlMember(Alias = "clientCredentials")]
        public IOAuthFlowObject ClientCredentials { get; set; }

        [YamlMember(Alias = "authorizationCode")]
        public IOAuthFlowObject AuthorizationCode { get; set; }
    }

    public class OAuthFlowObject : IOAuthFlowObject
    {
        [YamlMember(Alias = "authorizationUrl")]
        public string AuthorizationUrl { get; set; }

        [YamlMember(Alias = "tokenUrl")]
        public string TokenUrl { get; set; }

        [YamlMember(Alias = "refreshUrl")]
        public string RefreshUrl { get; set; }

        [YamlMember(Alias = "scopes")]
        public Dictionary<string, string> Scopes { get; set; }
    }

    public class TagObject : ITagObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "externalDocs")]
        public IExternalDocumentationObject ExternalDocs { get; set; }
    }

    public class ExternalDocumentationObject : IExternalDocumentationObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "url")]
        public string Url { get; set; }
    }
}
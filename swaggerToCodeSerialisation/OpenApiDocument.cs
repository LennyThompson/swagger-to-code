using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace OpenApi.Models
{
    public class OpenApiDocument
    {
        [YamlMember(Alias = "openapi")]
        public string OpenApi { get; set; }

        [YamlMember(Alias = "info")]
        public InfoObject Info { get; set; }

        [YamlMember(Alias = "servers")]
        public List<ServerObject> Servers { get; set; }

        [YamlMember(Alias = "paths")]
        public Dictionary<string, PathItemObject> Paths { get; set; }

        [YamlMember(Alias = "components")]
        public ComponentsObject Components { get; set; }

        [YamlMember(Alias = "security")]
        public List<Dictionary<string, List<string>>> Security { get; set; }

        [YamlMember(Alias = "tags")]
        public List<TagObject> Tags { get; set; }

        [YamlMember(Alias = "externalDocs")]
        public ExternalDocumentationObject ExternalDocs { get; set; }

        public bool UpdateSchemaReferences()
        {
            if (Paths == null || Components?.Schemas == null)
            {
                return false;
            }
            
            foreach(KeyValuePair<string, SchemaObject> schemaEntry in Components.Schemas)
            {
                if (!schemaEntry.Value.IsReference)
                {
                    schemaEntry.Value.Name = schemaEntry.Key;
                }
            }

            // Get all schema references from paths using LINQ
            var schemaReferences = Paths.Values
                .SelectMany(path => new[]
                {
                    // Path parameters
                    path.Parameters?.SelectMany(p => new[] { p.Schema }),
                    
                    // Operations (GET, POST, PUT, DELETE)
                    new[] { path.Get, path.Post, path.Put, path.Delete }
                        .Where(op => op != null)
                        .SelectMany(op => new[]
                        {
                            // Operation parameters
                            op.Parameters?.SelectMany(p => new[] { p.Schema }),
                            
                            // Request body schemas
                            op.RequestBody?.Content?.Values
                                .Select(c => c.Schema),
                                
                            // Response schemas
                            op.Responses?.Values
                                .SelectMany(r => r.Content?.Values
                                    .Select(c => c.Schema) ?? Array.Empty<SchemaObject>())
                        })
                        .SelectMany(x => x ?? Array.Empty<SchemaObject>())
                })
                .SelectMany(x => x ?? Array.Empty<SchemaObject>())
                .Where(schema => schema != null && !string.IsNullOrEmpty(schema.Ref));

            // Update each schema reference
            schemaReferences
                .ToList()
                .ForEach(schemaRef =>
                {
                    var schemaName = schemaRef.Ref.Split('/').Last();
                    if (Components.Schemas.TryGetValue(schemaName, out var referencedSchema))
                    {
                        schemaRef.ReferenceSchemaObject = referencedSchema;
                    }
                });
            return true;
        }
    }
    
    

    public class InfoObject
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
        public ContactObject Contact { get; set; }

        [YamlMember(Alias = "license")]
        public LicenseObject License { get; set; }
    }

    public class ContactObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "url")]
        public string Url { get; set; }

        [YamlMember(Alias = "email")]
        public string Email { get; set; }
    }

    public class LicenseObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "url")]
        public string Url { get; set; }
    }

    public class ServerObject
    {
        [YamlMember(Alias = "url")]
        public string Url { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "variables")]
        public Dictionary<string, ServerVariableObject> Variables { get; set; }
    }

    public class ServerVariableObject
    {
        [YamlMember(Alias = "enum")]
        public List<string> Enum { get; set; }

        [YamlMember(Alias = "default")]
        public string Default { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }
    }

    public class ComponentsObject
    {
        [YamlMember(Alias = "schemas")]
        public Dictionary<string, SchemaObject> Schemas { get; set; }

        [YamlMember(Alias = "responses")]
        public Dictionary<string, ResponseObject> Responses { get; set; }

        [YamlMember(Alias = "parameters")]
        public Dictionary<string, ParameterObject> Parameters { get; set; }

        [YamlMember(Alias = "requestBodies")]
        public Dictionary<string, RequestBodyObject> RequestBodies { get; set; }

        [YamlMember(Alias = "securitySchemes")]
        public Dictionary<string, SecuritySchemeObject> SecuritySchemes { get; set; }
    }

    public class SchemaObject
    {
        private readonly Dictionary<string, object> _vendorExtensions = new();

        [YamlMember(Alias = "$ref")]
        public string Ref { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "format")]
        public string Format { get; set; }

        [YamlMember(Alias = "properties")]
        public Dictionary<string, SchemaObject> Properties { get; set; }

        [YamlMember(Alias = "items")]
        public SchemaObject Items { get; set; }

        [YamlMember(Alias = "required")]
        public List<string> Required { get; set; }

        [YamlMember(Alias = "enum")]
        public List<string> Enum { get; set; }

        [YamlMember(Alias = "allOf")]
        public List<SchemaObject> AllOf { get; set; }

        [YamlMember(Alias = "oneOf")]
        public List<SchemaObject> OneOf { get; set; }

        [YamlMember(Alias = "anyOf")]
        public List<SchemaObject> AnyOf { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "default")]
        public object Default { get; set; }

        [YamlMember(Alias = "additionalProperties")]
        public object AdditionalProperties { get; set; }
        
        [YamlMember(Alias = "discriminator")]
        public DiscriminatorObject Discriminator { get; set; }
        
        [YamlIgnore] public Dictionary<string, object> VendorExtensions => _vendorExtensions;

        [YamlIgnore] public object this[string key]
        {
            get => _vendorExtensions.TryGetValue(key, out var value) ? value : null;
            set
            {
                if (key.StartsWith("x-"))
                {
                    _vendorExtensions[key] = value;
                }
            }
        }

        [YamlIgnore] public string Name { get; set; }
        [YamlIgnore] public bool IsReference => ReferenceSchemaObject != null || !string.IsNullOrEmpty(Ref);
        [YamlIgnore] public bool IsSimpleType => Type == "string" || Type == "number" || Type == "integer" || Type == "boolean";
        [YamlIgnore] public SchemaObject? ReferenceSchemaObject { get; set; }
    }

    public class DiscriminatorObject
    {
        [YamlMember(Alias = "propertyName")]
        public string PropertyName { get; set; }

        [YamlMember(Alias = "mapping")]
        public Dictionary<string, string> Mapping { get; set; }
    }
    
    public class PathItemObject
    {
        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "get")]
        public OperationObject Get { get; set; }

        [YamlMember(Alias = "put")]
        public OperationObject Put { get; set; }

        [YamlMember(Alias = "post")]
        public OperationObject Post { get; set; }

        [YamlMember(Alias = "delete")]
        public OperationObject Delete { get; set; }

        [YamlMember(Alias = "parameters")]
        public List<ParameterObject> Parameters { get; set; }
    }

    public class OperationObject
    {
        [YamlMember(Alias = "tags")]
        public List<string> Tags { get; set; }

        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "operationId")]
        public string OperationId { get; set; }

        [YamlMember(Alias = "parameters")]
        public List<ParameterObject> Parameters { get; set; }

        [YamlMember(Alias = "requestBody")]
        public RequestBodyObject RequestBody { get; set; }

        [YamlMember(Alias = "responses")]
        public Dictionary<string, ResponseObject> Responses { get; set; }

        [YamlMember(Alias = "security")]
        public List<Dictionary<string, List<string>>> Security { get; set; }
    }

    public class ParameterObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "in")]
        public string In { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "required")]
        public bool Required { get; set; }

        [YamlMember(Alias = "schema")]
        public SchemaObject Schema { get; set; }
    }

    public class RequestBodyObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "required")]
        public bool Required { get; set; }

        [YamlMember(Alias = "content")]
        public Dictionary<string, MediaTypeObject> Content { get; set; }
    }

    public class ResponseObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "content")]
        public Dictionary<string, MediaTypeObject> Content { get; set; }

        [YamlMember(Alias = "headers")]
        public Dictionary<string, HeaderObject> Headers { get; set; }
    }

    public class MediaTypeObject
    {
        [YamlMember(Alias = "schema")]
        public SchemaObject Schema { get; set; }

        [YamlMember(Alias = "example")]
        public object Example { get; set; }

        [YamlMember(Alias = "examples")]
        public Dictionary<string, ExampleObject> Examples { get; set; }
    }

    public class HeaderObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "schema")]
        public SchemaObject Schema { get; set; }
    }

    public class ExampleObject
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

    public class SecuritySchemeObject
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
        public OAuthFlowsObject Flows { get; set; }
    }

    public class OAuthFlowsObject
    {
        [YamlMember(Alias = "implicit")]
        public OAuthFlowObject Implicit { get; set; }

        [YamlMember(Alias = "password")]
        public OAuthFlowObject Password { get; set; }

        [YamlMember(Alias = "clientCredentials")]
        public OAuthFlowObject ClientCredentials { get; set; }

        [YamlMember(Alias = "authorizationCode")]
        public OAuthFlowObject AuthorizationCode { get; set; }
    }

    public class OAuthFlowObject
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

    public class TagObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "externalDocs")]
        public ExternalDocumentationObject ExternalDocs { get; set; }
    }

    public class ExternalDocumentationObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "url")]
        public string Url { get; set; }
    }
}
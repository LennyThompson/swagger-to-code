using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using OpenApi.Models;

namespace SwaggerToCode.Adapters
{
    public class OpenApiDocumentAdapter : IOpenApiDocument
    {
        private readonly OpenApiDocument _document;

        public OpenApiDocumentAdapter(OpenApiDocument document)
        {
            _document = document;
        }

        public string OpenApi 
        { 
            get => _document.OpenApi; 
            set => _document.OpenApi = value; 
        }

        public IInfoObject Info 
        { 
            get => _document.Info; 
            set => _document.Info = value; 
        }

        public List<IServerObject> Servers 
        { 
            get => _document.Servers; 
            set => _document.Servers = value; 
        }

        public Dictionary<string, IPathItemObject> Paths 
        { 
            get => _document.Paths; 
            set => _document.Paths = value; 
        }

        public IComponentsObject Components 
        { 
            get => _document.Components; 
            set => _document.Components = value; 
        }

        public List<Dictionary<string, List<string>>> Security 
        { 
            get => _document.Security; 
            set => _document.Security = value; 
        }

        public List<ITagObject> Tags 
        { 
            get => _document.Tags; 
            set => _document.Tags = value; 
        }

        public IExternalDocumentationObject ExternalDocs 
        { 
            get => _document.ExternalDocs; 
            set => _document.ExternalDocs = value; 
        }

        public string SwaggerFile 
        { 
            get => _document.SwaggerFile; 
            set => _document.SwaggerFile = value; 
        }

        public bool UpdateSchemaReferences()
        {
            return _document.UpdateSchemaReferences();
        }
    }

    public class InfoObjectAdapter : IInfoObject
    {
        private readonly InfoObject _info;

        public InfoObjectAdapter(InfoObject info)
        {
            _info = info;
        }

        public string Title 
        { 
            get => _info.Title; 
            set => _info.Title = value; 
        }

        public string Description 
        { 
            get => _info.Description; 
            set => _info.Description = value; 
        }

        public string Version 
        { 
            get => _info.Version; 
            set => _info.Version = value; 
        }

        public string TermsOfService 
        { 
            get => _info.TermsOfService; 
            set => _info.TermsOfService = value; 
        }

        public ContactObject Contact 
        { 
            get => _info.Contact; 
            set => _info.Contact = value; 
        }

        public LicenseObject License 
        { 
            get => _info.License; 
            set => _info.License = value; 
        }
    }

    public class ContactObjectAdapter : IContactObject
    {
        private readonly ContactObject _contact;

        public ContactObjectAdapter(ContactObject contact)
        {
            _contact = contact;
        }

        public string Name 
        { 
            get => _contact.Name; 
            set => _contact.Name = value; 
        }

        public string Url 
        { 
            get => _contact.Url; 
            set => _contact.Url = value; 
        }

        public string Email 
        { 
            get => _contact.Email; 
            set => _contact.Email = value; 
        }
    }

    public class LicenseObjectAdapter : ILicenseObject
    {
        private readonly LicenseObject _license;

        public LicenseObjectAdapter(LicenseObject license)
        {
            _license = license;
        }

        public string Name 
        { 
            get => _license.Name; 
            set => _license.Name = value; 
        }

        public string Url 
        { 
            get => _license.Url; 
            set => _license.Url = value; 
        }
    }

    public class ServerObjectAdapter : IServerObject
    {
        private readonly ServerObject _server;

        public ServerObjectAdapter(ServerObject server)
        {
            _server = server;
        }

        public string Url 
        { 
            get => _server.Url; 
            set => _server.Url = value; 
        }

        public string Description 
        { 
            get => _server.Description; 
            set => _server.Description = value; 
        }

        public Dictionary<string, ServerVariableObject> Variables 
        { 
            get => _server.Variables; 
            set => _server.Variables = value; 
        }
    }

    public class ServerVariableObjectAdapter : IServerVariableObject
    {
        private readonly ServerVariableObject _serverVariable;

        public ServerVariableObjectAdapter(ServerVariableObject serverVariable)
        {
            _serverVariable = serverVariable;
        }

        public List<string> Enum 
        { 
            get => _serverVariable.Enum; 
            set => _serverVariable.Enum = value; 
        }

        public string Default 
        { 
            get => _serverVariable.Default; 
            set => _serverVariable.Default = value; 
        }

        public string Description 
        { 
            get => _serverVariable.Description; 
            set => _serverVariable.Description = value; 
        }
    }

    public class ComponentsObjectAdapter : IComponentsObject
    {
        private readonly ComponentsObject _components;

        public ComponentsObjectAdapter(ComponentsObject components)
        {
            _components = components;
        }

        public Dictionary<string, SchemaObject> Schemas 
        { 
            get => _components.Schemas; 
            set => _components.Schemas = value; 
        }

        public Dictionary<string, ResponseObject> Responses 
        { 
            get => _components.Responses; 
            set => _components.Responses = value; 
        }

        public Dictionary<string, ParameterObject> Parameters 
        { 
            get => _components.Parameters; 
            set => _components.Parameters = value; 
        }

        public Dictionary<string, RequestBodyObject> RequestBodies 
        { 
            get => _components.RequestBodies; 
            set => _components.RequestBodies = value; 
        }

        public Dictionary<string, SecuritySchemeObject> SecuritySchemes 
        { 
            get => _components.SecuritySchemes; 
            set => _components.SecuritySchemes = value; 
        }
    }

    public class SchemaObjectAdapter : ISchemaObject
    {
        private readonly SchemaObject _schema;

        public SchemaObjectAdapter(SchemaObject schema)
        {
            _schema = schema;
        }

        public string Ref 
        { 
            get => _schema.Ref; 
            set => _schema.Ref = value; 
        }

        public string Type 
        { 
            get => _schema.Type; 
            set => _schema.Type = value; 
        }

        public string Format 
        { 
            get => _schema.Format; 
            set => _schema.Format = value; 
        }

        public Dictionary<string, SchemaObject> Properties 
        { 
            get => _schema.Properties; 
            set => _schema.Properties = value; 
        }

        public SchemaObject Items 
        { 
            get => _schema.Items; 
            set => _schema.Items = value; 
        }

        public List<string> Required 
        { 
            get => _schema.Required; 
            set => _schema.Required = value; 
        }

        public List<string> Enum 
        { 
            get => _schema.Enum; 
            set => _schema.Enum = value; 
        }

        public List<SchemaObject> AllOf 
        { 
            get => _schema.AllOf; 
            set => _schema.AllOf = value; 
        }

        public List<SchemaObject> OneOf 
        { 
            get => _schema.OneOf; 
            set => _schema.OneOf = value; 
        }

        public List<SchemaObject> AnyOf 
        { 
            get => _schema.AnyOf; 
            set => _schema.AnyOf = value; 
        }

        public string Description 
        { 
            get => _schema.Description; 
            set => _schema.Description = value; 
        }

        public object Default 
        { 
            get => _schema.Default; 
            set => _schema.Default = value; 
        }

        public object AdditionalProperties 
        { 
            get => _schema.AdditionalProperties; 
            set => _schema.AdditionalProperties = value; 
        }

        public DiscriminatorObject Discriminator 
        { 
            get => _schema.Discriminator; 
            set => _schema.Discriminator = value; 
        }

        public Dictionary<string, object> VendorExtensions => _schema.VendorExtensions;

        public object this[string key] 
        { 
            get => _schema[key]; 
            set => _schema[key] = value; 
        }

        public string Name 
        { 
            get => _schema.Name; 
            set => _schema.Name = value; 
        }

        public bool IsReference => _schema.IsReference;

        public bool IsSimpleType => _schema.IsSimpleType;

        public SchemaObject? ReferenceSchemaObject 
        { 
            get => _schema.ReferenceSchemaObject; 
            set => _schema.ReferenceSchemaObject = value; 
        }

        public List<SchemaObjectField> Fields => _schema.Fields;
    }

    public class SchemaObjectFieldAdapter : ISchemaObjectField
    {
        private readonly SchemaObjectField _field;

        public SchemaObjectFieldAdapter(SchemaObjectField field)
        {
            _field = field;
        }

        public SchemaObject Parent => _field.Parent;

        public string Name => _field.Name;

        public SchemaObject Field => _field.Field;
    }

    public class DiscriminatorObjectAdapter : IDiscriminatorObject
    {
        private readonly DiscriminatorObject _discriminator;

        public DiscriminatorObjectAdapter(DiscriminatorObject discriminator)
        {
            _discriminator = discriminator;
        }

        public string PropertyName 
        { 
            get => _discriminator.PropertyName; 
            set => _discriminator.PropertyName = value; 
        }

        public Dictionary<string, string> Mapping 
        { 
            get => _discriminator.Mapping; 
            set => _discriminator.Mapping = value; 
        }
    }

    public class PathItemObjectAdapter : IPathItemObject
    {
        private readonly PathItemObject _pathItem;

        public PathItemObjectAdapter(PathItemObject pathItem)
        {
            _pathItem = pathItem;
        }

        public string Summary 
        { 
            get => _pathItem.Summary; 
            set => _pathItem.Summary = value; 
        }

        public string Description 
        { 
            get => _pathItem.Description; 
            set => _pathItem.Description = value; 
        }

        public OperationObject Get 
        { 
            get => _pathItem.Get; 
            set => _pathItem.Get = value; 
        }

        public OperationObject Put 
        { 
            get => _pathItem.Put; 
            set => _pathItem.Put = value; 
        }

        public OperationObject Post 
        { 
            get => _pathItem.Post; 
            set => _pathItem.Post = value; 
        }

        public OperationObject Delete 
        { 
            get => _pathItem.Delete; 
            set => _pathItem.Delete = value; 
        }

        public List<ParameterObject> Parameters 
        { 
            get => _pathItem.Parameters; 
            set => _pathItem.Parameters = value; 
        }
    }

    public class OperationObjectAdapter : IOperationObject
    {
        private readonly OperationObject _operation;

        public OperationObjectAdapter(OperationObject operation)
        {
            _operation = operation;
        }

        public List<string> Tags 
        { 
            get => _operation.Tags; 
            set => _operation.Tags = value; 
        }

        public string Summary 
        { 
            get => _operation.Summary; 
            set => _operation.Summary = value; 
        }

        public string Description 
        { 
            get => _operation.Description; 
            set => _operation.Description = value; 
        }

        public string OperationId 
        { 
            get => _operation.OperationId; 
            set => _operation.OperationId = value; 
        }

        public List<ParameterObject> Parameters 
        { 
            get => _operation.Parameters; 
            set => _operation.Parameters = value; 
        }

        public RequestBodyObject RequestBody 
        { 
            get => _operation.RequestBody; 
            set => _operation.RequestBody = value; 
        }

        public Dictionary<string, ResponseObject> Responses 
        { 
            get => _operation.Responses; 
            set => _operation.Responses = value; 
        }

        public List<Dictionary<string, List<string>>> Security 
        { 
            get => _operation.Security; 
            set => _operation.Security = value; 
        }
    }

    public class ParameterObjectAdapter : IParameterObject
    {
        private readonly ParameterObject _parameter;

        public ParameterObjectAdapter(ParameterObject parameter)
        {
            _parameter = parameter;
        }

        public string Name 
        { 
            get => _parameter.Name; 
            set => _parameter.Name = value; 
        }

        public string In 
        { 
            get => _parameter.In; 
            set => _parameter.In = value; 
        }

        public string Description 
        { 
            get => _parameter.Description; 
            set => _parameter.Description = value; 
        }

        public bool Required 
        { 
            get => _parameter.Required; 
            set => _parameter.Required = value; 
        }

        public SchemaObject Schema 
        { 
            get => _parameter.Schema; 
            set => _parameter.Schema = value; 
        }
    }

    public class RequestBodyObjectAdapter : IRequestBodyObject
    {
        private readonly RequestBodyObject _requestBody;

        public RequestBodyObjectAdapter(RequestBodyObject requestBody)
        {
            _requestBody = requestBody;
        }

        public string Description 
        { 
            get => _requestBody.Description; 
            set => _requestBody.Description = value; 
        }

        public bool Required 
        { 
            get => _requestBody.Required; 
            set => _requestBody.Required = value; 
        }

        public Dictionary<string, MediaTypeObject> Content 
        { 
            get => _requestBody.Content; 
            set => _requestBody.Content = value; 
        }
    }

    public class ResponseObjectAdapter : IResponseObject
    {
        private readonly ResponseObject _response;

        public ResponseObjectAdapter(ResponseObject response)
        {
            _response = response;
        }

        public string Description 
        { 
            get => _response.Description; 
            set => _response.Description = value; 
        }

        public Dictionary<string, MediaTypeObject> Content 
        { 
            get => _response.Content; 
            set => _response.Content = value; 
        }

        public Dictionary<string, HeaderObject> Headers 
        { 
            get => _response.Headers; 
            set => _response.Headers = value; 
        }
    }

    public class MediaTypeObjectAdapter : IMediaTypeObject
    {
        private readonly MediaTypeObject _mediaType;

        public MediaTypeObjectAdapter(MediaTypeObject mediaType)
        {
            _mediaType = mediaType;
        }

        public SchemaObject Schema 
        { 
            get => _mediaType.Schema; 
            set => _mediaType.Schema = value; 
        }

        public object Example 
        { 
            get => _mediaType.Example; 
            set => _mediaType.Example = value; 
        }

        public Dictionary<string, ExampleObject> Examples 
        { 
            get => _mediaType.Examples; 
            set => _mediaType.Examples = value; 
        }
    }

    public class HeaderObjectAdapter : IHeaderObject
    {
        private readonly HeaderObject _header;

        public HeaderObjectAdapter(HeaderObject header)
        {
            _header = header;
        }

        public string Description 
        { 
            get => _header.Description; 
            set => _header.Description = value; 
        }

        public SchemaObject Schema 
        { 
            get => _header.Schema; 
            set => _header.Schema = value; 
        }
    }

    public class ExampleObjectAdapter : IExampleObject
    {
        private readonly ExampleObject _example;

        public ExampleObjectAdapter(ExampleObject example)
        {
            _example = example;
        }

        public string Summary 
        { 
            get => _example.Summary; 
            set => _example.Summary = value; 
        }

        public string Description 
        { 
            get => _example.Description; 
            set => _example.Description = value; 
        }

        public object Value 
        { 
            get => _example.Value; 
            set => _example.Value = value; 
        }

        public string ExternalValue 
        { 
            get => _example.ExternalValue; 
            set => _example.ExternalValue = value; 
        }
    }

    public class SecuritySchemeObjectAdapter : ISecuritySchemeObject
    {
        private readonly SecuritySchemeObject _securityScheme;

        public SecuritySchemeObjectAdapter(SecuritySchemeObject securityScheme)
        {
            _securityScheme = securityScheme;
        }

        public string Type 
        { 
            get => _securityScheme.Type; 
            set => _securityScheme.Type = value; 
        }

        public string Description 
        { 
            get => _securityScheme.Description; 
            set => _securityScheme.Description = value; 
        }

        public string Name 
        { 
            get => _securityScheme.Name; 
            set => _securityScheme.Name = value; 
        }

        public string In 
        { 
            get => _securityScheme.In; 
            set => _securityScheme.In = value; 
        }

        public string Scheme 
        { 
            get => _securityScheme.Scheme; 
            set => _securityScheme.Scheme = value; 
        }

        public string BearerFormat 
        { 
            get => _securityScheme.BearerFormat; 
            set => _securityScheme.BearerFormat = value; 
        }

        public OAuthFlowsObject Flows 
        { 
            get => _securityScheme.Flows; 
            set => _securityScheme.Flows = value; 
        }
    }

    public class OAuthFlowsObjectAdapter : IOAuthFlowsObject
    {
        private readonly OAuthFlowsObject _oAuthFlows;

        public OAuthFlowsObjectAdapter(OAuthFlowsObject oAuthFlows)
        {
            _oAuthFlows = oAuthFlows;
        }

        public OAuthFlowObject Implicit 
        { 
            get => _oAuthFlows.Implicit; 
            set => _oAuthFlows.Implicit = value; 
        }

        public OAuthFlowObject Password 
        { 
            get => _oAuthFlows.Password; 
            set => _oAuthFlows.Password = value; 
        }

        public OAuthFlowObject ClientCredentials 
        { 
            get => _oAuthFlows.ClientCredentials; 
            set => _oAuthFlows.ClientCredentials = value; 
        }

        public OAuthFlowObject AuthorizationCode 
        { 
            get => _oAuthFlows.AuthorizationCode; 
            set => _oAuthFlows.AuthorizationCode = value; 
        }
    }

    public class OAuthFlowObjectAdapter : IOAuthFlowObject
    {
        private readonly OAuthFlowObject _oAuthFlow;

        public OAuthFlowObjectAdapter(OAuthFlowObject oAuthFlow)
        {
            _oAuthFlow = oAuthFlow;
        }

        public string AuthorizationUrl 
        { 
            get => _oAuthFlow.AuthorizationUrl; 
            set => _oAuthFlow.AuthorizationUrl = value; 
        }

        public string TokenUrl 
        { 
            get => _oAuthFlow.TokenUrl; 
            set => _oAuthFlow.TokenUrl = value; 
        }

        public string RefreshUrl 
        { 
            get => _oAuthFlow.RefreshUrl; 
            set => _oAuthFlow.RefreshUrl = value; 
        }

        public Dictionary<string, string> Scopes 
        { 
            get => _oAuthFlow.Scopes; 
            set => _oAuthFlow.Scopes = value; 
        }
    }

    public class TagObjectAdapter : ITagObject
    {
        private readonly TagObject _tag;

        public TagObjectAdapter(TagObject tag)
        {
            _tag = tag;
        }

        public string Name 
        { 
            get => _tag.Name; 
            set => _tag.Name = value; 
        }

        public string Description 
        { 
            get => _tag.Description; 
            set => _tag.Description = value; 
        }

        public ExternalDocumentationObject ExternalDocs 
        { 
            get => _tag.ExternalDocs; 
            set => _tag.ExternalDocs = value; 
        }
    }

    public class ExternalDocumentationObjectAdapter : IExternalDocumentationObject
    {
        private readonly ExternalDocumentationObject _externalDocs;

        public ExternalDocumentationObjectAdapter(ExternalDocumentationObject externalDocs)
        {
            _externalDocs = externalDocs;
        }

        public string Description 
        { 
            get => _externalDocs.Description; 
            set => _externalDocs.Description = value; 
        }

        public string Url 
        { 
            get => _externalDocs.Url; 
            set => _externalDocs.Url = value; 
        }
    }
}
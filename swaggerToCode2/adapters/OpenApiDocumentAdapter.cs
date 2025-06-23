using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using OpenApi.Models;
using swaggerToCode2.providers;
using System.Linq;

namespace SwaggerToCode.Adapters
{
    public class OpenApiDocumentAdapter : IOpenApiDocument
    {
        private readonly IOpenApiDocument _document;
        private readonly AdapterProvider _adapterProvider;

        public OpenApiDocumentAdapter(IOpenApiDocument document, AdapterProvider adapterProvider)
        {
            _document = document;
            _adapterProvider = adapterProvider;
        }

        public string OpenApi 
        { 
            get => _document.OpenApi; 
            set => _document.OpenApi = value; 
        }

        public IInfoObject Info 
        { 
            get => _document.Info is InfoObject info ? _adapterProvider.CreateInfoObjectAdapter(info) : null; 
            set => _document.Info = value; 
        }

        public List<IServerObject> Servers 
        { 
            get => _document.Servers.Select(server => _adapterProvider.CreateServerObjectAdapter(server as ServerObject)).ToList(); 
            set => _document.Servers = value; 
        }

        public Dictionary<string, IPathItemObject> Paths 
        { 
            get => _document.Paths.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreatePathItemObjectAdapter(kvp.Value as PathItemObject) as IPathItemObject); 
            set => _document.Paths = value; 
        }

        public IComponentsObject Components 
        { 
            get => _document.Components is ComponentsObject components ? 
                  _adapterProvider.CreateComponentsObjectAdapter(components) : null; 
            set => _document.Components = value; 
        }

        public List<Dictionary<string, List<string>>> Security 
        { 
            get => _document.Security; 
            set => _document.Security = value; 
        }

        public List<ITagObject> Tags 
        { 
            get => _document.Tags.Select(tag => 
                  _adapterProvider.CreateTagObjectAdapter(tag as TagObject) as ITagObject).ToList(); 
            set => _document.Tags = value; 
        }

        public IExternalDocumentationObject ExternalDocs 
        { 
            get => _document.ExternalDocs is ExternalDocumentationObject externalDocs ? 
                  _adapterProvider.CreateExternalDocumentationObjectAdapter(externalDocs) : null; 
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
        private readonly IInfoObject _info;
        private readonly AdapterProvider _adapterProvider;

        public InfoObjectAdapter(IInfoObject info, AdapterProvider adapterProvider)
        {
            _info = info;
            _adapterProvider = adapterProvider;
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
        private readonly IContactObject _contact;
        private readonly AdapterProvider _adapterProvider;

        public ContactObjectAdapter(IContactObject contact, AdapterProvider adapterProvider)
        {
            _contact = contact;
            _adapterProvider = adapterProvider;
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
        private readonly ILicenseObject _license;
        private readonly AdapterProvider _adapterProvider;

        public LicenseObjectAdapter(ILicenseObject license, AdapterProvider adapterProvider)
        {
            _license = license;
            _adapterProvider = adapterProvider;
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
        private readonly IServerObject _server;
        private readonly AdapterProvider _adapterProvider;

        public ServerObjectAdapter(IServerObject server, AdapterProvider adapterProvider)
        {
            _server = server;
            _adapterProvider = adapterProvider;
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
            get => _server.Variables.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateServerVariableObjectAdapter(kvp.Value) as ServerVariableObject); 
            set => _server.Variables = value; 
        }
    }

    public class ServerVariableObjectAdapter : IServerVariableObject
    {
        private readonly IServerVariableObject _serverVariable;
        private readonly AdapterProvider _adapterProvider;

        public ServerVariableObjectAdapter(IServerVariableObject serverVariable, AdapterProvider adapterProvider)
        {
            _serverVariable = serverVariable;
            _adapterProvider = adapterProvider;
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
        private readonly IComponentsObject _components;
        private readonly AdapterProvider _adapterProvider;

        public ComponentsObjectAdapter(IComponentsObject components, AdapterProvider adapterProvider)
        {
            _components = components;
            _adapterProvider = adapterProvider;
        }

        public Dictionary<string, SchemaObject> Schemas 
        { 
            get => _components.Schemas.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateSchemaObjectAdapter(kvp.Value) as SchemaObject); 
            set => _components.Schemas = value; 
        }

        public Dictionary<string, ResponseObject> Responses 
        { 
            get => _components.Responses.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateResponseObjectAdapter(kvp.Value) as ResponseObject); 
            set => _components.Responses = value; 
        }

        public Dictionary<string, ParameterObject> Parameters 
        { 
            get => _components.Parameters.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateParameterObjectAdapter(kvp.Value) as ParameterObject); 
            set => _components.Parameters = value; 
        }

        public Dictionary<string, RequestBodyObject> RequestBodies 
        { 
            get => _components.RequestBodies.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateRequestBodyObjectAdapter(kvp.Value) as RequestBodyObject); 
            set => _components.RequestBodies = value; 
        }

        public Dictionary<string, SecuritySchemeObject> SecuritySchemes 
        { 
            get => _components.SecuritySchemes.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateSecuritySchemeObjectAdapter(kvp.Value) as SecuritySchemeObject); 
            set => _components.SecuritySchemes = value; 
        }
    }

    public class SchemaObjectAdapter : ISchemaObject
    {
        private readonly ISchemaObject _schema;
        private readonly AdapterProvider _adapterProvider;

        public SchemaObjectAdapter(ISchemaObject schema, AdapterProvider adapterProvider)
        {
            _schema = schema;
            _adapterProvider = adapterProvider;
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
            get => _schema.Properties.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateSchemaObjectAdapter(kvp.Value) as SchemaObject); 
            set => _schema.Properties = value; 
        }

        public SchemaObject Items 
        { 
            get => _schema.Items != null ? 
                  _adapterProvider.CreateSchemaObjectAdapter(_schema.Items) as SchemaObject : null; 
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
            get => _schema.AllOf?.Select(schema => 
                  _adapterProvider.CreateSchemaObjectAdapter(schema) as SchemaObject).ToList(); 
            set => _schema.AllOf = value; 
        }

        public List<SchemaObject> OneOf 
        { 
            get => _schema.OneOf?.Select(schema => 
                  _adapterProvider.CreateSchemaObjectAdapter(schema) as SchemaObject).ToList(); 
            set => _schema.OneOf = value; 
        }

        public List<SchemaObject> AnyOf 
        { 
            get => _schema.AnyOf?.Select(schema => 
                  _adapterProvider.CreateSchemaObjectAdapter(schema) as SchemaObject).ToList(); 
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
            get => _schema.Discriminator != null ? 
                  _adapterProvider.CreateDiscriminatorObjectAdapter(_schema.Discriminator) as DiscriminatorObject : null; 
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
            get => _schema.ReferenceSchemaObject != null ? 
                  _adapterProvider.CreateSchemaObjectAdapter(_schema.ReferenceSchemaObject) as SchemaObject : null; 
            set => _schema.ReferenceSchemaObject = value; 
        }

        public List<SchemaObjectField> Fields => _schema.Fields.Select(field => 
                                                _adapterProvider.CreateSchemaObjectFieldAdapter(field) as SchemaObjectField).ToList();
    }

    public class SchemaObjectFieldAdapter : ISchemaObjectField
    {
        private readonly ISchemaObjectField _field;
        private readonly AdapterProvider _adapterProvider;

        public SchemaObjectFieldAdapter(ISchemaObjectField field, AdapterProvider adapterProvider)
        {
            _field = field;
            _adapterProvider = adapterProvider;
        }

        public SchemaObject Parent => _field.Parent != null ? 
                                     _adapterProvider.CreateSchemaObjectAdapter(_field.Parent) as SchemaObject : null;

        public string Name => _field.Name;

        public SchemaObject Field => _field.Field != null ? 
                                    _adapterProvider.CreateSchemaObjectAdapter(_field.Field) as SchemaObject : null;
    }

    public class DiscriminatorObjectAdapter : IDiscriminatorObject
    {
        private readonly IDiscriminatorObject _discriminator;
        private readonly AdapterProvider _adapterProvider;

        public DiscriminatorObjectAdapter(IDiscriminatorObject discriminator, AdapterProvider adapterProvider)
        {
            _discriminator = discriminator;
            _adapterProvider = adapterProvider;
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
        private readonly IPathItemObject _pathItem;
        private readonly AdapterProvider _adapterProvider;

        public PathItemObjectAdapter(IPathItemObject pathItem, AdapterProvider adapterProvider)
        {
            _pathItem = pathItem;
            _adapterProvider = adapterProvider;
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
            get => _pathItem.Get != null ? 
                  _adapterProvider.CreateOperationObjectAdapter(_pathItem.Get) as OperationObject : null; 
            set => _pathItem.Get = value; 
        }

        public OperationObject Put 
        { 
            get => _pathItem.Put != null ? 
                  _adapterProvider.CreateOperationObjectAdapter(_pathItem.Put) as OperationObject : null; 
            set => _pathItem.Put = value; 
        }

        public OperationObject Post 
        { 
            get => _pathItem.Post != null ? 
                  _adapterProvider.CreateOperationObjectAdapter(_pathItem.Post) as OperationObject : null; 
            set => _pathItem.Post = value; 
        }

        public OperationObject Delete 
        { 
            get => _pathItem.Delete != null ? 
                  _adapterProvider.CreateOperationObjectAdapter(_pathItem.Delete) as OperationObject : null; 
            set => _pathItem.Delete = value; 
        }

        public List<ParameterObject> Parameters 
        { 
            get => _pathItem.Parameters?.Select(param => 
                  _adapterProvider.CreateParameterObjectAdapter(param) as ParameterObject).ToList(); 
            set => _pathItem.Parameters = value; 
        }
    }

    public class OperationObjectAdapter : IOperationObject
    {
        private readonly IOperationObject _operation;
        private readonly AdapterProvider _adapterProvider;

        public OperationObjectAdapter(IOperationObject operation, AdapterProvider adapterProvider)
        {
            _operation = operation;
            _adapterProvider = adapterProvider;
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
            get => _operation.Parameters?.Select(param => 
                  _adapterProvider.CreateParameterObjectAdapter(param) as ParameterObject).ToList(); 
            set => _operation.Parameters = value; 
        }

        public RequestBodyObject RequestBody 
        { 
            get => _operation.RequestBody != null ? 
                  _adapterProvider.CreateRequestBodyObjectAdapter(_operation.RequestBody) as RequestBodyObject : null; 
            set => _operation.RequestBody = value; 
        }

        public Dictionary<string, ResponseObject> Responses 
        { 
            get => _operation.Responses.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateResponseObjectAdapter(kvp.Value) as ResponseObject); 
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
        private readonly IParameterObject _parameter;
        private readonly AdapterProvider _adapterProvider;

        public ParameterObjectAdapter(IParameterObject parameter, AdapterProvider adapterProvider)
        {
            _parameter = parameter;
            _adapterProvider = adapterProvider;
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
            get => _parameter.Schema != null ? 
                  _adapterProvider.CreateSchemaObjectAdapter(_parameter.Schema) as SchemaObject : null; 
            set => _parameter.Schema = value; 
        }
    }

    public class RequestBodyObjectAdapter : IRequestBodyObject
    {
        private readonly IRequestBodyObject _requestBody;
        private readonly AdapterProvider _adapterProvider;

        public RequestBodyObjectAdapter(IRequestBodyObject requestBody, AdapterProvider adapterProvider)
        {
            _requestBody = requestBody;
            _adapterProvider = adapterProvider;
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
            get => _requestBody.Content.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateMediaTypeObjectAdapter(kvp.Value) as MediaTypeObject); 
            set => _requestBody.Content = value; 
        }
    }

    public class ResponseObjectAdapter : IResponseObject
    {
        private readonly IResponseObject _response;
        private readonly AdapterProvider _adapterProvider;

        public ResponseObjectAdapter(IResponseObject response, AdapterProvider adapterProvider)
        {
            _response = response;
            _adapterProvider = adapterProvider;
        }

        public string Description 
        { 
            get => _response.Description; 
            set => _response.Description = value; 
        }

        public Dictionary<string, MediaTypeObject> Content 
        { 
            get => _response.Content?.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateMediaTypeObjectAdapter(kvp.Value) as MediaTypeObject); 
            set => _response.Content = value; 
        }

        public Dictionary<string, HeaderObject> Headers 
        { 
            get => _response.Headers?.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateHeaderObjectAdapter(kvp.Value) as HeaderObject); 
            set => _response.Headers = value; 
        }
    }

    public class MediaTypeObjectAdapter : IMediaTypeObject
    {
        private readonly IMediaTypeObject _mediaType;
        private readonly AdapterProvider _adapterProvider;

        public MediaTypeObjectAdapter(IMediaTypeObject mediaType, AdapterProvider adapterProvider)
        {
            _mediaType = mediaType;
            _adapterProvider = adapterProvider;
        }

        public SchemaObject Schema 
        { 
            get => _mediaType.Schema != null ? 
                  _adapterProvider.CreateSchemaObjectAdapter(_mediaType.Schema) as SchemaObject : null; 
            set => _mediaType.Schema = value; 
        }

        public object Example 
        { 
            get => _mediaType.Example; 
            set => _mediaType.Example = value; 
        }

        public Dictionary<string, ExampleObject> Examples 
        { 
            get => _mediaType.Examples?.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _adapterProvider.CreateExampleObjectAdapter(kvp.Value) as ExampleObject); 
            set => _mediaType.Examples = value; 
        }
    }

    public class HeaderObjectAdapter : IHeaderObject
    {
        private readonly IHeaderObject _header;
        private readonly AdapterProvider _adapterProvider;

        public HeaderObjectAdapter(IHeaderObject header, AdapterProvider adapterProvider)
        {
            _header = header;
            _adapterProvider = adapterProvider;
        }

        public string Description 
        { 
            get => _header.Description; 
            set => _header.Description = value; 
        }

        public SchemaObject Schema 
        { 
            get => _header.Schema != null ? 
                  _adapterProvider.CreateSchemaObjectAdapter(_header.Schema) as SchemaObject : null; 
            set => _header.Schema = value; 
        }
    }

    public class ExampleObjectAdapter : IExampleObject
    {
        private readonly IExampleObject _example;
        private readonly AdapterProvider _adapterProvider;

        public ExampleObjectAdapter(IExampleObject example, AdapterProvider adapterProvider)
        {
            _example = example;
            _adapterProvider = adapterProvider;
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
        private readonly ISecuritySchemeObject _securityScheme;
        private readonly AdapterProvider _adapterProvider;

        public SecuritySchemeObjectAdapter(ISecuritySchemeObject securityScheme, AdapterProvider adapterProvider)
        {
            _securityScheme = securityScheme;
            _adapterProvider = adapterProvider;
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
            get => _securityScheme.Flows != null ? 
                  _adapterProvider.CreateOAuthFlowsObjectAdapter(_securityScheme.Flows) as OAuthFlowsObject : null; 
            set => _securityScheme.Flows = value; 
        }
    }

    public class OAuthFlowsObjectAdapter : IOAuthFlowsObject
    {
        private readonly IOAuthFlowsObject _oAuthFlows;
        private readonly AdapterProvider _adapterProvider;

        public OAuthFlowsObjectAdapter(IOAuthFlowsObject oAuthFlows, AdapterProvider adapterProvider)
        {
            _oAuthFlows = oAuthFlows;
            _adapterProvider = adapterProvider;
        }

        public OAuthFlowObject Implicit 
        { 
            get => _oAuthFlows.Implicit != null ? 
                  _adapterProvider.CreateOAuthFlowObjectAdapter(_oAuthFlows.Implicit) as OAuthFlowObject : null; 
            set => _oAuthFlows.Implicit = value; 
        }

        public OAuthFlowObject Password 
        { 
            get => _oAuthFlows.Password != null ? 
                  _adapterProvider.CreateOAuthFlowObjectAdapter(_oAuthFlows.Password) as OAuthFlowObject : null; 
            set => _oAuthFlows.Password = value; 
        }

        public OAuthFlowObject ClientCredentials 
        { 
            get => _oAuthFlows.ClientCredentials != null ? 
                  _adapterProvider.CreateOAuthFlowObjectAdapter(_oAuthFlows.ClientCredentials) as OAuthFlowObject : null; 
            set => _oAuthFlows.ClientCredentials = value; 
        }

        public OAuthFlowObject AuthorizationCode 
        { 
            get => _oAuthFlows.AuthorizationCode != null ? 
                  _adapterProvider.CreateOAuthFlowObjectAdapter(_oAuthFlows.AuthorizationCode) as OAuthFlowObject : null; 
            set => _oAuthFlows.AuthorizationCode = value; 
        }
    }

    public class OAuthFlowObjectAdapter : IOAuthFlowObject
    {
        private readonly IOAuthFlowObject _oAuthFlow;
        private readonly AdapterProvider _adapterProvider;

        public OAuthFlowObjectAdapter(IOAuthFlowObject oAuthFlow, AdapterProvider adapterProvider)
        {
            _oAuthFlow = oAuthFlow;
            _adapterProvider = adapterProvider;
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
        private readonly ITagObject _tag;
        private readonly AdapterProvider _adapterProvider;

        public TagObjectAdapter(ITagObject tag, AdapterProvider adapterProvider)
        {
            _tag = tag;
            _adapterProvider = adapterProvider;
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
            get => _tag.ExternalDocs != null ? 
                  _adapterProvider.CreateExternalDocumentationObjectAdapter(_tag.ExternalDocs) as ExternalDocumentationObject : null; 
            set => _tag.ExternalDocs = value; 
        }
    }

    public class ExternalDocumentationObjectAdapter : IExternalDocumentationObject
    {
        private readonly IExternalDocumentationObject _externalDocs;
        private readonly AdapterProvider _adapterProvider;

        public ExternalDocumentationObjectAdapter(IExternalDocumentationObject externalDocs, AdapterProvider adapterProvider)
        {
            _externalDocs = externalDocs;
            _adapterProvider = adapterProvider;
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
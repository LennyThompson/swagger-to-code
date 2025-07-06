using System.Collections;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.ObjectFactories;

namespace OpenApi.Models
{

    public class OpenApiObjectFactory : DefaultObjectFactory
    {
        // Map of interface types to their concrete implementation types
        private readonly Dictionary<Type, Type> _interfaceToImplementationMap;

        public OpenApiObjectFactory()
        {
            // Initialize the interface-to-implementation map
            _interfaceToImplementationMap = new Dictionary<Type, Type>
            {
                { typeof(IOpenApiDocument), typeof(OpenApiDocument) },
                { typeof(IInfoObject), typeof(InfoObject) },
                { typeof(IContactObject), typeof(ContactObject) },
                { typeof(ILicenseObject), typeof(LicenseObject) },
                { typeof(IServerObject), typeof(ServerObject) },
                { typeof(IServerVariableObject), typeof(ServerVariableObject) },
                { typeof(IComponentsObject), typeof(ComponentsObject) },
                { typeof(ISchemaObject), typeof(SchemaObject) },
                { typeof(IDiscriminatorObject), typeof(DiscriminatorObject) },
                { typeof(IPathItemObject), typeof(PathItemObject) },
                { typeof(IOperationObject), typeof(OperationObject) },
                { typeof(IParameterObject), typeof(ParameterObject) },
                { typeof(IRequestBodyObject), typeof(RequestBodyObject) },
                { typeof(IResponseObject), typeof(ResponseObject) },
                { typeof(IMediaTypeObject), typeof(MediaTypeObject) },
                { typeof(IHeaderObject), typeof(HeaderObject) },
                { typeof(IExampleObject), typeof(ExampleObject) },
                { typeof(ISecuritySchemeObject), typeof(SecuritySchemeObject) },
                { typeof(IOAuthFlowsObject), typeof(OAuthFlowsObject) },
                { typeof(IOAuthFlowObject), typeof(OAuthFlowObject) },
                { typeof(ITagObject), typeof(TagObject) },
                { typeof(IExternalDocumentationObject), typeof(ExternalDocumentationObject) }
            };
        }

        public override object Create(Type type)
        {
            // Check if we're dealing with an interface from our map
            if (type.IsInterface && _interfaceToImplementationMap.TryGetValue(type, out var implementationType))
            {
                return Activator.CreateInstance(implementationType);
            }
            
            // Otherwise, use the default implementation
            return base.Create(type);
        }
    }
}

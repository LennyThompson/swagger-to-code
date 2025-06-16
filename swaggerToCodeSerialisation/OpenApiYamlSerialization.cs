using System.Reflection;
using OpenApi.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

public class OpenApiYamlSerializer
{
    public static OpenApiDocument DeserializeFromYaml(string yaml)
    {
        var namingConvention = CamelCaseNamingConvention.Instance; // Or your desired convention
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(namingConvention)
            .WithNodeDeserializer(inner => 
                    new VendorExtensionNodeDeserializer(inner, namingConvention), // Pass the convention
                selector => 
                    selector.InsteadOf<ObjectNodeDeserializer>()
            )
            .Build();

        return deserializer.Deserialize<OpenApiDocument>(yaml);
    }
}

public class VendorExtensionNodeDeserializer : INodeDeserializer
{
    private readonly INodeDeserializer _innerDeserializer;
    private readonly INamingConvention _namingConvention;
    private static readonly Dictionary<string, PropertyInfo> _schemaObjectPropertyMapCache = new();
    private static readonly object _cacheLock = new();

    public VendorExtensionNodeDeserializer(INodeDeserializer innerDeserializer, INamingConvention namingConvention)
    {
        _innerDeserializer = innerDeserializer ?? throw new ArgumentNullException(nameof(innerDeserializer));
        _namingConvention = namingConvention ?? throw new ArgumentNullException(nameof(namingConvention));
        BuildPropertyMapCache(typeof(SchemaObject), _namingConvention);
    }
    
    private static void BuildPropertyMapCache(Type type, INamingConvention convention)
    {
        // Simple cache key, assumes convention instance is consistent or doesn't affect mapping significantly for this purpose.
        // A more robust key might involve convention.GetType().FullName.
        // For this example, we assume one map for SchemaObject is sufficient if convention is stable.
        if (!_schemaObjectPropertyMapCache.Any()) 
        {
            lock (_cacheLock)
            {
                if (!_schemaObjectPropertyMapCache.Any()) // Double-check lock
                {
                    foreach (var propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (!propInfo.CanWrite) continue;

                        var yamlMemberAttribute = propInfo.GetCustomAttribute<YamlMemberAttribute>();
                        if (yamlMemberAttribute?.Alias != null)
                        {
                            _schemaObjectPropertyMapCache[yamlMemberAttribute.Alias] = propInfo;
                        }
                        // else
                        // {
                        //     // Apply naming convention to C# property name to get expected YAML name
                        //     var yamlName = convention.Apply(propInfo.Name);
                        //     _schemaObjectPropertyMapCache[yamlName] = propInfo;
                        // }
                    }
                }
            }
        }
    }

    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer objectDeserializer)
    {
        if (expectedType != typeof(SchemaObject))
        {
            return _innerDeserializer.Deserialize(reader, expectedType, nestedObjectDeserializer, out value, objectDeserializer);
        }

        var schema = new SchemaObject();
        reader.Expect<MappingStart>();
        
        while (!reader.Accept<MappingEnd>())
        {
            var propertyNameScalar = reader.Expect<Scalar>();
            var propertyName = propertyNameScalar.Value;

            if (string.IsNullOrEmpty(propertyName)) // Should not happen with valid YAML
            {
                reader.SkipThisAndNestedEvents();
                continue;
            }

            if (propertyName.StartsWith("x-"))
            {
                object? extensionValue = nestedObjectDeserializer(reader, typeof(object));
                schema[propertyName] = extensionValue;
            }
            else
            {
                if (_schemaObjectPropertyMapCache.TryGetValue(propertyName, out var propertyInfo))
                {
                    object? propertyValue = nestedObjectDeserializer(reader, propertyInfo.PropertyType);
                    propertyInfo.SetValue(schema, propertyValue);
                }
                else
                {
                    // Property name from YAML not found in our map for SchemaObject
                    reader.SkipThisAndNestedEvents();
                }
            }
        }

        reader.Expect<MappingEnd>();
        value = schema;
        return true;
    }
}
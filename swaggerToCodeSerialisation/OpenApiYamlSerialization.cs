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
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithNodeDeserializer((inner) => 
                new VendorExtensionNodeDeserializer(inner), 
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

    public VendorExtensionNodeDeserializer(INodeDeserializer innerDeserializer)
    {
        _innerDeserializer = innerDeserializer;
    }

    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer objectDeserializer)
    {
        if (expectedType != typeof(SchemaObject))
        {
            return _innerDeserializer.Deserialize(reader, expectedType, nestedObjectDeserializer, out value, objectDeserializer);
        }

        var schema = new SchemaObject();
        var mapping = reader.Expect<MappingStart>();
        
        while (!reader.Accept<MappingEnd>())
        {
            var property = reader.Expect<Scalar>();
            var propertyName = property.Value;

            if (propertyName.StartsWith("x-"))
            {
                // Handle vendor extension
                object? extensionValue = nestedObjectDeserializer(reader, typeof(object));
                schema[propertyName] = extensionValue;
            }
            else
            {
                // Handle regular properties using reflection
                var propertyInfo = typeof(SchemaObject).GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    object? propertyValue = nestedObjectDeserializer(reader, propertyInfo.PropertyType);
                    propertyInfo.SetValue(schema, propertyValue);
                }
                else
                {
                    // Skip unknown property
                    reader.SkipThisAndNestedEvents();
                }
            }
        }

        reader.Expect<MappingEnd>();
        value = schema;
        return true;
    }
}
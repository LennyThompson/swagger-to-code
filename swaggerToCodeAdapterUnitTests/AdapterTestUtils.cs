using OpenApi.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace swaggerToCodeAdapterUnitTests
{
    public static class AdapterTestUtils
    {
        /// <summary>
        /// Deserializes a YAML string into a SchemaObject
        /// </summary>
        /// <param name="yaml">The YAML string to deserialize</param>
        /// <returns>The deserialized schema object</returns>
        public static ISchemaObject DeserializeSchema(string yaml)
        {
            var namingConvention = CamelCaseNamingConvention.Instance; // Or your desired convention
            var deserializer = new DeserializerBuilder()
                .WithObjectFactory(new OpenApiObjectFactory())
                .WithNodeDeserializer(inner => 
                        new VendorExtensionNodeDeserializer(inner, namingConvention), // Pass the convention
                    selector => 
                        selector.InsteadOf<ObjectNodeDeserializer>()
                )
                .Build();

            return deserializer.Deserialize<SchemaObject>(yaml);
        }
    }
}
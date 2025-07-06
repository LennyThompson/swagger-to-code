using NUnit.Framework;
using Moq;
using OpenApi.Models;
using swaggerToCode2.providers;
using SwaggerToCode.Adapters;
using SwaggerToCode.Models;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace SwaggerToCode.Tests.Adapters
{
    [TestFixture]
    public class SchemaObjectAdapterYamlTests
    {
        private Mock<AdapterProvider> _mockAdapterProvider;
        private Mock<TypeAdapterProvider> _mockTypeAdapterProvider;
        private Mock<TypeAdapter> _mockTypeAdapter;
        private TemplateConfig _templateConfig;

        [SetUp]
        public void Setup()
        {
            _mockAdapterProvider = new Mock<AdapterProvider>();
            _mockTypeAdapterProvider = new Mock<TypeAdapterProvider>();
            _mockTypeAdapter = new Mock<TypeAdapter>();
            _templateConfig = new TemplateConfig();

            // Set up the TypeAdapterProvider to return our mock TypeAdapter
            _mockTypeAdapterProvider
                .Setup(p => p.GetTypeAdapter(It.IsAny<TemplateConfig>(), It.IsAny<string>(), It.IsAny<ISchemaObject>()))
                .Returns(_mockTypeAdapter.Object);

            // Set up the AdapterProvider to create real SchemaObjectAdapter instances
            // _mockAdapterProvider
            //     .Setup(p => p.CreateSchemaObjectAdapter(It.IsAny<string>(), It.IsAny<ISchemaObject>()))
            //     .Returns((ISchemaObject schema) => new SchemaObjectAdapter(
            //         schema,
            //         _mockAdapterProvider.Object,
            //         _mockTypeAdapterProvider.Object,
            //         _templateConfig));
        }

        [Test]
        public void CreateAdapter_FromSimpleTypeYaml_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
type: string
format: email
description: Email address
";
            ISchemaObject schema = DeserializeSchema(yaml);

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);
            _mockTypeAdapter.Setup(ta => ta.IsSimpleType).Returns(true);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("string"));
            Assert.That(adapter.Format, Is.EqualTo("email"));
            Assert.That(adapter.Description, Is.EqualTo("Email address"));
            Assert.That(adapter.IsSimpleType, Is.True);
        }

        [Test]
        public void CreateAdapter_FromArrayTypeYaml_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
type: array
items:
  type: string
description: List of strings
";
            ISchemaObject schema = DeserializeSchema(yaml);
            
            // Mock the item adapter that will be created for the array items
            var mockItemsAdapter = new Mock<ISchemaObject>();
            mockItemsAdapter.Setup(m => m.Type).Returns("string");
            mockItemsAdapter.Setup(m => m.ObjectType).Returns(SchemaObjectType.String);
            
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns(mockItemsAdapter.Object);
            _mockTypeAdapter.Setup(ta => ta.IsArrayType).Returns(true);

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("array"));
            Assert.That(adapter.Description, Is.EqualTo("List of strings"));
            Assert.That(adapter.IsArrayType, Is.True);
            Assert.That(adapter.Items, Is.Not.Null);
        }

        [Test]
        public void CreateAdapter_FromObjectTypeYaml_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
type: object
properties:
  name:
    type: string
  age:
    type: integer
  isActive:
    type: boolean
required:
  - name
description: User object
";
            ISchemaObject schema = DeserializeSchema(yaml);
            
            // Reset the setup to pass through the real adapter creation
            _mockAdapterProvider.Reset();
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject s) => new SchemaObjectAdapter(
                    s,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));
            _mockTypeAdapter.Setup(ta => ta.IsObjectType).Returns(true);

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("object"));
            Assert.That(adapter.Description, Is.EqualTo("User object"));
            Assert.That(adapter.IsObjectType, Is.True);
            Assert.That(adapter.Properties, Is.Not.Null);
            Assert.That(adapter.Properties.Count, Is.EqualTo(3));
            Assert.That(adapter.Properties.ContainsKey("name"), Is.True);
            Assert.That(adapter.Properties.ContainsKey("age"), Is.True);
            Assert.That(adapter.Properties.ContainsKey("isActive"), Is.True);
            Assert.That(adapter.Required, Is.Not.Null);
            Assert.That(adapter.Required.Count, Is.EqualTo(1));
            Assert.That(adapter.Required[0], Is.EqualTo("name"));
        }

        [Test]
        public void CreateAdapter_FromReferenceYaml_CreatesCorrectAdapter()
        {
            const string yamlUser = @"
type: object
properties:
  name:
    type: string
  age:
    type: integer
  isActive:
    type: boolean
required:
  - name
description: User object
";
            ISchemaObject schemaUser = DeserializeSchema(yamlUser);
            schemaUser.Name = "User";
            
            // Reset the setup to pass through the real adapter creation
            _mockAdapterProvider.Reset();
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject s) => new SchemaObjectAdapter(
                    s,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));
            _mockAdapterProvider
                .Setup(p => p.FindSchemaByReference("#/components/schemas/User"))
                .Returns(() => schemaUser);

            // Arrange
            const string yaml = @"
$ref: '#/components/schemas/User'
";
            ISchemaObject schema = DeserializeSchema(yaml);

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // The reference schema will be replaced by the actual schema in the adapter
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Ref, Is.Empty);
            Assert.That(adapter.Name, Is.EqualTo("User"));
        }

        [Test]
        public void CreateAdapter_FromCompositionYaml_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
allOf:
  - $ref: '#/components/schemas/BaseObject'
  - type: object
    properties:
      additionalProp:
        type: string
";
            ISchemaObject schema = DeserializeSchema(yaml);
            
            // Reset and setup the adapter provider to create adapters for the allOf items
            _mockAdapterProvider.Reset();
            _mockAdapterProvider
                .Setup(ap => ap.FindSchemaByReference("#/components/schemas/BaseObject"))
                .Returns(() => {
                    // Create a mock BaseObject schema
                    var baseObjectSchema = new SchemaObject
                    {
                        Name = "BaseObject",
                        Type = "object",
                        Description = "This is the base object schema",
                        Properties = new Dictionary<string, ISchemaObject>
                        {
                            ["id"] = new SchemaObject
                            {
                                Type = "string",
                                Format = "uuid",
                                Description = "Unique identifier for the object"
                            },
                            ["createdAt"] = new SchemaObject
                            {
                                Type = "string",
                                Format = "date-time",
                                Description = "Creation timestamp"
                            },
                            ["updatedAt"] = new SchemaObject
                            {
                                Type = "string",
                                Format = "date-time",
                                Description = "Last update timestamp"
                            }
                        },
                        Required = new List<string> { "id" }
                    };
                
                    return baseObjectSchema;
                });
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject s) => new SchemaObjectAdapter(
                    s,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));
            

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Fields, Is.Not.Null);
            Assert.That(adapter.Fields.Count, Is.EqualTo(4));
            var listSchemas = adapter.Schemas;
            Assert.That(listSchemas.Count, Is.EqualTo(2));
            Assert.That(listSchemas[0].Name, Is.EqualTo("BaseObject"));
            Assert.That(listSchemas[1].Type, Is.EqualTo("object"));
        }

        [Test]
        public void CreateAdapter_FromEnumYaml_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
type: string
enum:
  - pending
  - active
  - inactive
description: User status
";
            ISchemaObject schema = DeserializeSchema(yaml);

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("string"));
            Assert.That(adapter.Description, Is.EqualTo("User status"));
            Assert.That(adapter.Enum, Is.Not.Null);
            Assert.That(adapter.Enum.Count, Is.EqualTo(3));
            Assert.That(adapter.Enum, Contains.Item("pending"));
            Assert.That(adapter.Enum, Contains.Item("active"));
            Assert.That(adapter.Enum, Contains.Item("inactive"));
        }

        [Test]
        [Ignore("Vendor extensions for x-nullable and x-custom-metadata are not handled")]

        public void CreateAdapter_FromYamlWithVendorExtensions_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
type: object
properties:
  name:
    type: string
x-nullable: true
x-custom-metadata: test value
";
            ISchemaObject schema = DeserializeSchema(yaml);
            
            // Set up the adapter provider for any object properties
            _mockAdapterProvider.Reset();
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject s) => new SchemaObjectAdapter(
                    s,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("object"));
            Assert.That(adapter.VendorExtensions, Is.Not.Null);
            Assert.That(adapter.VendorExtensions.Count, Is.EqualTo(2));
            Assert.That(adapter["x-nullable"], Is.EqualTo(true));
            Assert.That(adapter["x-custom-metadata"], Is.EqualTo("test value"));
        }

        [Test]
        public void CreateAdapter_FromComplexNestedYaml_CreatesCorrectAdapter()
        {
            // Arrange
            const string yaml = @"
type: object
properties:
  id:
    type: integer
    format: int64
  user:
    type: object
    properties:
      name:
        type: string
      email:
        type: string
        format: email
  tags:
    type: array
    items:
      type: string
  status:
    type: string
    enum:
      - pending
      - active
      - inactive
required:
  - id
  - user
description: Complex object with nested properties
";
            ISchemaObject schema = DeserializeSchema(yaml);
            
            // Set up the adapter provider to create adapters for nested properties
            _mockAdapterProvider.Reset();
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter(It.IsAny<string>(), It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject s) => new SchemaObjectAdapter(
                    s,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));

            // Act
            var adapter = new SchemaObjectAdapter(
                schema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("object"));
            Assert.That(adapter.Description, Is.EqualTo("Complex object with nested properties"));
            Assert.That(adapter.Properties, Is.Not.Null);
            Assert.That(adapter.Properties.Count, Is.EqualTo(4));
            Assert.That(adapter.Properties.ContainsKey("id"), Is.True);
            Assert.That(adapter.Properties.ContainsKey("user"), Is.True);
            Assert.That(adapter.Properties.ContainsKey("tags"), Is.True);
            Assert.That(adapter.Properties.ContainsKey("status"), Is.True);
            Assert.That(adapter.Required, Is.Not.Null);
            Assert.That(adapter.Required.Count, Is.EqualTo(2));
        }

        [Test]
        public void CreateAdapter_FromOpenApiDocument_CreatesCorrectSchemaAdapters()
        {
            // Arrange - Simple OpenAPI document with a schema
            const string yaml = @"
openapi: 3.0.0
info:
  title: Test API
  version: 1.0.0
components:
  schemas:
    User:
      type: object
      properties:
        id:
          type: integer
        name:
          type: string
      required:
        - id
";
            var document = OpenApiYamlSerializer.DeserializeFromYaml(yaml);
            var userSchema = document.Components.Schemas["User"];
            
            // Set up the adapter provider to create adapters for nested properties
            _mockAdapterProvider.Reset();
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject s) => new SchemaObjectAdapter(
                    s,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));

            // Act
            var adapter = new SchemaObjectAdapter(
                userSchema,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig);

            // Assert
            Assert.That(adapter, Is.Not.Null);
            Assert.That(adapter.Type, Is.EqualTo("object"));
            Assert.That(adapter.Properties, Is.Not.Null);
            Assert.That(adapter.Properties.Count, Is.EqualTo(2));
            Assert.That(adapter.Properties.ContainsKey("id"), Is.True);
            Assert.That(adapter.Properties.ContainsKey("name"), Is.True);
            Assert.That(adapter.Required, Is.Not.Null);
            Assert.That(adapter.Required.Count, Is.EqualTo(1));
            Assert.That(adapter.Required[0], Is.EqualTo("id"));
            Assert.That(adapter.Fields.Count, Is.EqualTo(2));
        }

        // Helper method to deserialize YAML to SchemaObject
        private ISchemaObject DeserializeSchema(string yaml)
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
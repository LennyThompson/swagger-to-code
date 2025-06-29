using NUnit.Framework;
using Moq;
using OpenApi.Models;
using swaggerToCode2.providers;
using SwaggerToCode.Adapters;
using SwaggerToCode.Models;
using System.Collections.Generic;

namespace SwaggerToCode.Tests.Adapters
{
    [TestFixture]
    public class SchemaObjectAdapterTests
    {
        private Mock<ISchemaObject> _mockSchema;
        private Mock<AdapterProvider> _mockAdapterProvider;
        private Mock<TypeAdapterProvider> _mockTypeAdapterProvider;
        private Mock<TypeAdapter> _mockTypeAdapter;
        private TemplateConfig _templateConfig;
        private SchemaObjectAdapter _adapter;

        [SetUp]
        public void Setup()
        {
            _mockSchema = new Mock<ISchemaObject>();
            _mockAdapterProvider = new Mock<AdapterProvider>();
            _mockTypeAdapterProvider = new Mock<TypeAdapterProvider>();
            _mockTypeAdapter = new Mock<TypeAdapter>();
            _templateConfig = new TemplateConfig();

            // Set up the schema with a name to avoid null reference
            _mockSchema.Setup(s => s.Name).Returns("TestSchema");

            _mockTypeAdapterProvider
                .Setup(p => p.GetTypeAdapter(It.IsAny<TemplateConfig>(), It.IsAny<string>(), It.IsAny<ISchemaObject>()))
                .Returns(_mockTypeAdapter.Object);

            _adapter = new SchemaObjectAdapter(
                _mockSchema.Object,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig
            );
        }

        [Test]
        public void Constructor_InitializesAdapter()
        {
            // Assert
            Assert.That(_adapter, Is.Not.Null);
            Assert.That(_adapter.TypeAdapter, Is.EqualTo(_mockTypeAdapter.Object));
        }

        [Test]
        public void Type_GetSetsSchemaType()
        {
            // Arrange
            string testType = "string";
            _mockSchema.Setup(s => s.Type).Returns(testType);

            // Act
            var result = _adapter.Type;
            _adapter.Type = "object";

            // Assert
            Assert.That(result, Is.EqualTo(testType));
            _mockSchema.VerifySet(s => s.Type = "object", Times.Once);
        }

        [Test]
        public void Properties_GetReturnsWrappedProperties()
        {
            // Arrange
            var mockProperty = new Mock<ISchemaObject>();
            var properties = new Dictionary<string, ISchemaObject>
            {
                { "testProperty", mockProperty.Object }
            };
            var mockPropertyAdapter = new Mock<ISchemaObject>();

            _mockSchema.Setup(s => s.Properties).Returns(properties);
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter(mockProperty.Object))
                .Returns(mockPropertyAdapter.Object);

            // Act
            var result = _adapter.Properties;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result["testProperty"], Is.EqualTo(mockPropertyAdapter.Object));
        }

        [Test]
        public void Properties_ReturnsNullWhenSchemaPropertiesIsNull()
        {
            // Arrange
            _mockSchema.Setup(s => s.Properties).Returns((Dictionary<string, ISchemaObject>)null);

            // Act
            var result = _adapter.Properties;

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void TypeName_ForwardsToTypeAdapter()
        {
            // Arrange
            string expectedTypeName = "TestType";
            _mockTypeAdapter.Setup(a => a.TypeName).Returns(expectedTypeName);

            // Act
            var result = _adapter.TypeName;

            // Assert
            Assert.That(result, Is.EqualTo(expectedTypeName));
        }

        // Tests for simple types
        [TestCase("string")]
        [TestCase("integer")]
        [TestCase("boolean")]
        [TestCase("number")]
        public void IsSimpleType_WithSimpleTypes_ForwardsToSchema(string type)
        {
            // Arrange
            _mockSchema.Setup(s => s.Type).Returns(type);
            _mockSchema.Setup(s => s.IsSimpleType).Returns(true);

            // Act
            var result = _adapter.IsSimpleType;

            // Assert
            Assert.That(result, Is.True);
            _mockSchema.VerifyGet(s => s.IsSimpleType, Times.Once);
            
            result = _adapter.IsReference;
            Assert.That(result, Is.False);
            _mockSchema.VerifyGet(s => s.IsReference, Times.Once);
        }

        [Test]
        public void IsSimpleType_WithObjectType_ReturnsFalse()
        {
            // Arrange
            _mockSchema.Setup(s => s.Type).Returns("object");
            _mockSchema.Setup(s => s.IsSimpleType).Returns(false);

            // Act
            var result = _adapter.IsSimpleType;

            // Assert
            Assert.That(result, Is.False);
            _mockSchema.VerifyGet(s => s.IsSimpleType, Times.Once);
        }

        // Tests for object types
        [Test]
        public void IsObjectType_WithObjectType_ForwardsToSchema()
        {
            // Arrange
            _mockSchema.Setup(s => s.Type).Returns("object");
            _mockSchema.Setup(s => s.IsObjectType).Returns(true);

            // Act
            var result = _adapter.IsObjectType;

            // Assert
            Assert.That(result, Is.True);
            _mockSchema.VerifyGet(s => s.IsObjectType, Times.Once);
        }

        [Test]
        public void IsObjectType_WithComplexObject_ChecksForProperties()
        {
            // Arrange
            _mockSchema.Setup(s => s.Type).Returns("object");
            _mockSchema.Setup(s => s.IsObjectType).Returns(true);
            
            var mockProperty1 = new Mock<ISchemaObject>();
            var mockProperty2 = new Mock<ISchemaObject>();
            var properties = new Dictionary<string, ISchemaObject>
            {
                { "prop1", mockProperty1.Object },
                { "prop2", mockProperty2.Object }
            };
            _mockSchema.Setup(s => s.Properties).Returns(properties);
            
            // Mock adapter creation for properties
            var mockProp1Adapter = new Mock<ISchemaObject>();
            var mockProp2Adapter = new Mock<ISchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockProperty1.Object))
                .Returns(mockProp1Adapter.Object);
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockProperty2.Object))
                .Returns(mockProp2Adapter.Object);

            // Act
            var isObjectType = _adapter.IsObjectType;
            var adaptedProperties = _adapter.Properties;

            // Assert
            Assert.That(isObjectType, Is.True);
            Assert.That(adaptedProperties, Is.Not.Null);
            Assert.That(adaptedProperties.Count, Is.EqualTo(2));
            Assert.That(adaptedProperties["prop1"], Is.EqualTo(mockProp1Adapter.Object));
            Assert.That(adaptedProperties["prop2"], Is.EqualTo(mockProp2Adapter.Object));
        }

        // Tests for array types
        [Test]
        public void IsArrayType_WithArrayType_ForwardsToSchema()
        {
            // Arrange
            _mockSchema.Setup(s => s.Type).Returns("array");
            _mockSchema.Setup(s => s.IsArrayType).Returns(true);

            // Act
            var result = _adapter.IsArrayType;

            // Assert
            Assert.That(result, Is.True);
            _mockSchema.VerifyGet(s => s.IsArrayType, Times.Once);
        }

        [Test]
        public void Items_WithArrayOfSimpleType_HandlesItemsCorrectly()
        {
            // Arrange
            var mockItems = new Mock<ISchemaObject>();
            mockItems.Setup(s => s.Type).Returns("string");
            mockItems.Setup(s => s.IsSimpleType).Returns(true);
            
            _mockSchema.Setup(s => s.Type).Returns("array");
            _mockSchema.Setup(s => s.IsArrayType).Returns(true);
            _mockSchema.Setup(s => s.Items).Returns(mockItems.Object);
            
            var mockItemsAdapter = new Mock<SchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockItems.Object))
                .Returns(mockItemsAdapter.Object);

            // Act
            var isArrayType = _adapter.IsArrayType;
            var items = _adapter.Items;

            // Assert
            Assert.That(isArrayType, Is.True);
            Assert.That(items, Is.EqualTo(mockItemsAdapter.Object));
        }

        [Test]
        public void Items_WithArrayOfObjects_HandlesItemsCorrectly()
        {
            // Arrange
            var mockItems = new Mock<ISchemaObject>();
            mockItems.Setup(s => s.Type).Returns("object");
            mockItems.Setup(s => s.IsObjectType).Returns(true);
            
            _mockSchema.Setup(s => s.Type).Returns("array");
            _mockSchema.Setup(s => s.IsArrayType).Returns(true);
            _mockSchema.Setup(s => s.Items).Returns(mockItems.Object);
            
            var mockItemsAdapter = new Mock<SchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockItems.Object))
                .Returns(mockItemsAdapter.Object);

            // Act
            var isArrayType = _adapter.IsArrayType;
            var items = _adapter.Items;

            // Assert
            Assert.That(isArrayType, Is.True);
            Assert.That(items, Is.EqualTo(mockItemsAdapter.Object));
        }

        // Testing references
        [Test]
        public void IsReference_WithReference_ForwardsToSchema()
        {
            // Arrange
            _mockSchema.Setup(s => s.Ref).Returns("#/components/schemas/TestSchema");
            _mockSchema.Setup(s => s.IsReference).Returns(true);

            // Act
            var result = _adapter.IsReference;

            // Assert
            Assert.That(result, Is.True);
            _mockSchema.VerifyGet(s => s.IsReference, Times.Once);
        }

        [Test]
        public void ReferenceSchemaObject_WithReference_ReturnsWrappedObject()
        {
            // Arrange
            var mockRefSchema = new Mock<ISchemaObject>();
            _mockSchema.Setup(s => s.ReferenceSchemaObject).Returns(mockRefSchema.Object);
            
            var mockRefAdapter = new Mock<SchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockRefSchema.Object))
                .Returns(mockRefAdapter.Object);

            // Act
            var result = _adapter.ReferenceSchemaObject;

            // Assert
            Assert.That(result, Is.EqualTo(mockRefAdapter.Object));
        }

        // Tests for composition with allOf, oneOf, anyOf
        [Test]
        public void AllOf_ReturnsWrappedSchemas()
        {
            // Arrange
            var mockSchema1 = new Mock<ISchemaObject>();
            var mockSchema2 = new Mock<ISchemaObject>();
            var allOfList = new List<ISchemaObject> { mockSchema1.Object, mockSchema2.Object };
            
            _mockSchema.Setup(s => s.AllOf).Returns(allOfList);
            
            var mockAdapter1 = new Mock<ISchemaObject>();
            var mockAdapter2 = new Mock<ISchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockSchema1.Object))
                .Returns(mockAdapter1.Object);
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockSchema2.Object))
                .Returns(mockAdapter2.Object);

            // Act
            var result = _adapter.AllOf;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(mockAdapter1.Object));
            Assert.That(result[1], Is.EqualTo(mockAdapter2.Object));
        }

        [Test]
        public void OneOf_ReturnsWrappedSchemas()
        {
            // Arrange
            var mockSchema1 = new Mock<ISchemaObject>();
            var mockSchema2 = new Mock<ISchemaObject>();
            var oneOfList = new List<ISchemaObject> { mockSchema1.Object, mockSchema2.Object };
            
            _mockSchema.Setup(s => s.OneOf).Returns(oneOfList);
            
            var mockAdapter1 = new Mock<ISchemaObject>();
            var mockAdapter2 = new Mock<ISchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockSchema1.Object))
                .Returns(mockAdapter1.Object);
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockSchema2.Object))
                .Returns(mockAdapter2.Object);

            // Act
            var result = _adapter.OneOf;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(mockAdapter1.Object));
            Assert.That(result[1], Is.EqualTo(mockAdapter2.Object));
        }

        [Test]
        public void AnyOf_ReturnsWrappedSchemas()
        {
            // Arrange
            var mockSchema1 = new Mock<ISchemaObject>();
            var mockSchema2 = new Mock<ISchemaObject>();
            var anyOfList = new List<ISchemaObject> { mockSchema1.Object, mockSchema2.Object };
            
            _mockSchema.Setup(s => s.AnyOf).Returns(anyOfList);
            
            var mockAdapter1 = new Mock<ISchemaObject>();
            var mockAdapter2 = new Mock<ISchemaObject>();
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockSchema1.Object))
                .Returns(mockAdapter1.Object);
            _mockAdapterProvider.Setup(p => p.CreateSchemaObjectAdapter(mockSchema2.Object))
                .Returns(mockAdapter2.Object);

            // Act
            var result = _adapter.AnyOf;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(mockAdapter1.Object));
            Assert.That(result[1], Is.EqualTo(mockAdapter2.Object));
        }

        // Test for vendor extensions
        [Test]
        public void VendorExtensions_ForwardsToSchema()
        {
            // Arrange
            var extensions = new Dictionary<string, object>
            {
                { "x-nullable", true },
                { "x-custom", "value" }
            };
            
            _mockSchema.Setup(s => s.VendorExtensions).Returns(extensions);

            // Act
            var result = _adapter.VendorExtensions;

            // Assert
            Assert.That(result, Is.EqualTo(extensions));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["x-nullable"], Is.EqualTo(true));
            Assert.That(result["x-custom"], Is.EqualTo("value"));
        }

        // Test for indexer access
        [Test]
        public void Indexer_ForwardsToSchema()
        {
            // Arrange
            _mockSchema.Setup(s => s["x-test"]).Returns("test-value");

            // Act
            var result = _adapter["x-test"];
            _adapter["x-test"] = "new-value";

            // Assert
            Assert.That(result, Is.EqualTo("test-value"));
            _mockSchema.VerifySet(s => s["x-test"] = "new-value", Times.Once);
        }
    }
}
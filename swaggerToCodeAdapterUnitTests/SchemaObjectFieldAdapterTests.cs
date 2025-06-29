using NUnit.Framework;
using Moq;
using OpenApi.Models;
using swaggerToCode2.providers;
using SwaggerToCode.Adapters;
using SwaggerToCode.Models;

namespace SwaggerToCode.Tests.Adapters
{
    [TestFixture]
    public class SchemaObjectFieldAdapterTests
    {
        private Mock<ISchemaObjectField> _mockField;
        private Mock<AdapterProvider> _mockAdapterProvider;
        private Mock<TypeAdapterProvider> _mockTypeAdapterProvider;
        private Mock<TypeAdapter> _mockTypeAdapter;
        private Mock<ISchemaObject> _mockParent;
        private Mock<ISchemaObject> _mockSchemaObject;
        private TemplateConfig _templateConfig;
        private SchemaObjectFieldAdapter _adapter;

        [SetUp]
        public void Setup()
        {
            _mockField = new Mock<ISchemaObjectField>();
            _mockAdapterProvider = new Mock<AdapterProvider>();
            _mockTypeAdapterProvider = new Mock<TypeAdapterProvider>();
            _mockTypeAdapter = new Mock<TypeAdapter>();
            _mockParent = new Mock<ISchemaObject>();
            _mockSchemaObject = new Mock<ISchemaObject>();
            _templateConfig = new TemplateConfig();

            _mockField.Setup(f => f.Name).Returns("testField");
            _mockField.Setup(f => f.Field).Returns(_mockSchemaObject.Object);
            _mockField.Setup(f => f.Parent).Returns(_mockParent.Object);

            // Create a concrete SchemaObjectAdapter for _fieldAdapter to avoid mocking issues
            var mockSchemaAdapter = new Mock<SchemaObjectAdapter>(
                _mockSchemaObject.Object,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig
            );

            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter(_mockSchemaObject.Object))
                .Returns(mockSchemaAdapter.Object);

            _mockTypeAdapterProvider
                .Setup(p => p.GetTypeAdapter(It.IsAny<TemplateConfig>(), It.IsAny<string>(), It.IsAny<SchemaObjectAdapter>()))
                .Returns(_mockTypeAdapter.Object);

            _adapter = new SchemaObjectFieldAdapter(
                _mockField.Object,
                _mockAdapterProvider.Object,
                _mockTypeAdapterProvider.Object,
                _templateConfig
            );
        }

        [Test]
        public void Name_ReturnsFieldName()
        {
            // Act
            var result = _adapter.Name;

            // Assert
            Assert.That(result, Is.EqualTo("testField"));
        }

        [Test]
        public void OutputName_ForwardsToTypeAdapter()
        {
            // Arrange
            string expectedOutputName = "TestOutput";
            _mockTypeAdapter.Setup(a => a.OutputName).Returns(expectedOutputName);

            // Act
            var result = _adapter.OutputName;

            // Assert
            Assert.That(result, Is.EqualTo(expectedOutputName));
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
    }
}
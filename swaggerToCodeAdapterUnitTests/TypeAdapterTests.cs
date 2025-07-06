using NUnit.Framework;
using Moq;
using SwaggerToCode.Adapters;
using System;
using System.Text.Json;
using OpenApi.Models;
using swagger.utils.converters;
using SwaggerToCode.Models;
using swaggerToCode2.providers;
using swaggerToCodeAdapterUnitTests;

namespace SwaggerToCode.Tests.Adapters
{
    [TestFixture]
    public class TypeAdapterTests
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
            _templateConfig = new TemplateConfig()
            {
                Template = "generateCPPHeaderFromComponent",
                FileExtension = "h",
                PathRoot = "CppRoot",
                Path = "comp/include",
                Target = ["each-model"],
                Generators = ["schema-obj"],
                GenerateParams = ["model"],
                GenerateType = "cpp",
                Use = true
            };

            // Set up the TypeAdapterProvider to return our mock TypeAdapter
            _mockTypeAdapterProvider
                .Setup(p => p.GetTypeAdapter(It.IsAny<TemplateConfig>(), It.IsAny<string>(), It.IsAny<ISchemaObject>()))
                .Returns(_mockTypeAdapter.Object);

            // Set up the AdapterProvider to create real SchemaObjectAdapter instances
            _mockAdapterProvider
                .Setup(p => p.CreateSchemaObjectAdapter("", It.IsAny<ISchemaObject>()))
                .Returns((ISchemaObject schema) => new SchemaObjectAdapter(
                    schema,
                    _mockAdapterProvider.Object,
                    _mockTypeAdapterProvider.Object,
                    _templateConfig));
        }

        [Test]
        public void TypeAdapter_Interface_HasExpectedProperties()
        {
            // Arrange
            var mockAdapter = new Mock<TypeAdapter>();
            string expectedOutputName = "OutputName";
            string expectedTypeName = "TypeName";
            string expectedListTypeName = "List<TypeName>";
            string expectedMemberPrefix = "m_";

            mockAdapter.Setup(a => a.OutputName).Returns(expectedOutputName);
            mockAdapter.Setup(a => a.TypeName).Returns(expectedTypeName);
            mockAdapter.Setup(a => a.ListTypeName).Returns(expectedListTypeName);
            mockAdapter.Setup(a => a.MemberPrefix).Returns(expectedMemberPrefix);

            // Act
            var outputName = mockAdapter.Object.OutputName;
            var typeName = mockAdapter.Object.TypeName;
            var listTypeName = mockAdapter.Object.ListTypeName;
            var memberPrefix = mockAdapter.Object.MemberPrefix;

            // Assert
            Assert.That(outputName, Is.EqualTo(expectedOutputName));
            Assert.That(typeName, Is.EqualTo(expectedTypeName));
            Assert.That(listTypeName, Is.EqualTo(expectedListTypeName));
            Assert.That(memberPrefix, Is.EqualTo(expectedMemberPrefix));
        }

        [Test]
        public void CppTypeAdapter_HasExpectedComponentName()
        {
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
            ISchemaObject schemaObj = AdapterTestUtils.DeserializeSchema(yaml);
            schemaObj.Name = "Bob";
//            _mockTypeAdapter.Setups()
            var adapter = new CppTypeAdapter
                (
                    schemaObj.Name, 
                    schemaObj.Type, 
                    schemaObj.Format, 
                    (string strName) => new CamelToPascalCaseAdapter(new SnakeToCamelCaseAdapter(strName)),
                    schemaObj
                );
            
            Assert.That(adapter.Name, Is.EqualTo("Bob"));
            Assert.That(adapter.TypeName, Is.EqualTo("Bob"));
        }
    }
}
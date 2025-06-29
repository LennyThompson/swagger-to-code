using System;
using NUnit.Framework;
using OpenApi.Models;

namespace SwaggerToCode.Tests
{
    interface ITestInterface { }

    [TestFixture]
    public class OpenApiObjectFactoryTests
    {
        private OpenApiObjectFactory _factory;
        
        [SetUp]
        public void SetUp()
        {
            _factory = new OpenApiObjectFactory();
        }
        
        [Test]
        public void Create_Interface_ReturnsCorrectImplementation()
        {
            // Act
            var openApiDoc = _factory.Create(typeof(IOpenApiDocument));
            var infoObject = _factory.Create(typeof(IInfoObject));
            var schemaObject = _factory.Create(typeof(ISchemaObject));
            var componentsObject = _factory.Create(typeof(IComponentsObject));
            
            // Assert
            Assert.That(openApiDoc, Is.InstanceOf<OpenApiDocument>());
            Assert.That(infoObject, Is.InstanceOf<InfoObject>());
            Assert.That(schemaObject, Is.InstanceOf<SchemaObject>());
            Assert.That(componentsObject, Is.InstanceOf<ComponentsObject>());
        }
        
        [Test]
        public void Create_ConcreteType_ReturnsSameType()
        {
            // Act
            var openApiDoc = _factory.Create(typeof(OpenApiDocument));
            var infoObject = _factory.Create(typeof(InfoObject));
            
            // Assert
            Assert.That(openApiDoc, Is.InstanceOf<OpenApiDocument>());
            Assert.That(infoObject, Is.InstanceOf<InfoObject>());
        }
        
        [Test]
        public void Create_UnsupportedInterface_ThrowsException()
        {
            // Assert & Act
            Assert.That(() => _factory.Create(typeof(ITestInterface)), Throws.TypeOf<InvalidOperationException>());
        }
    }
}
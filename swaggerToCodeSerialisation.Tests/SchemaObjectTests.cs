using System.Collections.Generic;
using NUnit.Framework;
using OpenApi.Models;

namespace SwaggerToCode.Tests
{
    [TestFixture]
    public class SchemaObjectTests
    {
        [Test]
        public void IsReference_WithRefProperty_ReturnsTrue()
        {
            // Arrange
            var schema = new SchemaObject
            {
                Ref = "#/components/schemas/Test"
            };
            
            // Assert
            Assert.That(schema.IsReference, Is.True);
        }
        
        [Test]
        public void IsReference_WithoutRefProperty_ReturnsFalse()
        {
            // Arrange
            var schema = new SchemaObject
            {
                Type = "object"
            };
            
            // Assert
            Assert.That(schema.IsReference, Is.False);
        }
        
        [Test]
        public void IsSimpleType_WithSimpleType_ReturnsTrue()
        {
            // Arrange
            var simpleTypes = new[] { "string", "number", "integer", "boolean" };
            
            foreach (var type in simpleTypes)
            {
                var schema = new SchemaObject { Type = type };
                
                // Assert
                Assert.That(schema.IsSimpleType, Is.True, $"Type '{type}' should be considered a simple type");
            }
        }
        
        [Test]
        public void IsSimpleType_WithNonSimpleType_ReturnsFalse()
        {
            // Arrange
            var nonSimpleTypes = new[] { "object", "array", null };
            
            foreach (var type in nonSimpleTypes)
            {
                var schema = new SchemaObject { Type = type };
                
                // Assert
                Assert.That(schema.IsSimpleType, Is.False, $"Type '{type}' should not be considered a simple type");
            }
        }
        
        [Test]
        public void VendorExtensions_AddAndRetrieve_WorksCorrectly()
        {
            // Arrange
            var schema = new SchemaObject();
            
            // Act
            schema["x-nullable"] = true;
            schema["x-custom-property"] = "test value";
            
            // Assert
            Assert.That(schema.VendorExtensions.Count, Is.EqualTo(2));
            Assert.That(schema.VendorExtensions["x-nullable"], Is.EqualTo(true));
            Assert.That(schema.VendorExtensions["x-custom-property"], Is.EqualTo("test value"));
        }
    }
}
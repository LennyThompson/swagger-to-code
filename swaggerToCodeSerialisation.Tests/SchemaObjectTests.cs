using System.Collections.Generic;
using NUnit.Framework;
using OpenApi.Models;

namespace SwaggerToCode.Tests
{
    [TestFixture]
    public class SchemaObjectTests
    {
        
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
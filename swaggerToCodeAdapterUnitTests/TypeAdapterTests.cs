using NUnit.Framework;
using Moq;
using SwaggerToCode.Adapters;
using System;

namespace SwaggerToCode.Tests.Adapters
{
    [TestFixture]
    public class TypeAdapterTests
    {
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
    }
}
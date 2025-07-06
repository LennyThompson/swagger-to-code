using NUnit.Framework;
using OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Core;

namespace SwaggerToCode.Tests
{
    [TestFixture]
    public class OpenApiYamlSerializerTests
    {
        private string _simpleWabulatorYaml;
        private string _wabulatorYamlWithExtensions;
        private string _invalidYaml;
        private string _complexWabulatorYaml;
        private string _enumsYaml;
        private string _discriminatorYaml;
        
        [SetUp]
        public void Setup()
        {
            // Load test YAML files
            if (File.Exists("TestData/simple-wabulator.yaml"))
            {
                _simpleWabulatorYaml = File.ReadAllText("TestData/simple-wabulator.yaml");
                _wabulatorYamlWithExtensions = File.ReadAllText("TestData/vendor-extensions.yaml");
                _complexWabulatorYaml = File.ReadAllText("TestData/complex-references.yaml");
                _enumsYaml = File.ReadAllText("TestData/enums.yaml");
                _discriminatorYaml = File.ReadAllText("TestData/discriminator.yaml");
            }
            else
            {
                // Fallback to inline YAML strings if files are not available
                _simpleWabulatorYaml = @"
openapi: 3.0.0
info:
  title: Wabulator Web API
  description: API for managing and monitoring Wabulator state and configuration
  version: 1.0.0
paths:
  /api/config:
    get:
      summary: Get Wabulator configuration
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/WabConfig'
components:
  schemas:
    WabConfig:
      type: object
      description: Overall Wabulator configuration containing all settings
      required:
        - mode
        - id
        - config
        - mongo
        - control
      properties:
        mode:
          type: string
          example: 'replay'
        id:
          type: string
          example: 'wab-44-31580'
";

                _wabulatorYamlWithExtensions = @"
openapi: 3.0.0
info:
  title: Wabulator Web API
  version: 1.0.0
paths:
  /api/config:
    get:
      responses:
        '200':
          description: OK
components:
  schemas:
    Appenders:
      type: object
      description: 'Configuration for a logging appender'
      properties:
        name:
          type: string
          example: 'console'
      x-examples:
        consoleAppender:
          name: 'console'
          type: 'console'
          writeToStdout: true
          flushStdout: true
      additionalProperties: false
";

                _complexWabulatorYaml = @"
openapi: 3.0.0
info:
  title: Wabulator Web API
  description: API for managing and monitoring Wabulator state and configuration
  version: 1.0.0
paths:
  /api/wabulator-detail:
    get:
      summary: Get Wabulator details
      parameters:
        - name: wab_id
          in: query
          description: ID of the Wabulator to get details for
          schema:
            type: integer
            default: 0
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/WabulatorDetailsList'
components:
  schemas:
    WabulatorDetailsList:
      type: object
      required:
        - serverId
        - timestamp
        - wabulatorDetails
      properties:
        serverId:
          type: string
          example: 'server-001'
        timestamp:
          type: string
          format: date-time
          example: '2023-09-15T14:30:45Z'
        wabulatorDetails:
          type: array
          items:
            $ref: '#/components/schemas/WabulatorDetails'
    WabulatorDetails:
      type: object
      required:
        - id
        - currentState
        - previousState
        - lastStateChangeTime
        - stateHistory
        - wabettes
      properties:
        id:
          type: string
          example: 'wab-123'
        currentState:
          type: string
          oneOf:
            - $ref: '#/components/schemas/WabulatorState'
          example: 'CONNECTED'
        wabettes:
          type: array
          items:
            $ref: '#/components/schemas/WabetteDetails'
    WabulatorState:
      type: string
      enum:
        - 'NONE'
        - 'INITIALISING'
        - 'INITIALISED'
        - 'CONNECTING'
        - 'CONNECTED'
        - 'RUNNING'
        - 'STOPPED'
      example: 'RUNNING'
    WabetteDetails:
      type: object
      required:
        - siteId
        - currentState
        - lastStateChangeTime
      properties:
        siteId:
          type: integer
          format: int16
          example: 1
        currentState:
          type: string
          oneOf:
            - $ref: '#/components/schemas/WabetteState'
          example: 'RUNNING'
    WabetteState:
      type: string
      enum:
        - 'NONE'
        - 'INITIALISING'
        - 'RUNNING'
        - 'STOPPED'
      example: 'RUNNING'
";

                _enumsYaml = @"
openapi: 3.0.0
components:
  schemas:
    TransportType:
      type: string
      description: Valid transport types for POM connection
      enum:
        - 'rpc'
        - 'socket'
        - 'asio-socket'
      example: 'socket'
    WabulatorControlType:
      type: string
      description: Valid control config types
      enum:
        - 'server'
        - 'stand-alone'
        - 'client'
      example: 'stand-alone'
";

                _discriminatorYaml = @"
openapi: 3.0.0
components:
  schemas:
    EventStream:
      type: array
      description: Stream of events that can be of different types
      items:
        oneOf:
          - $ref: '#/components/schemas/WabulatorStateChangeEvent'
          - $ref: '#/components/schemas/WabetteStateChangeEvent'
        discriminator:
          propertyName: eventType
          mapping:
            wabulatorStateChange: '#/components/schemas/WabulatorStateChangeEvent'
            wabetteStateChange: '#/components/schemas/WabetteStateChangeEvent'
    WabulatorStateChangeEvent:
      type: object
      properties:
        eventType:
          type: string
          example: 'wabulatorStateChange'
    WabetteStateChangeEvent:
      type: object
      properties:
        eventType:
          type: string
          example: 'wabetteStateChange'
";
            }
            
            // Invalid YAML for testing error handling
            _invalidYaml = @"
openapi: 3.0.0
info:
  title: Wabulator Web API
  version 1.0.0  # Missing colon
paths:
  /api/config
    get:  # Indentation error
";
        }

        [Test]
        public void DeserializeFromYaml_ValidWabulatorYaml_ReturnsOpenApiDocument()
        {
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(_simpleWabulatorYaml);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OpenApi, Is.EqualTo("3.0.0"));
            Assert.That(result.Info.Title, Is.EqualTo("Wabulator Web API"));
            Assert.That(result.Info.Description, Is.EqualTo("API for managing and monitoring Wabulator state and configuration"));
            Assert.That(result.Info.Version, Is.EqualTo("1.0.0"));
            Assert.That(result.Paths, Is.Not.Null);
            Assert.That(result.Paths.ContainsKey("/api/config"), Is.True);
            Assert.That(result.Paths["/api/config"].Get, Is.Not.Null);
            Assert.That(result.Components, Is.Not.Null);
            Assert.That(result.Components.Schemas, Is.Not.Null);
            Assert.That(result.Components.Schemas.ContainsKey("WabConfig"), Is.True);
            
            var schema = result.Components.Schemas["WabConfig"];
            Assert.That(schema.Type, Is.EqualTo("object"));
            Assert.That(schema.Properties, Is.Not.Null);
            Assert.That(schema.Properties.ContainsKey("mode"), Is.True);
            Assert.That(schema.Properties.ContainsKey("id"), Is.True);
        }
        
        [Test]
        public void DeserializeFromYaml_WithVendorExtensions_HandlesExtensionsCorrectly()
        {
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(_wabulatorYamlWithExtensions);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Components, Is.Not.Null);
            Assert.That(result.Components.Schemas, Is.Not.Null);
            Assert.That(result.Components.Schemas.ContainsKey("Appenders"), Is.True);
            
            var schema = result.Components.Schemas["Appenders"];
            Assert.That(schema.VendorExtensions, Is.Not.Null);
            Assert.That(schema.VendorExtensions.ContainsKey("x-examples"), Is.True);
            
            // Check the x-examples extension
            var examples = schema.VendorExtensions["x-examples"] as Dictionary<object, object>;
            Assert.That(examples, Is.Not.Null);
            Assert.That(examples.ContainsKey("consoleAppender"), Is.True);
            
            var consoleAppender = examples["consoleAppender"] as Dictionary<object, object>;
            Assert.That(consoleAppender, Is.Not.Null);
            Assert.That(consoleAppender["name"], Is.EqualTo("console"));
            Assert.That(consoleAppender["type"], Is.EqualTo("console"));
            Assert.That(consoleAppender["writeToStdout"], Is.EqualTo("true"));
            Assert.That(consoleAppender["flushStdout"], Is.EqualTo("true"));
        }

        [Test]
        public void DeserializeFromYaml_InvalidYaml_ThrowsYamlException()
        {
            // Assert
            Assert.That(() => OpenApiYamlSerializer.DeserializeFromYaml(_invalidYaml), Throws.TypeOf<SemanticErrorException>());
        }
        
        [Test]
        public void DeserializeFromYaml_EmptyYaml_ThrowsException()
        {
            // Assert
            Assert.That(() => OpenApiYamlSerializer.DeserializeFromYaml(string.Empty), Is.Null);
        }
        
        [Test]
        public void DeserializeFromYaml_NullYaml_ThrowsArgumentNullException()
        {
            // Assert
            Assert.That(() => OpenApiYamlSerializer.DeserializeFromYaml(null), 
                Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void DeserializeFromYaml_ComplexWabulatorSchema_DeserializesCorrectly()
        {
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(_complexWabulatorYaml);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Components, Is.Not.Null);
            Assert.That(result.Components.Schemas, Is.Not.Null);
            
            // Check schema hierarchy is correctly deserialized
            Assert.That(result.Components.Schemas.ContainsKey("WabulatorDetailsList"), Is.True);
            Assert.That(result.Components.Schemas.ContainsKey("WabulatorDetails"), Is.True);
            Assert.That(result.Components.Schemas.ContainsKey("WabulatorState"), Is.True);
            Assert.That(result.Components.Schemas.ContainsKey("WabetteDetails"), Is.True);
            Assert.That(result.Components.Schemas.ContainsKey("WabetteState"), Is.True);
            
            // Check WabulatorDetailsList properties
            var detailsList = result.Components.Schemas["WabulatorDetailsList"];
            Assert.That(detailsList.Properties, Is.Not.Null);
            Assert.That(detailsList.Properties.ContainsKey("wabulatorDetails"), Is.True);
            
            // Check path parameter
            var path = result.Paths["/api/wabulator-detail"];
            Assert.That(path.Get, Is.Not.Null);
            Assert.That(path.Get.Parameters, Is.Not.Null);
            Assert.That(path.Get.Parameters.Count, Is.EqualTo(1));
            Assert.That(path.Get.Parameters[0].Name, Is.EqualTo("wab_id"));
            Assert.That(path.Get.Parameters[0].In, Is.EqualTo("query"));
            
            // Check enum values on WabulatorState
            var wabulatorState = result.Components.Schemas["WabulatorState"];
            Assert.That(wabulatorState.Enum, Is.Not.Null);
            Assert.That(wabulatorState.Enum.Count, Is.EqualTo(7));
            Assert.That(wabulatorState.Enum, Does.Contain("NONE"));
            Assert.That(wabulatorState.Enum, Does.Contain("CONNECTED"));
            Assert.That(wabulatorState.Enum, Does.Contain("RUNNING"));
        }
        
        [Test]
        public void DeserializeFromYaml_WithReferences_UpdatesSchemaReferences()
        {
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(_complexWabulatorYaml);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            
            // Check that references are updated
            var detailsList = result.Components.Schemas["WabulatorDetailsList"];
            var detailsProperty = detailsList.Properties["wabulatorDetails"];
            Assert.That(detailsProperty.Items, Is.Not.Null);
            Assert.That(detailsProperty.Items.Ref, Is.EqualTo("#/components/schemas/WabulatorDetails"));
            
            // Check that nested references are also resolved
            var wabulatorDetails = result.Components.Schemas["WabulatorDetails"];
            var wabettesProperty = wabulatorDetails.Properties["wabettes"];
            Assert.That(wabettesProperty.Items, Is.Not.Null);
        }
        
        [Test]
        public void DeserializeFromYaml_FromFile_DeserializesCorrectly()
        {
            // Arrange - Create a temporary file with YAML content
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, _simpleWabulatorYaml);
            
            try
            {
                // Act
                var yaml = File.ReadAllText(tempFile);
                var result = OpenApiYamlSerializer.DeserializeFromYaml(yaml);
                
                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Info.Title, Is.EqualTo("Wabulator Web API"));
            }
            finally
            {
                // Cleanup
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
        
        [Test]
        public void DeserializeFromYaml_WithEnums_DeserializesCorrectly()
        {
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(_enumsYaml);
            
            // Assert
            Assert.That(result.Components, Is.Not.Null);
            Assert.That(result.Components.Schemas, Is.Not.Null);
            
            var transportType = result.Components.Schemas["TransportType"];
            Assert.That(transportType, Is.Not.Null);
            Assert.That(transportType.Enum, Is.Not.Null);
            Assert.That(transportType.Enum.Count, Is.EqualTo(3));
            Assert.That(transportType.Enum, Does.Contain("rpc"));
            Assert.That(transportType.Enum, Does.Contain("socket"));
            Assert.That(transportType.Enum, Does.Contain("asio-socket"));
            
            var controlType = result.Components.Schemas["WabulatorControlType"];
            Assert.That(controlType, Is.Not.Null);
            Assert.That(controlType.Enum, Is.Not.Null);
            Assert.That(controlType.Enum.Count, Is.EqualTo(3));
            Assert.That(controlType.Enum, Does.Contain("server"));
            Assert.That(controlType.Enum, Does.Contain("stand-alone"));
            Assert.That(controlType.Enum, Does.Contain("client"));
        }
        
        [Test]
        public void DeserializeFromYaml_WithDiscriminator_DeserializesCorrectly()
        {
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(_discriminatorYaml);
            
            // Assert
            Assert.That(result.Components, Is.Not.Null);
            Assert.That(result.Components.Schemas, Is.Not.Null);
            
            var eventStream = result.Components.Schemas["EventStream"];
            Assert.That(eventStream, Is.Not.Null);
            Assert.That(eventStream.Items, Is.Not.Null);
            Assert.That(eventStream.Items.OneOf, Is.Not.Null);
            Assert.That(eventStream.Items.OneOf.Count, Is.EqualTo(2));
            
            Assert.That(eventStream.Items.Discriminator, Is.Not.Null);
            Assert.That(eventStream.Items.Discriminator.PropertyName, Is.EqualTo("eventType"));
            Assert.That(eventStream.Items.Discriminator.Mapping, Is.Not.Null);
            Assert.That(eventStream.Items.Discriminator.Mapping.Count, Is.EqualTo(2));
            Assert.That(eventStream.Items.Discriminator.Mapping["wabulatorStateChange"], 
                Is.EqualTo("#/components/schemas/WabulatorStateChangeEvent"));
            Assert.That(eventStream.Items.Discriminator.Mapping["wabetteStateChange"], 
                Is.EqualTo("#/components/schemas/WabetteStateChangeEvent"));
        }
        
        [Test]
        public void VendorExtensionNodeDeserializer_DeserializesCustomFields()
        {
            // This test specifically tests the VendorExtensionNodeDeserializer
            
            // Arrange
            string yamlWithCustomExtensions = @"
components:
  schemas:
    MongoConfig:
      type: object
      description: Configuration for MongoDB connection
      required:
        - url
        - db
      properties:
        url:
          type: string
          example: 'mongodb://localhost:27017'
      x-custom-object:
        connection-timeout: 30
        retry-count: 3
      x-custom-array:
        - replica1
        - replica2
      x-nullable: true
";
            
            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(yamlWithCustomExtensions);
            
            // Assert
            var schema = result.Components.Schemas["MongoConfig"];
            Assert.That(schema.VendorExtensions, Is.Not.Null);
            
            // Check object extension
            Assert.That(schema.VendorExtensions.ContainsKey("x-custom-object"), Is.True);
            var customObj = schema.VendorExtensions["x-custom-object"] as Dictionary<object, object>;
            Assert.That(customObj, Is.Not.Null);
            Assert.That(customObj["connection-timeout"], Is.EqualTo("30"));
            Assert.That(customObj["retry-count"], Is.EqualTo("3"));
            
            // Check array extension
            Assert.That(schema.VendorExtensions.ContainsKey("x-custom-array"), Is.True);
            var customArray = schema.VendorExtensions["x-custom-array"] as List<object>;
            Assert.That(customArray, Is.Not.Null);
            Assert.That(customArray.Count, Is.EqualTo(2));
            Assert.That(customArray[0], Is.EqualTo("replica1"));
            Assert.That(customArray[1], Is.EqualTo("replica2"));
            
            // Check boolean extension
            Assert.That(schema.VendorExtensions.ContainsKey("x-nullable"), Is.True);
            Assert.That(schema.VendorExtensions["x-nullable"], Is.EqualTo("true"));
        }

        [Test]
        public void DeserializeFromYaml_RealWorldSwaggerYaml_DeserializesCorrectly()
        {
            // This test uses a larger fragment of the actual swagger.yaml
            
            // Arrange
            string yamlContent = @"
openapi: 3.0.0
info:
  title: Wabulator Web API
  description: API for managing and monitoring Wabulator state and configuration
  version: 1.0.0
paths:
  /api/config:
    get:
      summary: Get Wabulator configuration
      description: Retrieves the current Wabulator configuration
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/WabConfig'
components:
  schemas:
    MongoConfig:
      type: object
      description: Configuration for MongoDB connection
      required:
        - url
        - db
      properties:
        url:
          type: string
          description: MongoDB connection string
          example: 'mongodb://localhost:27017'
        db:
          type: string
          description: Database name
          example: 'replay'
    WabConfig:
      type: object
      description: Overall Wabulator configuration containing all settings
      required:
        - mode
        - id
        - config
        - mongo
        - control
      properties:
        mode:
          type: string
          description: To be deprecated
          example: 'replay'
        id:
          type: string
          description: Id of the wabulator
          example: 'wab-44-31580'
        mongo:
          type: object
          allOf:
            - $ref: '#/components/schemas/MongoConfig'
";

            // Act
            var result = OpenApiYamlSerializer.DeserializeFromYaml(yamlContent);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OpenApi, Is.EqualTo("3.0.0"));
            Assert.That(result.Info.Title, Is.EqualTo("Wabulator Web API"));
            
            // Check components
            Assert.That(result.Components, Is.Not.Null);
            Assert.That(result.Components.Schemas, Is.Not.Null);
            Assert.That(result.Components.Schemas.ContainsKey("WabConfig"), Is.True);
            Assert.That(result.Components.Schemas.ContainsKey("MongoConfig"), Is.True);
            
            // Check WabConfig properties
            var wabConfig = result.Components.Schemas["WabConfig"];
            Assert.That(wabConfig.Properties, Is.Not.Null);
            Assert.That(wabConfig.Properties.ContainsKey("mode"), Is.True);
            Assert.That(wabConfig.Properties.ContainsKey("id"), Is.True);
            Assert.That(wabConfig.Properties.ContainsKey("mongo"), Is.True);
            
            // Check mongo reference
            var mongoProperty = wabConfig.Properties["mongo"];
            Assert.That(mongoProperty.AllOf, Is.Not.Null);
            Assert.That(mongoProperty.AllOf.Count, Is.EqualTo(1));
            Assert.That(mongoProperty.AllOf[0].Ref, Is.EqualTo("#/components/schemas/MongoConfig"));
        }
    }
}
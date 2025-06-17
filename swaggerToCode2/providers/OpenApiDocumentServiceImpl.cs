using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenApi.Models;
using SwaggerToCode.Models;
using SwaggerToCode.Services;

namespace SwaggerToCode.Services
{

    public class OpenApiDocumentProvider : IOpenApiDocumentProvider
    {
        private readonly IConfigurationReader _configReader;
        private readonly ILogger<OpenApiDocumentProvider> _logger;
        private readonly List<OpenApiDocument> _documents = new List<OpenApiDocument>();

        private readonly Dictionary<string, int> _filenameToIndexMap =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private bool _isLoaded = false;

        /// <summary>
        /// Gets a list of all loaded OpenAPI documents
        /// </summary>
        public IReadOnlyList<OpenApiDocument> Documents => _documents.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the OpenApiDocumentProvider class
        /// </summary>
        /// <param name="configReader">The configuration reader to get swagger file paths</param>
        /// <param name="logger">Logger for diagnostic information</param>
        public OpenApiDocumentProvider
        (
            IConfigurationReader configReader,
            ILogger<OpenApiDocumentProvider> logger
        )
        {
            _configReader = configReader ?? throw new ArgumentNullException(nameof(configReader));
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously loads all swagger files specified in the configuration
        /// </summary>
        public async Task LoadDocumentsAsync()
        {
            if (_isLoaded)
            {
                _logger?.LogInformation("OpenAPI documents are already loaded");
                return;
            }

            try
            {
                var config = _configReader.Configuration;

                var listSwaggerPaths = config?.SwaggerPaths ?? [];
                if (listSwaggerPaths.Count == 0)
                {
                    _logger?.LogWarning("No swagger files specified in configuration");
                    return;
                }

                _logger?.LogInformation($"Loading {listSwaggerPaths.Count} OpenAPI documents");

                for (int i = 0; i < listSwaggerPaths.Count; i++)
                {
                    string swaggerFile = listSwaggerPaths[i];
                    await LoadSwaggerFileAsync(swaggerFile, i);
                }

                _isLoaded = true;
                _logger?.LogInformation($"Successfully loaded {_documents.Count} OpenAPI documents");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading OpenAPI documents from configuration");
                throw;
            }
        }

        /// <summary>
        /// Gets an OpenAPI document by its index in the list
        /// </summary>
        /// <param name="index">Index of the document</param>
        /// <returns>The OpenAPI document at the specified index</returns>
        public OpenApiDocument GetDocumentByIndex(int index)
        {
            if (!_isLoaded)
            {
                throw new InvalidOperationException("Documents have not been loaded. Call LoadDocumentsAsync first.");
            }

            if (index < 0 || index >= _documents.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Index {index} is out of range. Valid range is 0-{_documents.Count - 1}");
            }

            return _documents[index];
        }

        /// <summary>
        /// Gets an OpenAPI document by the filename specified in the configuration
        /// </summary>
        /// <param name="filename">Name of the swagger file</param>
        /// <returns>The OpenAPI document matching the filename, or null if not found</returns>
        public OpenApiDocument GetDocumentByFilename(string filename)
        {
            if (!_isLoaded)
            {
                throw new InvalidOperationException("Documents have not been loaded. Call LoadDocumentsAsync first.");
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename cannot be null or empty", nameof(filename));
            }

            if (_filenameToIndexMap.TryGetValue(filename, out int index))
            {
                return _documents[index];
            }

            return null;
        }

        private async Task LoadSwaggerFileAsync(string swaggerFile, int index)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(swaggerFile))
                {
                    _logger?.LogWarning($"Swagger file path at index {index} is null or empty");
                    return;
                }

                _logger?.LogInformation($"Loading swagger file: {swaggerFile}");

                if (!File.Exists(swaggerFile))
                {
                    _logger?.LogError($"Swagger file not found: {swaggerFile}");
                    throw new FileNotFoundException($"Swagger file not found: {swaggerFile}", swaggerFile);
                }

                // Read file content
                string content = await File.ReadAllTextAsync(swaggerFile);

                // Determine file type and deserialize accordingly
                OpenApiDocument document;
                if (swaggerFile.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase))
                {
                    document = OpenApiYamlSerializer.DeserializeFromYaml(content);
                }
                else if (swaggerFile.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase) ||
                         swaggerFile.EndsWith(".yml", StringComparison.OrdinalIgnoreCase))
                {
                    document = OpenApiYamlSerializer.DeserializeFromYaml(content);
                }
                else
                {
                    _logger?.LogWarning($"Unknown file extension for {swaggerFile}. Attempting to parse as YAML.");
                    document = OpenApiYamlSerializer.DeserializeFromYaml(content);
                }

                // Update schema references
                document.UpdateSchemaReferences();

                // Add to collections
                _documents.Add(document);
                _filenameToIndexMap[swaggerFile] = _documents.Count - 1;

                _logger?.LogInformation($"Successfully loaded {swaggerFile} with {document.Paths.Count} paths");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error loading swagger file: {swaggerFile}");
                throw;
            }
        }
    }
}
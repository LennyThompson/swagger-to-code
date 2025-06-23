using Microsoft.Extensions.Logging;
using OpenApi.Models;
using SwaggerToCode.Models;
using SwaggerToCode.Services;

namespace swaggerToCode2.providers
{
    /// <summary>
    /// Implementation of ITemplateConfigContextProvider that manages the current template configuration context
    /// </summary>
    public class TemplateConfigContextProviderImpl : TemplateConfigContextProvider
    {
        private TemplateConfig? _currentTemplateConfig;
        private IOpenApiDocument? _currentOpenApiDocument;
        private readonly ILogger<TemplateConfigContextProviderImpl> _logger;
        private readonly IConfigurationReader _configService;
        
        public TemplateConfigContextProviderImpl(IConfigurationReader configService, ILogger<TemplateConfigContextProviderImpl> logger)
        {
            _configService = configService;
            _logger = logger;
        }
        
        public bool IsContextSet => _currentTemplateConfig != null && _currentOpenApiDocument != null;
        public TemplateConfig CurrentTemplateConfig => _currentTemplateConfig;
        public IOpenApiDocument CurrentOpenApiDocument => _currentOpenApiDocument;
        public void SetContext(TemplateConfig templateConfig, IOpenApiDocument openApiDocument)
        {
            _currentTemplateConfig = templateConfig;
            _currentOpenApiDocument = openApiDocument;
        }
        public void ClearContext()
        {
            _currentTemplateConfig = null;
            _currentOpenApiDocument = null;
        }
    }
}
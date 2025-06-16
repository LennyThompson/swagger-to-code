using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;

namespace SwaggerToCode.Services
{
    public class GenerateConfigReader : IConfigurationReader
    {
        private readonly ILogger<GenerateConfigReader> _logger;
        private GenerateConfig _cachedConfig;
        
        public string ConfigurationFilePath { get; }

        public GenerateConfigReader(string configFilePath, ILogger<GenerateConfigReader> logger = null)
        {
            ConfigurationFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
            _logger = logger;

            if (!File.Exists(configFilePath))
            {
                var errorMessage = $"Configuration file not found: {configFilePath}";
                _logger?.LogError(errorMessage);
                throw new FileNotFoundException(errorMessage, configFilePath);
            }
        }

        public GenerateConfig GetConfiguration()
        {
            if (_cachedConfig != null)
            {
                return _cachedConfig;
            }

            try
            {
                _cachedConfig = GenerateConfig.FromJsonFile(ConfigurationFilePath);
                
                if (_cachedConfig == null)
                {
                    throw new InvalidOperationException("Failed to load configuration file.");
                }

                return _cachedConfig;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error loading configuration from {ConfigurationFilePath}");
                throw;
            }
        }
    }
}
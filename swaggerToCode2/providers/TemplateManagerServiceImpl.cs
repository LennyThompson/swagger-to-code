using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Compiler;
using Antlr4.StringTemplate.Misc;
using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;

namespace SwaggerToCode.Services
{

    public class TemplateManagerService : ITemplateManagerService
    {
        private readonly IConfigurationReader _configService;
        private readonly ILogger<TemplateManagerService> _logger;
        private TemplateGroup _templateGroup;
        private bool _isInitialized = false;

        public TemplateManagerService(IConfigurationReader configService, ILogger<TemplateManagerService> logger)
        {
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _logger = logger;
            _templateGroup = new TemplateGroup();
        }

        public TemplateGroup? LoadTemplates()
        {
            if (_isInitialized)
            {
                return _templateGroup;
            }

            string templateRootPath = _configService.Configuration.TemplateRootPath;
            
            if (string.IsNullOrEmpty(templateRootPath))
            {
                _logger.LogError("Template root path is not configured");
                return null;
            }

            // We'll try to load template files from the template directory
            string[] templateFiles = Directory.GetFiles(templateRootPath, "*.stg", SearchOption.AllDirectories);

            foreach (var templateFile in templateFiles)
            {
                try
                {
                    // Load each .stg file into the template group
                    TemplateGroupFile groupFile = new TemplateGroupFile(templateFile);
                    _templateGroup.ImportTemplates(groupFile);
                }
                catch (TemplateException ex)
                {
                    _logger.LogError($"Error loading template file {templateFile}: {ex.Message}");
                    Console.WriteLine($"Error loading template file {templateFile}: {ex.Message}");
                }
            }

            _isInitialized = true;
            return _templateGroup;
        }

        public Template? GetTemplate(string templateName)
        {
            if (!_isInitialized)
            {
                LoadTemplates();
            }

            return _templateGroup.GetInstanceOf(templateName);
        }

        public string RenderTemplate<T>(string templateName, T model) where T : class
        {
            Template? template = GetTemplate(templateName);

            if (template != null)
            {
                // Add the model to the template
                if (model != null)
                {
                    var properties = model.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(model);
                        if (value != null)
                        {
                            template.Add(property.Name, value);
                        }
                    }
                }

                return template.Render();
            }
            _logger.LogError($"Unable to find template {templateName}");
            return "";
        }
    }

}
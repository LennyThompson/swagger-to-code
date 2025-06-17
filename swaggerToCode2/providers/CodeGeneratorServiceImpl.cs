
using Microsoft.Extensions.Logging;
using SwaggerToCode.Services;

namespace SwaggerToCode
{
        // Example usage in a generator class
    public class CodeGeneratorServiceImpl : CodeGeneratorService
    {
        private readonly IConfigurationReader _configService;
        private readonly ITemplateManagerService _templateManager;
        private readonly ILogger<CodeGeneratorServiceImpl> _logger;

        public CodeGeneratorServiceImpl
        (
            IConfigurationReader configService, 
            ITemplateManagerService templateManager,
            ILogger<CodeGeneratorServiceImpl> logger
        )
        {
            _configService = configService;
            _templateManager = templateManager;
            _logger = logger;
        }

        public void GenerateCode<T>(T model) where T : class
        {
            // Get active templates from configuration
            var templateConfigs = _configService.Configuration.TemplateConfigs;
            
            foreach (var templateConfig in templateConfigs)
            {
                // Check if the template is meant to be used
                if (!templateConfig.Use)
                    continue;

                try
                {
                    // Get the template parameters from configuration
                    var templateParams = templateConfig.GenerateParams;
                    
                    // Check if the model has all required parameters
                    bool hasAllParams = true;
                    var modelParams = new Dictionary<string, object>();
                    
                    foreach (var param in templateParams)
                    {
                        try
                        {
                            var value = ((IDictionary<string, object>)model)[param];
                            if (value != null)
                            {
                                modelParams[param] = value;
                            }
                            else
                            {
                                hasAllParams = false;
                                break;
                            }
                        }
                        catch
                        {
                            hasAllParams = false;
                            break;
                        }
                    }
                    
                    if (!hasAllParams)
                        continue;
                    
                    // Get the template name from configuration
                    string templateName = templateConfig.Template;
                    
                    // Render the template with the model
                    string generatedCode = _templateManager.RenderTemplate(templateName, model);
                    
                    // Determine output path
                    string outputRootPath = _configService.Configuration.GetRootPath(templateConfig.PathRoot);
                    string outputPath = Path.Combine(outputRootPath, templateConfig.Path);
                    
                    // Ensure directory exists
                    Directory.CreateDirectory(outputPath);
                    
                    // Create file name
                    string fileName = $"{model.name}{templateConfig.FileExtension}";
                    string filePath = Path.Combine(outputPath, fileName);
                    
                    // Write to file
                    File.WriteAllText(filePath, generatedCode);
                    
                    Console.WriteLine($"Generated: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating code for template {templateConfig.Template}: {ex.Message}");
                }
            }
        }
    }

}


using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;
using SwaggerToCode.Services;

namespace swaggerToCode2.providers;

public class OutputFileProviderImpl : OutputFileProvider
{
    private readonly IConfigurationReader _configService;
    private readonly TemplateConfigContextProvider _templateConfigContextProvider;
    private readonly ILogger<OutputFileProviderImpl> _logger;

    OutputFileProviderImpl
    (
        IConfigurationReader configService,
        TemplateConfigContextProvider templateConfigContextProvider,
        ILogger<OutputFileProviderImpl> logger
    )
    {
        _configService = configService;
        _templateConfigContextProvider = templateConfigContextProvider;
        _logger = logger;
    }

    public bool WriteToFile(GenerateTarget generateTarget, string strTargetName, string strRendered)
    {
        // Determine output path
        string outputRootPath = _configService.Configuration.GetRootPath(_templateConfigContextProvider.CurrentTemplateConfig.PathRoot);
        string outputPath = Path.Combine(outputRootPath, _templateConfigContextProvider.CurrentTemplateConfig.Path);

        try
        {
            Directory.CreateDirectory(outputPath);

            // Create file name
            string fileName = $"{strTargetName}.{_templateConfigContextProvider.CurrentTemplateConfig.FileExtension}";
            string filePath = Path.Combine(outputPath, fileName);

            // Write to file
            File.WriteAllText(filePath, strRendered);

            _logger.LogInformation($"Generated: {filePath}");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

        return false;
    }
}
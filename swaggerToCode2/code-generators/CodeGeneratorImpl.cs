using Microsoft.Extensions.Logging;
using SwaggerToCode.Models;
using SwaggerToCode.Services;
using swaggerToCode2.providers;

namespace swaggerToCode.code_generators;

public abstract class CodeGeneratorImpl : CodeGenerator
{
    protected readonly TemplateConfigContextProvider _templateConfigContextProvider;
    protected readonly ITemplateManagerService _templateManager;
    protected readonly OutputFileProvider _outputFileProvider;
    protected readonly ILogger<CodeGeneratorImpl> _logger;
    protected string _strName;

    public CodeGeneratorImpl
    (
        string strName,
        TemplateConfigContextProvider templateConfigContextProvider, 
        OutputFileProvider outputFileProvider,
        ILogger<CodeGeneratorImpl> logger
    )
    {
        _strName = strName;
        _templateConfigContextProvider = templateConfigContextProvider;
        _outputFileProvider = outputFileProvider;
        _logger = logger;
    }

    public string Name => _strName;
    public abstract bool GenerateAll();

    public bool Generate(GenerateTarget target)
    {
        try
        {
            // Get the template parameters from configuration
            var templateParams = _templateConfigContextProvider.CurrentTemplateConfig.GenerateParams;

            // Check if the model has all required parameters
            var modelParams = new Dictionary<string, object>();
            List<string> listMissingParams = new List<string>();
            foreach (var param in templateParams)
            {
                try
                {
                    var value = target.Parameters[param];
                    if (value != null)
                    {
                        modelParams[param] = value.Model;
                    }
                    else
                    {
                        listMissingParams.Add(param);
                        break;
                    }
                }
                catch
                {
                    listMissingParams.Add(param);
                    break;
                }
            }

            if (listMissingParams.Count > 0)
            {
                _logger.LogError($"Target {target.TargetName} has missing parameters {listMissingParams}");
                throw new MissingMemberException($"Required parameter missing: {listMissingParams}");
            }

            // Get the template name from configuration
            string templateName = _templateConfigContextProvider.CurrentTemplateConfig.Template;

            // Render the template with the model
            string strRendered = _templateManager.RenderTemplate(templateName, modelParams);
            if (!string.IsNullOrEmpty(strRendered))
            {
                return _outputFileProvider.WriteToFile(target, target.TargetName, strRendered);
            }
            else
            {
                _logger.LogError($"Target {target.TargetName} failed to render with template {target.TargetName}");
            }

        }
        catch (Exception exc)
        {
            _logger.LogError($"Target {target.TargetName} failed with exception {exc.Message}");
        }
        return false;
    }
}
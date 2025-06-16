using SwaggerToCode.Models;

namespace SwaggerToCode.Services
{
    public interface IConfigurationReader
    {
        GenerateConfig GetConfiguration();
        string ConfigurationFilePath { get; }
    }
}

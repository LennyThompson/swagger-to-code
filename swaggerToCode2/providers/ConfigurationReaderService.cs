using SwaggerToCode.Models;

namespace SwaggerToCode.Services
{
    public interface IConfigurationReader
    {
        GenerateConfig Configuration { get; }
        string ConfigurationFilePath { get; }
    }
}

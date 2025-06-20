using SwaggerToCode.Models;

namespace SwaggerToCode
{
    public interface CodeGeneratorService
    {
        void GenerateCode<T>(T model) where T : GenerateTarget;
    }
}
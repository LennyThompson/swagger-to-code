
using swaggerToCode.code_generators;

namespace SwaggerToCode
{

    public interface CodeGeneratorProvider
    {
        CodeGenerator? GetGenerator(string generatorId);
        Dictionary<string, CodeGenerator> AllGenerators { get; }
    }
}
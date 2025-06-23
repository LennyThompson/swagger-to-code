using SwaggerToCode.Models;

namespace swaggerToCode.code_generators;

public interface CodeGenerator
{
    string Name { get; }

    bool GenerateAll();
    bool Generate(GenerateTarget target);
}


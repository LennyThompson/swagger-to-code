using SwaggerToCode.Models;

namespace SwaggerToCode
{
    public interface CodeGeneratorService
    { 
        bool ProcessOpenApiDocuments(string outputDirectory);
    }
}